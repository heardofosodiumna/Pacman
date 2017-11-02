using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class endBut : MonoBehaviour
{

    bool activateEnd;
    Text text;
    string og;
    Camera camera;
    public GameObject end;
    public GameObject open;
    Quaternion no_rotate;

    // Use this for initialization
    void Start()
    {
        activateEnd = false;
        text = GetComponentInChildren<Text>();
        og = text.text;
        camera = Camera.main;
        no_rotate = new Quaternion(0, 0, 0, 0);
    }
    void FixedUpdate()
    {
        if (activateEnd && Input.GetMouseButtonDown(0))
        {
            GameObject temp = Physics2D.Raycast(new Vector2(camera.ScreenToWorldPoint(Input.mousePosition).x, camera.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f).transform.gameObject;
            if (temp.tag == "openSpace")
            {
                GameObject oldEnd = GameObject.FindGameObjectWithTag("end");

                //first destroy and place a start buttons on the object we clicked
                Vector3 loc = temp.transform.position;
                Destroy(temp);
                Instantiate(end, loc, no_rotate);

                //then delete the previous start adn replace it with an open tile
                if (oldEnd)
                {
                    Vector3 oldloc = oldEnd.transform.position;
                    Destroy(oldEnd);
                    Instantiate(open, oldloc, no_rotate);
                }
                setEndFalse();

            }
        }

    }

    public void setEndFalse()
    {
        activateEnd = false;
        text.text = og;
    }
    public void setEndTrue()
    {

        activateEnd = true;
        text.text = "click to set location";

    }
    public bool GetEnd()
    {
        return activateEnd;
    }
}
