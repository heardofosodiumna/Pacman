using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnCheese : MonoBehaviour {

    
    public GameObject cheese;
    List<Vector2> points = new List<Vector2>();
    Vector2 point = new Vector2(9f,-3f);
    int howFar;

    void Start()
    { 
        Vector2 pos2 = new Vector2(Random.Range(-9.5f, -7.5f), Random.Range(-3f, -2f));
        Vector2 pos3 = new Vector2(Random.Range(7f, 8f), Random.Range(2f,3f));
        Vector2 pos4 = new Vector2(Random.Range(-10f, 9f), Random.Range(3f, 4f));

        Debug.Log("point2: " + pos2);
        Instantiate(cheese, point, Quaternion.identity);
        howFar = (int)Mathf.Sqrt(((pos2.x - point.x) * (pos2.x - point.x)) + ((pos2.y - point.y) * (pos2.x - point.x)));
        Debug.Log(howFar);
        for(int i=0; i < howFar; ++i)
        {
            point = Vector2.MoveTowards(point, pos2, 1);
            point += new Vector2(0,Random.Range(0, -.4f));
            points.Add(point);
            Instantiate(cheese, point, Quaternion.identity);
        }
        howFar = (int)Mathf.Sqrt(((pos3.x - pos2.x) * (pos3.x - pos2.x)) + ((pos3.y - pos2.y) * (pos3.x - pos2.x)));
        Debug.Log(howFar);
        for (int i = 0; i < howFar; ++i)
        {
            point = Vector2.MoveTowards(point, pos3, 1);
            point += new Vector2(0, Random.Range(-.4f, .4f));
            points.Add(point);
            Instantiate(cheese, point, Quaternion.identity);
        }
        howFar = (int)Mathf.Sqrt(((pos4.x - pos3.x) * (pos4.x - pos3.x)) + ((pos4.y - pos3.y) * (pos4.x - pos3.x)));
        Debug.Log(howFar);
        for (int i = 0; i < howFar; ++i)
        {
            point = Vector2.MoveTowards(point, pos4, 1);
            point += new Vector2(0, Random.Range(0f, .4f));
            points.Add(point);
            Instantiate(cheese, point, Quaternion.identity);
        }


    }

    // Update is called once per frame
    void Update () {
		
	}
}
