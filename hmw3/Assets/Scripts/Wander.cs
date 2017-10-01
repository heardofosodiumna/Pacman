using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : MonoBehaviour {

    public GameObject circle;
    public GameObject spot;
    public float speed;
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
    public GameObject[] flock;

    private void Start()
    {
        curr_spot = Instantiate(spot, new Vector2(0, 0), new Quaternion(0, 0, 0, 0));

        curr_circle = Instantiate(circle, new Vector2(0, 0), new Quaternion(0, 0, 0, 0));
        curr_spot.transform.parent = curr_circle.transform;
        curr_spot.transform.localScale = new Vector2(1 / transform.localScale.x, 1 / transform.localScale.y);
        curr_circle.transform.Rotate(new Vector2(0, 0));
        curr_circle.transform.localScale = new Vector2(circle_diam / transform.localScale.x, circle_diam / transform.localScale.y);
        curr_circle.transform.parent = gameObject.transform;
        curr_circle.transform.localPosition = Vector2.right * circle_dist;
    }

	private void FixedUpdate()
    {
        for(int i = 0; i < flock.Length; i++)
        {
            Debug.Log(flock[i].transform.position);454
        }
        /*Seek(target);

        Vector2 direction = target - (Vector2)transform.position;
        float target_angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Align(Quaternion.AngleAxis(target_angle, Vector3.forward));
        */
    }

    private void ConeCheck()
    {
        Debug.Log("Implement");
    }

    private void Seek(Vector2 target_pos)
    {
        Vector2 direction = (target_pos - (Vector2)transform.position).normalized;
        transform.GetComponent<Rigidbody2D>().velocity = direction * speed;
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
