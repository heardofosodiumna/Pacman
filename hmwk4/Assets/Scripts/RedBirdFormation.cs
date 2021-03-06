﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBirdFormation : MonoBehaviour {

    GameObject[] flock;
    Vector2[] locs;

    float char_rad;
    public GameObject leader;
    public float spread;
    public float max_accel;
    public float sight_distance;
    public float min_coll_dist;
    public float d_arrive_dist = 1f;
    public float max_speed;
    public float slow_radius = Mathf.PI/24;
    public float max_rot_speed = Mathf.PI/2;
    public List<GameObject> path_points;
    public int path_index;
    public float leader_speed = .01f;
    public float leader_accel;
    public int future_index = 10;
    public float sight_line;
    public LayerMask layerMask;
    public float ray_angle;
    int dont_start;
    bool stopped;
    bool line_time;
    bool time3;
    float true_spread;

    // Use this for initialization
    void Start () {
        line_time = false;
        dont_start = 0;
        stopped = false;
        time3 = false;
        true_spread = spread;
        flock = GameObject.FindGameObjectsWithTag("Bird");
        char_rad = flock[0].GetComponent<CircleCollider2D>().radius;
        locs = new Vector2[flock.Length];
        spread = leader.GetComponent<LeaderSpread>().spread;
        leader.GetComponent<CircleCollider2D>().radius = spread * char_rad / Mathf.Sin(Mathf.PI / flock.Length) + .25f;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        flock = GameObject.FindGameObjectsWithTag("Bird");
        spread = leader.GetComponent<LeaderSpread>().spread;
        leader.GetComponent<CircleCollider2D>().radius = spread * char_rad / Mathf.Sin(Mathf.PI / flock.Length) + .25f;

        //find slot loc
        for (int i = 0; i < flock.Length; i++)
        {
            Vector2 target_loc = GetSlotLocation(i, flock.Length);

            target_loc += (Vector2)leader.transform.position;

            Debug.DrawLine(flock[i].transform.position, target_loc, Color.blue);

            if (line_time)
                GetInLine(flock[i], i);
            else if (time3 && !RayCastAvoidWalls(flock[i]))
                GetIn3(flock[i], i);
            else
            {
                //CollisionPredict(flock[i]);
                //if (!RayCastAvoidWalls(flock[i]))
                Seek(flock[i], target_loc);
            }
            

            Align(flock[i], flock[i].GetComponent<Rigidbody2D>().velocity.normalized);

            //clip to max_speed
            if (flock[i].GetComponent<Rigidbody2D>().velocity.magnitude > max_speed)
                flock[i].GetComponent<Rigidbody2D>().velocity = flock[i].GetComponent<Rigidbody2D>().velocity.normalized * max_speed;

            if(leader.GetComponent<Rigidbody2D>().velocity.magnitude > leader_speed)
                leader.GetComponent<Rigidbody2D>().velocity = leader.GetComponent<Rigidbody2D>().velocity.normalized * leader_speed;

            //fix orientation
            if (flock[i].transform.rotation.z >= .5 || flock[i].transform.rotation.z <= -.5)
                flock[i].GetComponent<SpriteRenderer>().flipY = true;
            else
                flock[i].GetComponent<SpriteRenderer>().flipY = false;
        }

        if(leader.GetComponent<Rigidbody2D>().velocity.magnitude > .05f)
            Align(leader, leader.GetComponent<Rigidbody2D>().velocity.normalized);

        if (!RayCastAvoidWalls(leader))
        {
            if (dont_start > 200)
                PathFollow();
            else
                dont_start++;
        }
    }

    void GetInLine(GameObject bird, int index)
    {
        if (index == 0)
            Seek(bird, leader.transform.position);
        else
            Seek(bird, (Vector2)flock[index - 1].transform.position - flock[index - 1].GetComponent<Rigidbody2D>().velocity.normalized * spread / 20);
    }

    void GetIn3(GameObject bird, int index)
    {
        if (index == 0)
        {
            locs[index] = leader.transform.position;
            Seek(bird, leader.transform.position);
        }
        else if(index == 1)
        {
            Vector2 temp = flock[index - 1].GetComponent<Rigidbody2D>().velocity.normalized;
            temp.x = flock[index - 1].GetComponent<Rigidbody2D>().velocity.normalized.y;
            temp.y = -flock[index - 1].GetComponent<Rigidbody2D>().velocity.normalized.x;
            Vector2 aPos = (Vector2)leader.transform.position - temp * spread / 4;

            locs[index] = aPos;
            Seek(bird, aPos);
        }
        else if (index == 2)
        {
            Vector2 temp = flock[index - 2].GetComponent<Rigidbody2D>().velocity.normalized;
            temp.x = -flock[index - 2].GetComponent<Rigidbody2D>().velocity.normalized.y;
            temp.y = flock[index - 2].GetComponent<Rigidbody2D>().velocity.normalized.x;
            Vector2 aPos = (Vector2)leader.transform.position - temp * spread / 4;
            
            locs[index] = aPos;
            Seek(bird, aPos);
        }
        else 
        {
            locs[index] = (Vector2)flock[index - 3].transform.position - flock[index - 3].GetComponent<Rigidbody2D>().velocity.normalized * spread / 10;
            Seek(bird, (Vector2)flock[index - 3].transform.position - flock[index - 3].GetComponent<Rigidbody2D>().velocity.normalized * spread / 10);
        }

        Debug.DrawLine(leader.transform.position, locs[index]);
    }

    void PathFollow()
    { 
        float min_dist = Mathf.Infinity;
        int min_point_index = path_index;
        for (int i = 0; i < path_points.Count; i++)
        {
            if (i < 0 || i > path_points.Count)
                continue;

            float distance = (leader.transform.position - path_points[i].transform.position).magnitude;
            if (distance < min_dist)
            {
                min_point_index = i;
                min_dist = distance;
            }
        }

        path_index = min_point_index;
        /*if (path_index == 43 && !stopped)
        {
            line_time = true;
            stopped = true;
            dont_start = 0;
            max_accel *= 2;
            max_speed *= 2;
            //leader_speed /= 2;
            return;
        }

        if (path_index == 74 && stopped)
        {
            line_time = false;
            stopped = false;
            dont_start = -200;
            max_accel /= 2;
            max_speed /= 2;
            //leader_speed *= 2;
            return;
        }
        if (path_index == 105 && !stopped)
        {
            time3 = true;
            stopped = true;
            dont_start = -100;
            //leader_speed /= 2;
            return;
        }*/

        if (path_index + future_index >= path_points.Count)
        {
            path_index = path_points.Count - future_index - 1;
        }
        Vector2 target = path_points[path_index + future_index].transform.position;

        Vector2 accel = target - (Vector2)leader.transform.position;
        float power = accel.magnitude;
        accel = (accel - leader.GetComponent<Rigidbody2D>().velocity).normalized * power;
        float strength = Mathf.Min(accel.magnitude, leader_accel);
        Vector2 thrust = accel.normalized * strength;

        float dist = accel.magnitude;

        leader.GetComponent<Rigidbody2D>().AddForce(thrust);
    }

    bool RayCastAvoidWalls(GameObject bird)
    {
        bool collided = false;
        Vector2 left_ray = (Vector2.right * Mathf.Cos(ray_angle / 2 * Mathf.PI / 180) + Vector2.up * Mathf.Sin(ray_angle / 2 * Mathf.PI / 180)).normalized;
        Vector2 center_ray = Vector2.right;
        Vector2 right_ray = (Vector2.right * Mathf.Cos(ray_angle / 2 * Mathf.PI / 180) - Vector2.up * Mathf.Sin(ray_angle / 2 * Mathf.PI / 180)).normalized;
        RaycastHit2D left_hit = Physics2D.Raycast(bird.transform.position, bird.transform.TransformDirection(left_ray), sight_line, layerMask);
        RaycastHit2D center_hit = Physics2D.Raycast(bird.transform.position, bird.transform.TransformDirection(center_ray), sight_line, layerMask);
        RaycastHit2D right_hit = Physics2D.Raycast(bird.transform.position, bird.transform.TransformDirection(right_ray), sight_line, layerMask);
        Vector2 target = new Vector2(0, 0);

        if (center_hit.collider != null)
        {
            Debug.DrawLine(leader.transform.position, center_hit.point);
            target = (Vector2)bird.transform.position - center_hit.point;
            target = (Vector2)bird.transform.position + target.normalized * 3;
            collided = true;
        }
        else if (left_hit.collider != null && right_hit.collider != null)
        {
            Debug.DrawLine(leader.transform.position, left_hit.point, Color.red);
            Debug.DrawLine(leader.transform.position, right_hit.point, Color.red);
            collided = false;
        }
        else if (left_hit.collider != null)
        {
            Debug.DrawLine(leader.transform.position, left_hit.point, Color.red);
            target = left_hit.point + left_hit.normal * 5;
            collided = true;
        }
        else if (right_hit.collider != null)
        {
            Debug.DrawLine(leader.transform.position, right_hit.point, Color.red);
            target = right_hit.point + right_hit.normal * 5;
            collided = true;
        }
        else
            collided = false;

        if(collided)
        {
            Vector2 accel = target - (Vector2)bird.transform.position;
            float power = accel.magnitude;
            accel = (accel - bird.GetComponent<Rigidbody2D>().velocity).normalized * power;
            float strength = Mathf.Min(accel.magnitude, max_accel);
            Vector2 thrust = accel.normalized * strength;

            float dist = accel.magnitude;

            bird.GetComponent<Rigidbody2D>().AddForce(thrust);
        }

        return collided;
        
    }

    /*public float isInsideDegree(RedBird bird, GameObject target, float n)
    {
        Vector2 dir = target.transform.position - bird.transform.position;
        float angle = Vector2.Angle(dir, bird.transform.right);

        if (angle < n)
            return angle;

        return -1f;
    }

    public void ConeCheck(RedBird bird)
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjects)
        {
            if (go.tag == "YellowBird")
            {
                float checkDegree = isInsideDegree(bird, go, cone_angle);
                if (checkDegree != -1)
                {
                    Vector2 goPos = go.transform.position;
                    float dist = Vector2.Distance(goPos, bird.transform.position);
                    if (dist < dodge_dist)
                    {
                        Flee(bird, go.transform.position);
                    }
                }
            }
        }
    }*/

    //Defensive Circle Formation
    Vector2 GetSlotLocation(int slot_num, int total_birds) {

        float angle = (float)slot_num / (float)total_birds * Mathf.PI * 2;
        float rad = spread * char_rad / Mathf.Sin(Mathf.PI / total_birds);

        Vector2 slot_loc = new Vector2(rad * Mathf.Cos(angle), rad * Mathf.Sin(angle));
        return slot_loc;
    }

    Vector2 GetSlotOrientation(Vector2 target_location) {
        Vector2 direction = target_location - (Vector2)leader.transform.position;
        return direction.normalized;
    }

    private void Seek(GameObject bird, Vector2 target)
    {
        Vector2 accel = target - (Vector2)bird.transform.position;
        float power = accel.magnitude;
        accel = (accel - bird.GetComponent<Rigidbody2D>().velocity).normalized*power;   
        float strength = Mathf.Min(accel.magnitude, max_accel);
        Vector2 thrust = accel.normalized * strength;

        float dist = accel.magnitude;

        if (dist <= d_arrive_dist && dist > 0)
        {
            float target_speed = max_speed * (dist / d_arrive_dist);
                 
            Vector2 direction = (target - (Vector2)bird.transform.position).normalized;
            Vector2 target_velocity = target_speed * direction;
            thrust = target_velocity - (Vector2)bird.GetComponent<Rigidbody2D>().velocity;

            if (bird.GetComponent<Rigidbody2D>().velocity.magnitude != 0)
                thrust = thrust / (dist / bird.GetComponent<Rigidbody2D>().velocity.magnitude);
        }

        bird.GetComponent<Rigidbody2D>().AddForce(thrust);
    }

    private void Align(GameObject bird, Vector2 orientation)
    {
        orientation = orientation.normalized;
        float angle = Mathf.Atan2(orientation.y, orientation.x) * Mathf.Rad2Deg;
        float rot_speed;
        if (Mathf.Abs(angle) > slow_radius)
            rot_speed = max_rot_speed;
        else
            rot_speed = angle / slow_radius;

        bird.transform.localRotation = Quaternion.Slerp(bird.transform.localRotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rot_speed);
    }

    public void CollisionPredict(GameObject bird)
    {
        float min_dist = Mathf.Infinity;
        Vector2 min_bird = new Vector2(Mathf.Infinity, 0);
        Vector2 min_go = new Vector2(0, Mathf.Infinity);
        foreach (GameObject o_bird in flock)
        {
            if (o_bird.tag.Equals("Bird"))
            {
                Vector2 dp = o_bird.transform.position - bird.transform.position;

                float adjusted_sight_distance = sight_distance * bird.GetComponent<Rigidbody2D>().velocity.magnitude;

                if (dp.magnitude > adjusted_sight_distance)
                    continue;
                
                Vector2 dv = o_bird.GetComponent<Rigidbody2D>().velocity - bird.GetComponent<Rigidbody2D>().velocity;

                float t_2_closest = -1 * (Vector2.Dot(dp, dv) / (dv.magnitude * dv.magnitude));

                if (t_2_closest < 0)
                    continue;

                Vector2 p_bird_close = (Vector2)bird.transform.position + bird.GetComponent<Rigidbody2D>().velocity * t_2_closest;
                Vector2 p_go_close = (Vector2)o_bird.transform.position + o_bird.GetComponent<Rigidbody2D>().velocity * t_2_closest;

                if (Vector2.Distance(p_bird_close, p_go_close) < min_dist)
                {
                    min_dist = Vector2.Distance(p_bird_close, p_go_close);
                    min_bird = p_bird_close;
                    min_go = p_go_close;
                }
            }
        }

        if (min_dist < min_coll_dist)
        {
            Vector2 accel = min_bird - min_go;
            float strength = Mathf.Min(max_accel / accel.magnitude, max_accel);
            bird.GetComponent<Rigidbody2D>().AddForce(accel.normalized * strength * .5f);
        }
    }
}
