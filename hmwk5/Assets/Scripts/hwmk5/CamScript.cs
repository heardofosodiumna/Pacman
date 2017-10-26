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
        if (Input.GetAxis("Mouse ScrollWheel") > 0f|| Input.GetKeyDown("q")) // forward
        {
             Vector3 temp = mainCam.transform.position;
            if(temp.z<-20)
                temp.z+=2;
            mainCam.transform.position = temp;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f|| Input.GetKeyDown("e")) // backwards
        {
            Vector3 temp = mainCam.transform.position;
            if (temp.z > -184)
                temp.z-=2;
            mainCam.transform.position = temp;
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
            move = new Vector3(-.3f, 0, 0);
            if(mainCam.transform.position.x > 0)
                mainCam.transform.Translate(move, Space.World);
            move = Vector3.zero;
        }
        if (Input.GetKey("d"))
        {
            move = new Vector3(.3f, 0, 0);
            if (mainCam.transform.position.x < 250)
                mainCam.transform.Translate(move, Space.World);
            move = Vector3.zero;
        }
        if (Input.GetKey("s"))
        {
            move= new Vector3(0, -.3f, 0);
            if(mainCam.transform.position.y > -230)
                mainCam.transform.Translate(move, Space.World);
            move = Vector3.zero;
        }
        if (Input.GetKey("w"))
        {
            move = new Vector3(0, .3f, 0);
            if(mainCam.transform.position.y < 0)
                mainCam.transform.Translate(move, Space.World);
            move = Vector3.zero;
        }
        /*if (mainCam.transform.position.x > 0 && moveX.x < 0 || moveX.x > 0 && mainCam.transform.position.x < 250)
            if ((moveX.x > -1 && moveX.x < .2) || (moveX.x > .7 && moveX.x < 1.1))
            {
                mainCam.transform.Translate(moveX, Space.World);
                moveX = Vector3.zero;
            }*/
        //  if (mainCam.transform.position.y >-230&& moveY.y<0 || moveY.y>0 && mainCam.transform.position.y < 0)
        //   mainCam.transform.Translate(moveY, Space.World);
        
    }

   }
