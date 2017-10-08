using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowBird : MonoBehaviour {
    int next;
    Vector3 nextPos;
	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {

    }

    public int getNext()
    {
        return next;
    }
    public void incNext()
    {
        next++;
    }
    public void setPos(Vector3 p)
    {
        nextPos = p;
    }
    public Vector3 getPos()
    {
        return nextPos;
    }
}
