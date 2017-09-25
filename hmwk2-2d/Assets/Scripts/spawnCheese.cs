using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnCheese : MonoBehaviour {

    
    public GameObject cheese;
    public List<Vector2> points = new List<Vector2>();
    Vector2 point = new Vector2(26f,-8f);
    int howFar;
    public int div;
    public int count;

    void Start()
    { 
        Vector2 pos2 = new Vector2(Random.Range(-32f, -28f), Random.Range(-9f, -7f));

      //  Debug.Log("point2: " + pos2);
        Instantiate(cheese, point, Quaternion.identity);
        points.Add(point);
        howFar = (int)Mathf.Sqrt(((pos2.x - point.x) * (pos2.x - point.x)) + ((pos2.y - point.y) * (pos2.x - point.x)));
      //  Debug.Log(howFar);
        int howMany = howFar / div;
        for(int i=0; i < count; ++i)
        {
            point = Vector2.MoveTowards(point, pos2, howMany);
            point += new Vector2(0,Random.Range(0, -1f));
            points.Add(point);
            Instantiate(cheese, point, Quaternion.identity);
        }

        Vector2 pos3 = new Vector2(Random.Range(24f, 28f), Random.Range(7f, 9f));
        howFar = (int)Mathf.Sqrt(((pos3.x - pos2.x) * (pos3.x - pos2.x)) + ((pos3.y - pos2.y) * (pos3.x - pos2.x)));
    //    Debug.Log(howFar);
        howMany = howFar / div;
        for (int i = 0; i < count/2; ++i)
        {
             point = Vector2.MoveTowards(point, pos3, howMany);
             point += new Vector2(0, Random.Range(0f, 2f));
             points.Add(point);
             Instantiate(cheese, point, Quaternion.identity);
        }
        for (int i = 0; i < count / 2 ; ++i)
        {
            point = Vector2.MoveTowards(point, pos3, howMany);
            point += new Vector2(0, Random.Range(-3f, 0f));
            points.Add(point);
            Instantiate(cheese, point, Quaternion.identity);
        }
        Vector2 pos4 = new Vector2(Random.Range(-27f, -18f), Random.Range(6f, 9f));
        howFar = (int)Mathf.Sqrt(((pos4.x - pos3.x) * (pos4.x - pos3.x)) + ((pos4.y - pos3.y) * (pos4.x - pos3.x)));
    //    Debug.Log(howFar);
        howMany = howFar / div;
        for (int i = 0; i < count; ++i)
        {
            point = Vector2.MoveTowards(point, pos4, howMany);
            point += new Vector2(0, Random.Range(0f, 1f));
            points.Add(point);
            Instantiate(cheese, point, Quaternion.identity);
        }
       

    }

    public List<Vector2> getPath()
    {
        return points;
    }

    private void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            Application.LoadLevel(0);
        }
    }
}
