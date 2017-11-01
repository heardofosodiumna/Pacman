using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgorithmScript : MonoBehaviour {

    GameObject start;
    GameObject end;
    Vector3 startPos;
    Vector3 endPos;

    // Use this for initialization
    void Start() {
        findAndGetStart();
        findAndGetEnd();

    }
	
	// Update is called once per frame
	void Update () {
        //need to contantly get this because the user can relocate start and end
        //if either is relocated the reference of the original is deleted and this the reference is null
        //thus if it is null we relocate it agains
        if (!start)
            findAndGetStart();
        if (!end)
            findAndGetEnd();

        //for demonstrations purposes
        //destroys objects 2 units left,right,up,down from start
        //this will give an error
        //Destroy(getObjectDown(getObjectDown(start)));
        //Destroy(getObjectUp(getObjectUp(start)));
        //Destroy(getObjectLeft(getObjectLeft(start)));
        //Destroy(getObjectRight(getObjectRight(start)));

        if(start && end)
        {
            destroyPathToEnd();
        }
    }
    void destroyPathToEnd()
    {
        Vector3 temp = startPos;
        Vector3 dir = endPos - startPos;
        if (dir.x != 0)
            dir.x = dir.x / Mathf.Abs(dir.x);

        if (dir.y != 0)
            dir.y = dir.y / Mathf.Abs(dir.y);
        temp = temp + dir;
        while (temp!= endPos)
        {
            dir = endPos - temp;
            if (dir.x != 0)
                dir.x = dir.x / Mathf.Abs(dir.x);

            if (dir.y != 0)
                dir.y = dir.y / Mathf.Abs(dir.y);
            Destroy(FindAt(temp));
            temp = temp+dir;
        }

    }
    GameObject getObjectDown(GameObject aGameObject)
    {
        if (aGameObject == null)
        {
            return null;
        }
        Vector3 loc = aGameObject.transform.position + new Vector3(0, -1, 0);
        return FindAt(loc);
    }
    GameObject getObjectUp(GameObject aGameObject)
    {
        if (aGameObject == null)
        {
            return null;
        }
        Vector3 loc = aGameObject.transform.position + new Vector3(0, 1, 0);
        return FindAt(loc);
    }
    GameObject getObjectRight(GameObject aGameObject)
    {
        if (aGameObject == null)
        {
            return null;
        }
        Vector3 loc = aGameObject.transform.position + new Vector3(1, 0, 0);
        return FindAt(loc);
    }
    GameObject getObjectLeft(GameObject aGameObject)
    {
        if (aGameObject == null)
        {
            return null;
        }
        Vector3 loc = aGameObject.transform.position + new Vector3(-1, 0, 0);
        return FindAt(loc);
    }
    GameObject FindAt(Vector3 pos) {
        // get all colliders that intersect pos:
        Collider2D col = Physics2D.OverlapBox(pos, new Vector2(.5f, .5f), 0);
        return col.gameObject;
     }
    void findAndGetStart()
    {
        start = GameObject.FindGameObjectWithTag("start");
        if (start != null)
        {
            startPos = start.transform.position;
        }
    }
    void findAndGetEnd()
    {
        end = GameObject.FindGameObjectWithTag("end");
        if (end != null)
        {
            endPos = end.transform.position;
        }
    }

}
