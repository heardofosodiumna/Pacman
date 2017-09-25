using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIScript : MonoBehaviour {

    //target we wish to follow
    public string state = "Dynamic Wander";
    public GameObject player;
    Rigidbody2D rb;
    public float speed;
    public float rot_speed;
    public float circle_diam;
    public float circle_dist;
    Vector2 target;
    int update = 0;
    public int update_dur;
    public GameObject circle;
    public GameObject spot;
    GameObject curr_spot;
    GameObject curr_circle;
    Quaternion turn;
    public int pursue_rad;
    GameObject curr_chase_target;
    public int d_arrive_dist;
    System.Random rnd = new System.Random();
    public float time_to_target;
    public float angle_slow;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject d_arrive_circ = Instantiate(circle, player.transform.position, new Quaternion(0, 0, 0, 0));
        d_arrive_circ.transform.parent = player.transform;
        d_arrive_circ.transform.localScale = new Vector2(d_arrive_dist*2 / player.transform.localScale.x, d_arrive_dist*2 / player.transform.localScale.y);
        curr_chase_target = Instantiate(spot, new Vector2(0, 0), new Quaternion(0, 0, 0, 0));
        curr_chase_target.SetActive(false);
        curr_spot = Instantiate(spot, new Vector2(0, 0), new Quaternion(0, 0, 0, 0));
        GameObject track_circ = Instantiate(circle, new Vector2(transform.position.x, transform.position.y), new Quaternion(0, 0, 0, 0));
        track_circ.transform.parent = gameObject.transform;
        track_circ.transform.localScale = 2 * pursue_rad * new Vector2(1 / transform.localScale.x, 1 / transform.localScale.y);

        curr_circle = Instantiate(circle, new Vector2(0,0), new Quaternion(0, 0, 0, 0));
        curr_spot.transform.parent = curr_circle.transform;
        curr_spot.transform.localScale = new Vector2(1/transform.localScale.x, 1/transform.localScale.y);
        curr_circle.transform.Rotate(new Vector2(0, 0));
        curr_circle.transform.localScale = new Vector2(circle_diam/transform.localScale.x, circle_diam/transform.localScale.y);
        curr_circle.transform.parent = gameObject.transform;
        curr_circle.transform.localPosition = Vector2.right * circle_dist;
    }

    // Update is called once per frame
    private void FixedUpdate() {
        tomtext.behavior = state;
        if (transform.rotation.z >= .5 || transform.rotation.z <= -.5)
            gameObject.GetComponent<SpriteRenderer>().flipY = true;
        else
            gameObject.GetComponent<SpriteRenderer>().flipY = false;
        Vector3 vel = rb.velocity;
        if (transform.position.x > 35 || transform.position.x < -35)
        {
            transform.Rotate(0, 0, 180);
            //rb.velocity = new Vector3(-vel.x, vel.y, vel.z);
        }
        if (transform.position.y > 17 || transform.position.y < -17)
        {
            transform.Rotate(0, 0, 180);
            //rb.velocity = new Vector3(vel.x, -vel.y, vel.z);
        }
            
        //Dynamic Pursue
        if ((player.transform.position - transform.position).magnitude < pursue_rad)
        {
            
            curr_chase_target.SetActive(true);
            curr_circle.SetActive(false);
            curr_spot.SetActive(false);
            //using the distance calculate the direction we must go
            Vector2 player_guess = player.transform.position;
            float dist = (player.transform.position - transform.position).magnitude;
            player_guess.x += player.GetComponent<Rigidbody2D>().velocity.x * dist/speed;
            player_guess.y += player.GetComponent<Rigidbody2D>().velocity.y * dist/speed;

            curr_chase_target.transform.position = player_guess;

            //using the direction check the angle that we must turn
            Align(player.transform.rotation);
            //move towards the player
            Vector2 thrust;

            //Dynamic Arrival
            if (dist <= d_arrive_dist)
            {
                state = "Dynamic Arrival";
                float target_speed = speed * (dist / d_arrive_dist);
                Vector2 direction = (curr_chase_target.transform.position - transform.position).normalized;
                Vector2 target_velocity = target_speed * direction;
                thrust = target_velocity - (Vector2)gameObject.GetComponent<Rigidbody2D>().velocity;
                rb.AddForce(thrust/(dist/speed));
            }
            else
            {
                state = "Dynamic Pursue";
                Vector2 direction = (curr_chase_target.transform.position - transform.position).normalized;
                Vector2 target_velocity = speed * direction;
                rb.velocity = target_velocity;
            }
            
            update = 0;
        }
        else
        {
            state = "Dynamic Wander";
            curr_chase_target.SetActive(false);
            curr_circle.SetActive(true);
            curr_spot.SetActive(true);
            //Dynamic Wander
            update--;
            if (update <= 0)
            {
                float angle = rnd.Next(0, 360);
                angle *= Mathf.Deg2Rad;

                float x = circle_diam/2 * Mathf.Cos(angle);
                float y = circle_diam/2 * Mathf.Sin(angle);

                curr_spot.transform.localPosition = new Vector2(x / (curr_circle.transform.localScale.x*transform.localScale.x), y / (curr_circle.transform.localScale.y*transform.localScale.y));
                target = curr_spot.transform.position;

                //Rotate towards target
                /*Vector2 vectorToTarget = target - (Vector2)transform.position;
                float new_angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
                turn = Quaternion.AngleAxis(new_angle, Vector3.forward);*/
                update = update_dur;
            }
    
            //Move in current facing direction
            Seek(target);

            Vector2 direction = target - (Vector2)transform.position;
            float target_angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Align(Quaternion.AngleAxis(target_angle, Vector3.forward));
            //transform.rotation = Quaternion.Slerp(transform.rotation, turn, Time.deltaTime * rot_speed);
            //transform.Translate(Vector2.right * Time.deltaTime * speed);
        }
    }

    private void Align(Quaternion target_orientation)
    {
        float angle_dist = target_orientation.eulerAngles.z - transform.rotation.eulerAngles.z;
        if (angle_dist < 0)
            angle_dist *= -1;
        if (angle_dist < angle_slow)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, target_orientation, Time.deltaTime * rot_speed / (angle_dist / angle_slow));
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, target_orientation, Time.deltaTime * rot_speed);
        }
    }

    private void Seek(Vector2 target_pos)
    {
        Vector2 direction = (target_pos - (Vector2)transform.position).normalized;
        transform.GetComponent<Rigidbody2D>().velocity = direction * speed;
    }
    /*
     * 1. rotation	=	target.orientation	-character.orientation
    2. Map	rotation	to	the	(-π,	π)	interval	
    3. Similar	to	adjusting	linear	acceleration	for	arrival,		
    1. Initially	character	turn	at	maximum	angular	speed	
    2. Slow	down	a_er	reaching	the	slow	radius	(an	angle)	
    3. In	a	fixed	amount	of	time,	bring	character’s	rotation	
    speed	to	the	same	as	the	target’s	(0	if	the	target	is	not	
    rotating)
    */
}
