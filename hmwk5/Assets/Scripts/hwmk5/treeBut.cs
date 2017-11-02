using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class treeBut : MonoBehaviour
{

    bool activateTree;
    Text text;
    string og;
    Camera camera;
    public GameObject tree;
    public GameObject open;
    Quaternion no_rotate;

    // Use this for initialization
    void Start()
    {
        activateTree = false;
        text = GetComponentInChildren<Text>();
        og = text.text;
        camera = Camera.main;
        no_rotate = new Quaternion(0, 0, 0, 0);
    }
    void FixedUpdate()
    {
        if (activateTree && Input.GetMouseButtonDown(0))
        {
            GameObject temp = Physics2D.Raycast(new Vector2(camera.ScreenToWorldPoint(Input.mousePosition).x, camera.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f).transform.gameObject;
            if (temp.tag == "openSpace")
            {

                //first destroy and place a start buttons on the object we clicked
                Vector3 loc = temp.transform.position;
                Destroy(temp);
                Instantiate(tree, loc, no_rotate);
                

            }
        }

    }

    public void setTreeFalse()
    {
        activateTree = false;
        text.text = og;
    }
    public void setTreeTrue()
    {
        if (!activateTree)
        {
            activateTree = true;
            text.text = "click to set location";
        }
        else
        {
            activateTree = false;
            text.text = og;
        }
    }
    public bool GetTree()
    {
        return activateTree;
    }
}
