using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class startBut : MonoBehaviour {

    bool activateStart;
    Text text;
    string og;
    Camera camera;
    public GameObject start;
    public GameObject open;
    Quaternion no_rotate;

    // Use this for initialization
    void Start () {
        activateStart =false;
        text = GetComponentInChildren<Text>();
        og = text.text;
        camera = Camera.main;
        no_rotate = new Quaternion(0, 0, 0, 0);
    }
    void FixedUpdate()
    {
        if (activateStart && Input.GetMouseButtonDown(0))
        {
            GameObject temp = Physics2D.Raycast(new Vector2(camera.ScreenToWorldPoint(Input.mousePosition).x, camera.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f).transform.gameObject;
            if (temp.tag == "openSpace")
            {
                GameObject oldStart = GameObject.FindGameObjectWithTag("start");

                //first destroy and place a start buttons on the object we clicked
                Vector3 loc = temp.transform.position;
                Destroy(temp);
                Instantiate(start, loc, no_rotate);

                //then delete the previous start adn replace it with an open tile
                if (oldStart)
                {
                    Vector3 oldloc = oldStart.transform.position;
                    Destroy(oldStart);
                    Instantiate(open, oldloc, no_rotate);
                }
                setStartFalse();

            }
        }
        
    }

    public void setStartFalse()
    {
        activateStart = false;
        text.text = og;
    }
    public void setStartTrue()
    {

        activateStart = true;
        text.text = "click to set location";
        
    }
    public bool GetStart()
    {
        return activateStart;
    }
}
