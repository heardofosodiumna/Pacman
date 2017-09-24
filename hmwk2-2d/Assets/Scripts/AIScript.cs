using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIScript : MonoBehaviour {

    //target we wish to follow
    public Transform player;
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
    System.Random rnd = new System.Random();

    private void Start()
    {
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
        if ((player.position - transform.position).magnitude < catch_radius)
        {
            //using the distance calculate the direction we must go
            Vector2 dir = player.position - transform.position;
            //using the direction check the angle that we must turn
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            //rotate the character
            Debug.Log(angle);
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 90);
            //move towards the player 
            transform.Translate(Vector2.right * Time.deltaTime * speed);
            update = 0;
        }
        else
        {
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
