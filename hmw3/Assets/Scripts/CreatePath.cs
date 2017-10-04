using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreatePath : MonoBehaviour {

    public GameObject red_point;
    public GameObject blue_point;
    public List<Vector2> red_path_points = new List<Vector2>();
    public List<Vector2> blue_path_points = new List<Vector2>();
    public float height;
    public float width;

	// Use this for initialization
	void Start () {

        if (SceneManager.GetActiveScene().name.Equals("Main"))
        {
            for (float i = 0; i <= 6 * Mathf.PI; i += Mathf.PI / 24)
            {
                red_path_points.Add(new Vector2(i * width - 10, height * Mathf.Sin(i + Mathf.PI / 2)));
                blue_path_points.Add(new Vector2(i * width - 10, height * Mathf.Sin(-i - Mathf.PI / 2)));
            }
        } 
        else if(SceneManager.GetActiveScene().name.Equals("Pattern 2"))
        {
            for (float i = 0; i <= 2 * Mathf.PI; i += Mathf.PI / 96)
            {
                red_path_points.Add(new Vector2(width * Mathf.Sin(2 * i), height * 2 * Mathf.Sin(3 * i)));
                blue_path_points.Add(new Vector2(width * 2 * Mathf.Sin(3 * i), height * Mathf.Sin(2 * i)));
            }
        }
        else if (SceneManager.GetActiveScene().name.Equals("Pattern 3"))
        {
            int a = 0;
            float b = .5f;
            for (float i = 6 * Mathf.PI; i > 0 ; i -= Mathf.PI / 96)
            {
                red_path_points.Add(new Vector2(b*i*Mathf.Cos(i+a), b * i * Mathf.Sin(i+a)));
                blue_path_points.Add(new Vector2(b * i * Mathf.Sin(i + a), b * i * Mathf.Cos(i + a)));
            }
        }

        foreach (Vector2 p in red_path_points)
        {
            Instantiate(red_point, p, new Quaternion(0, 0, 0, 0));
        }
        foreach(Vector2 p in blue_path_points)
        {
            Instantiate(blue_point, p, new Quaternion(0, 0, 0, 0));
        }
    }
}
