using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamScript : MonoBehaviour {
    
    Camera mainCam;
    Vector3 dragOrigin;
    //z range from -20 to -184
    //x from 0 to 250
    //y from 0 to -205
	// Use this for initialization
	void Start () {
        mainCam = Camera.main;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetKeyDown("q")) // forward
        {
            float temp = mainCam.orthographicSize;
            if(temp > 3)
                temp -= 3;
            mainCam.orthographicSize = temp;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f || Input.GetKeyDown("e")) // backwards
        {
            float temp = mainCam.orthographicSize;
            if (temp < 160)
                temp += 3;
            mainCam.orthographicSize = temp;
        }

        /*
            *--------------------------- 
            * |(0,1)
            * |
            * |
            * |
            * |
            * |(0,0)               (1,0)
            * --------------------------
            * 
            */
        Vector3 move;
            
        if (Input.GetKey("a"))
        {
            move = new Vector3(-1f, 0, 0);
            mainCam.transform.Translate(move, Space.World);
            move = Vector3.zero;
        }
        if (Input.GetKey("d"))
        {
            move = new Vector3(1f, 0, 0);
            mainCam.transform.Translate(move, Space.World);
            move = Vector3.zero;
        }
        if (Input.GetKey("s"))
        {
            move= new Vector3(0, -1f, 0);
            mainCam.transform.Translate(move, Space.World);
            move = Vector3.zero;
        }
        if (Input.GetKey("w"))
        {
            move = new Vector3(0, 1f, 0);
            mainCam.transform.Translate(move, Space.World);
            move = Vector3.zero;
        }
        
    }

}
