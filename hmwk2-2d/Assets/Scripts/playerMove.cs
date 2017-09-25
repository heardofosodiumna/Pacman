using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour {

    public float speed;
    Vector3 moveDir = Vector3.zero;
    public spawnCheese pathScript;
    List<Vector2> path;
    List<Vector2> completed;
    Vector2 closestPoint;
    float aDist;
    float shortestDist;
    bool wasEvading = false;
   // CharacterController controller;
    Quaternion turn;
    int targetIndex = 0;
    public GameObject enemy;
    public float evade_rad;
    float distEnemy;
    Rigidbody2D rb;
    int curr_path_index = 0;
    public float angle_slow;
    public float rot_speed;
    public int update_rate;
    Vector2 evade_target;
    int evading_dur;
    int update;
    public GameObject evade_circle;
    bool control = false;
    // Update is called once per frame

    void Start()
    {
        // controller = GetComponent<CharacterController>();
        update = 0;
        evading_dur = 0;
        completed = new List<Vector2>();
        pathScript = pathScript.GetComponent<spawnCheese>();
        rb = GetComponent<Rigidbody2D>();
        path = pathScript.points;
        GameObject evade_circ = Instantiate(evade_circle, transform.position, new Quaternion(0,0,0,0));
        evade_circ.transform.localScale = new Vector2(evade_rad*2, evade_rad*2);
        evade_circ.transform.parent = transform;
    }
    private void FixedUpdate () {
     
        //path follow
        if (transform.rotation.z >= .5 || transform.rotation.z <= -.5)
            gameObject.GetComponent<SpriteRenderer>().flipY = true;
        else
            gameObject.GetComponent<SpriteRenderer>().flipY = false;
        distEnemy = Vector2.Distance(enemy.transform.position, transform.position);
        //  Debug.Log("distEnemy: " + distEnemy);
        if (distEnemy > evade_rad && evading_dur <= 0)
        {
            update = 0;
            speed = 3;
            /*if(curr_path_index >= 0)
             {
                 if((Vector2)transform.position == path[curr_path_index])
                 {
                     curr_path_index++;
                 }

                 Vector2 target = path[curr_path_index];
                 //Rotate towards target
                 Vector2 vectorToTarget = target - (Vector2)transform.position;
                 float new_angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
                 turn = Quaternion.AngleAxis(new_angle, Vector3.forward);

                 //Move in current facing direction
                 transform.rotation = Quaternion.Slerp(transform.rotation, turn, Time.deltaTime * 3);
                 //rb.AddForce(transform.right * speed);
                 rb.velocity = (transform.right * speed);

             }*/
            closestPoint = path[0];
            shortestDist = Mathf.Sqrt(((closestPoint.x - transform.position.x) * (closestPoint.x - transform.position.x)) + ((closestPoint.y - transform.position.y) * (closestPoint.y - transform.position.y)));
            foreach (Vector2 p in path)
            {
                // Debug.Log("shortest is: " + shortestDist + " at point: " + closestPoint);

                aDist = Mathf.Sqrt(((p.x - transform.position.x) * (p.x - transform.position.x)) + ((p.y - transform.position.y) * (p.y - transform.position.y)));
                // Debug.Log("calculatiions is: " + aDist + " at point: " + p);

                if (aDist <= shortestDist)
                {
                    closestPoint = p;
                    shortestDist = aDist;
                }
            }
            // Debug.Log("closestPoint: " + closestPoint);
            //closestPoint;
            Vector2 target = closestPoint;

            //Rotate towards target
            Vector2 vectorToTarget = target - (Vector2)transform.position;
            float new_angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            turn = Quaternion.AngleAxis(new_angle, Vector3.forward);

            //Move in current facing direction
            transform.rotation = Quaternion.Slerp(transform.rotation, turn, Time.deltaTime * 3);
            //rb.AddForce(transform.right * speed);
            rb.velocity = (transform.right * speed); 
        }
        else
        {
            //evade
            foreach (Vector2 point in completed)
            { 
                path.Add(point);
            }
            completed = new List<Vector2>();

            if(evading_dur <= 0)
                evading_dur = 60;
            evading_dur--;
            speed = 4;
            //float t = distEnemy / enemy.GetComponent<AIScript>().speed;

            //using the distance to calulate predicted location
            update--;
            if (update <= 0)
            {
                Debug.Log("Here");
                Vector2 enemy_guess = enemy.transform.position;
                float dist = (enemy.transform.position - transform.position).magnitude;
                enemy_guess.x += enemy.GetComponent<Rigidbody2D>().velocity.x * dist / speed/2;
                enemy_guess.y += enemy.GetComponent<Rigidbody2D>().velocity.y * dist / speed/2;

                evade_target = enemy_guess;
                update = update_rate;
            }

            //using the direction check the angle that we must turn
            Vector2 angle_direction = (Vector2)transform.position - evade_target;
            float target_angle = Mathf.Atan2(angle_direction.y, angle_direction.x) * Mathf.Rad2Deg;
            Align(Quaternion.AngleAxis(target_angle, Vector3.forward));

            //move towards the enemy
            Flee(evade_target);

            wasEvading = true;
        }
    }

    private void Align(Quaternion target_orientation)
    {
        float angle_dist = target_orientation.eulerAngles.z - transform.rotation.eulerAngles.z;
        if (angle_dist < 0)
            angle_dist *= -1;
        if (angle_dist < angle_slow)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, target_orientation, Time.deltaTime * rot_speed / (angle_dist * angle_slow));
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, target_orientation, Time.deltaTime * rot_speed);
        }
    }

    private void Flee(Vector2 target_pos)
    {
        Vector2 direction = ((Vector2)transform.position- target_pos).normalized;
        transform.GetComponent<Rigidbody2D>().velocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {   
       if((Vector2)coll.transform.position == closestPoint)
       {
            Debug.Log("removed " + closestPoint);
            completed.Add(path[targetIndex]);
            path.Remove(closestPoint);

       }
    }
    /*
     *  public SteeringAgent TargetAgent;
        public float FleeRadius = 1.0f;
        public bool DrawGizmos = false;

        public override Vector2 GetVelocity()
        {
                //dist =   distact  from enemy to targer
            float distance = Vector3.Distance(transform.position, TargetAgent.transform.position);

            //if distance is <flee
            if (distance < FleeRadius)
            {
                // t is distance divied by max velocity
                float t = distance / TargetAgent.MaxVelocity;
                //point will be the Vec2 of target position + currentvel times t
                Vector2 targetPoint = (Vector2)TargetAgent.transform.position + TargetAgent.CurrentVelocity * t;
                //return the (target point - position).normlized * max veloxcity) - agent current velocity
                return -(((targetPoint - (Vector2)transform.position).normalized * agent.MaxVelocity) - agent.CurrentVelocity);
            }
            else
                return Vector2.zero;
        }

    */
}