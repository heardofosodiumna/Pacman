using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamScript : MonoBehaviour {
    
    Camera mainCam;
    float dragSpeed = 3;
    Vector3 dragOrigin;
    bool active = false;
    //z range from -20 to -184
    //x from 0 to 250
    //y from 0 to -205
	// Use this for initialization
	void Start () {
        mainCam = Camera.main;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
             Vector3 temp = mainCam.transform.position;
            if(temp.z<-20)
                temp.z+=2;
            mainCam.transform.position = temp;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            Vector3 temp = mainCam.transform.position;
            if (temp.z > -184)
                temp.z-=2;
            mainCam.transform.position = temp;
        }
        if (Input.GetMouseButtonDown(0) && !active)
            active = true;
        if (active)
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

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
            if ((pos.x > -1 && pos.x < .2))
            {
                print("left");
                move = new Vector3(-.3f, 0, 0);
                mainCam.transform.Translate(move, Space.World);
                move = Vector3.zero;
            }
            if ((pos.x > .8 && pos.x < 1.1))
            {
                print("right");
                move = new Vector3(.3f, 0, 0);
                mainCam.transform.Translate(move, Space.World);
                move = Vector3.zero;
            }
            if ((pos.y > -1 && pos.y < .2))
            {
                print("down");
                move= new Vector3(0, -.3f, 0);
                mainCam.transform.Translate(move, Space.World);
                move = Vector3.zero;
            }
            if ((pos.y > .8 && pos.y < 1.1))
            {
                print("up");
                move = new Vector3(0, .3f, 0);
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

   }
