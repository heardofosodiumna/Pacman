using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour {

    public float speed;
    Vector3 moveDir = Vector3.zero;
    public spawnCheese pathScript;
    List<Vector2> path;
    Vector2 closestPoint;
    float aDist;
    float shortestDist;
    CharacterController controller;
    Quaternion turn;
    int targetIndex = 0;

    bool control = true;
    // Update is called once per frame

    void Start()
    {
        controller = GetComponent<CharacterController>();
        pathScript = pathScript.GetComponent<spawnCheese>();
        path = pathScript.points;
    }
    private void FixedUpdate () {
        if (Input.GetKeyDown("space"))
            control = !control;
        if (control)
        {
            if (transform.rotation.z >= .5 || transform.rotation.z <= -.5)
                gameObject.GetComponent<SpriteRenderer>().flipY = true;
            else
                gameObject.GetComponent<SpriteRenderer>().flipY = false;

           
            moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            moveDir = transform.TransformDirection(moveDir);
            moveDir = moveDir * speed;
            controller.Move(moveDir * Time.deltaTime);
        }else
        { 
            if (transform.rotation.z >= .5 || transform.rotation.z <= -.5)
                gameObject.GetComponent<SpriteRenderer>().flipY = true;
            else
                gameObject.GetComponent<SpriteRenderer>().flipY = false;

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
            Vector2 target = path[targetIndex];

            //Rotate towards target
            Vector2 vectorToTarget = target - (Vector2)transform.position;
            float new_angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            turn = Quaternion.AngleAxis(new_angle, Vector3.forward);
         
            //Move in current facing direction
            transform.rotation = Quaternion.Slerp(transform.rotation, turn, Time.deltaTime * 3);
            transform.Translate(Vector2.right * Time.deltaTime * speed);

        }

    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("cleared: " + path[targetIndex]);
        path.Remove(path[targetIndex]);
        Debug.Log("seeking: " + path[targetIndex]);
    }
    /*
     *  public SteeringAgent TargetAgent;
        public float FleeRadius = 1.0f;
        public bool DrawGizmos = false;

        public override Vector2 GetVelocity()
        {
                //dist =   distact  from player to targer
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