using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBird : MonoBehaviour
{
    int next;
    Vector3 nextPos;
    public float rot_speed;
    public float angle_slow;
    public Vector3 test;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.rotation.z >= .5 || transform.rotation.z <= -.5)
            gameObject.GetComponent<SpriteRenderer>().flipY = true;
        else
            gameObject.GetComponent<SpriteRenderer>().flipY = false;
        Vector2 direction = (Vector2)nextPos - (Vector2)transform.position;
        float target_angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Align(Quaternion.AngleAxis(target_angle, Vector3.forward));

        checkCollision();
    }

    public void checkCollision()
    {
        float angle = 30;
        angle *= Mathf.Deg2Rad;
        Vector3 red = (transform.position + transform.right);
        //Debug.Log(transform.right);
        Debug.DrawLine(transform.position, red, Color.red);

        float curent_angle = Mathf.Atan2(transform.right.y, transform.right.x);
        float greenx = (transform.position.x + Mathf.Cos(curent_angle+angle));
        float greeny = (transform.position.y + Mathf.Sin(curent_angle+angle));
        Vector3 green = new Vector3(greenx, greeny, 0);
        Debug.DrawLine(transform.position, green, Color.green);

        float bluex = (transform.position.x + Mathf.Cos(curent_angle - angle));
        float bluey = (transform.position.y + Mathf.Sin(curent_angle - angle));
        Vector3 blue = new Vector3(bluex, bluey, 0);
        Debug.DrawLine(transform.position, blue, Color.blue);

        //green x will be origin + hypothenus*sin(30)
        //green y will be origin + hypothenus*cos(30)
        // Debug.Log(green);
        //        Debug.DrawLine(transform.position, (transform.position + transform.right), Color.blue);
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
    private void Align(Quaternion target_orientation)
    {
        float angle_dist = target_orientation.eulerAngles.z - transform.rotation.eulerAngles.z;
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
