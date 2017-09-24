using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIScript : MonoBehaviour {

    //target we wish to follow
    public GameObject player;
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
    public int catch_radius;
    GameObject curr_chase_target;
    public int d_arrive_dist;
    System.Random rnd = new System.Random();

    private void Start()
    {
        GameObject d_arrive_circ = Instantiate(circle, player.transform.position, new Quaternion(0, 0, 0, 0));
        d_arrive_circ.transform.parent = player.transform;
        d_arrive_circ.transform.localScale = new Vector2(d_arrive_dist / player.transform.localScale.x, d_arrive_dist / player.transform.localScale.y);
        curr_chase_target = Instantiate(spot, new Vector2(0, 0), new Quaternion(0, 0, 0, 0));
        curr_chase_target.SetActive(false);
        curr_spot = Instantiate(spot, new Vector2(0, 0), new Quaternion(0, 0, 0, 0));
        GameObject track_circ = Instantiate(circle, new Vector2(0, 0), new Quaternion(0, 0, 0, 0));
        track_circ.transform.parent = gameObject.transform;
        track_circ.transform.localScale = 2 * catch_radius * new Vector2(1 / transform.localScale.x, 1 / transform.localScale.y);

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
        if (transform.rotation.z >= .5 || transform.rotation.z <= -.5)
            gameObject.GetComponent<SpriteRenderer>().flipY = true;
        else
            gameObject.GetComponent<SpriteRenderer>().flipY = false;
        //Dynamic Pursue
        if ((player.transform.position - transform.position).magnitude < catch_radius)
        {
            curr_chase_target.SetActive(true);
            curr_circle.SetActive(false);
            curr_spot.SetActive(false);
            //using the distance calculate the direction we must go
            Vector2 player_guess = player.transform.position;
            float dist = (player.transform.position - transform.position).magnitude;
            player_guess.x += player.GetComponent<CharacterController>().velocity.x * Time.deltaTime * 2 * dist;
            player_guess.y += player.GetComponent<CharacterController>().velocity.y * Time.deltaTime * 2 * dist;
            Debug.Log(player.GetComponent<CharacterController>().velocity);

            curr_chase_target.transform.position = player_guess;


            Vector2 dir = player_guess - (Vector2)transform.position;
            //using the direction check the angle that we must turn
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            //rotate the character
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 90);
            //move towards the player
            if (dist < d_arrive_dist)
                transform.Translate(Vector2.right * Time.deltaTime * speed * (dist/d_arrive_dist));
            else
                transform.Translate(Vector2.right * Time.deltaTime * speed);
            update = 0;
        }
        else
        {
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
                Vector2 vectorToTarget = target - (Vector2)transform.position;
                float new_angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
                turn = Quaternion.AngleAxis(new_angle, Vector3.forward);
                update = update_dur;
            }

            //Move in current facing direction
            transform.rotation = Quaternion.Slerp(transform.rotation, turn, Time.deltaTime * rot_speed);
            transform.Translate(Vector2.right * Time.deltaTime * speed);
        }
        
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
