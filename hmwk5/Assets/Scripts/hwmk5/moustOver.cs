using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moustOver : MonoBehaviour {

    public GameObject start;
    public GameObject End;
    public GameObject Wall;
    public GameObject Tree;
    public GameObject Tile;
    GameObject startButton;
    GameObject endButton;
    GameObject treeButton;
    bool hover;

    Quaternion no_rotate;


    void Start()
    {
        hover= false;
        no_rotate = new Quaternion(0, 0, 0, 0);
        startButton = GameObject.Find("Set Start");
        endButton = GameObject.Find("Set End");
        treeButton = GameObject.Find("Set Tree");
    }
    void FixedUpdate()
    {
        if (hover)
        {
            if (Input.GetMouseButtonDown(0) && this.tag == "openSpace" && startButton.GetComponent<startBut>().GetStart())
            {
                GameObject old = GameObject.FindGameObjectWithTag("start");
                //first destroy the old start tile
                if(old != null)
                {
                    Vector3 temp = old.transform.position;
                    Destroy(old.gameObject);
                    Instantiate(Tile, temp, no_rotate);
                }
                //destroy the current tile
                Vector3 pos = this.transform.position;
                Destroy(this.gameObject);
                Instantiate(start, pos, no_rotate);

                startButton.GetComponent<startBut>().setStartFalse();
            }
            if (Input.GetMouseButtonDown(0) && this.tag == "openSpace" && endButton.GetComponent<endBut>().GetEnd())
            {
                GameObject old = GameObject.FindGameObjectWithTag("end");
                //first destroy the old start tile
                if (old != null)
                {
                    Vector3 temp = old.transform.position;
                    Destroy(old.gameObject);
                    Instantiate(Tile, temp, no_rotate);
                }
                //destroy the current tile
                Vector3 pos = this.transform.position;
                Destroy(this.gameObject);
                Instantiate(End, pos, no_rotate);

                endButton.GetComponent<endBut>().setEndFalse();
            }

            if (Input.GetMouseButtonDown(0) && this.tag == "openSpace" && treeButton.GetComponent<treeBut>().GetTree())
            {
                
                //destroy the current tile
                Vector3 pos = this.transform.position;
                Destroy(this.gameObject);
                Instantiate(Tree, pos, no_rotate);
            }
        }
        
    }
    void OnMouseEnter()
    {
        hover = true;
    }
    void OnMouseExit()
    {
        hover = false;
    }
}
