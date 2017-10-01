using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : MonoBehaviour {

    public GameObject circle;
    public GameObject spot;
    public float max_acceleration;
    GameObject curr_circle;
    GameObject curr_spot;
    GameObject curr_chase_target;
    public float circle_diam;
    int update = 0;
    public int update_rate;
    System.Random rnd = new System.Random();
    public int circle_dist;
    Vector2 target;
    public int angle_slow;
    public int rot_speed;
    public GameObject yellow_birds;
    Transform[] flock;
    public int k;

    private void Start()
    {
        flock = yellow_birds.GetComponentsInChildren<Transform>();
    }

	private void FixedUpdate()
    {
        Debug.Log(flock);
        for(int i = 0; i < flock.Length; i++)
        {
            Debug.Log(flock[i].transform.position);
            //Separate
            foreach(Transform bird in flock)
            {
                Vector2 direction = transform.position - bird.position;
                float distance = direction.magnitude;
                float strength = Mathf.Min(k / (distance * distance), max_acceleration);

                GetComponent<Rigidbody2D>().AddForce(direction.normalized * strength);
            }
        }
    }

    private void ConeCheck()
    {
        Debug.Log("Implement");
    }

    private void Seek(Vector2 target_pos)
    {
        Debug.Log("Redo for dynamic");
    }

    private void Align(Quaternion target_orientation)
    {
        float angle_dist = target_orientation.eulerAngles.z - transform.rotation.eulerAngles.z;
        Debug.Log(angle_dist);
        if (angle_dist < 0)
            angle_dist *= -1;
        if (angle_dist < angle_slow)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, target_orientation, Time.deltaTime * rot_speed / (angle_dist / angle_slow));
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, target_orientation, Time.deltaTime * rot_speed);
        }
    }
}
