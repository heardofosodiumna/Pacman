using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moustOver : MonoBehaviour {

    public GameObject start;
    public GameObject End;
    public GameObject Wall;
    public GameObject Tree;
    public GameObject Tile;
    bool hover;

    Quaternion no_rotate;

    void Start()
    {
        hover= false;
        no_rotate = new Quaternion(0, 0, 0, 0);
    }
    void FixedUpdate()
    {
        if (hover)
        {
            print(this.gameObject);

            if (Input.GetMouseButtonDown(0))
            {
                if (this.tag == "openSpace")
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
                    print(pos);
                    Destroy(this.gameObject);
                    Instantiate(start, pos, no_rotate);
                }
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
