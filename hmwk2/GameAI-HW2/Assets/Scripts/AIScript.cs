using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIScript : MonoBehaviour {

    //target we wish to follow
    public Transform player;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //using the distance calculate the direction we must go
        Vector3 dir = player.position - transform.position;
        Debug.Log("direction is " + dir);
        //using the direction check the angle that we must turn
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        Debug.Log("angle is " + angle);
        //rotate the character
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 90);
        //move towards the player
        transform.Translate(Vector3.forward * Time.deltaTime * 5);
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
