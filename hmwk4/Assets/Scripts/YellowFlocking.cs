using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class YellowFlocking : MonoBehaviour {

    YellowBird[] flock;
    public float k;
    public float max_accel;
    public float max_speed;

    int next;
    public float tolerance;
    public float arrive_radius;

    public GameObject path;
    List<Vector2> path_points;
    int path_index;
    public int future_index;

    public float cone_angle = 30f;
    public float max_rot_speed;
    public float slowRadius;
    public float dodge_dist;
    public bool cone = true;
    public float dist_tol;
    public float consider_dist;

    public Button button;
    // Use this for initialization
    void Start () {
        path_index = 0;
        path_points = path.GetComponent<CreatePath>().red_path_points;
        flock = GetComponentsInChildren<YellowBird>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        PathFollow();
        foreach (YellowBird bird in flock)
        {
            if (bird.transform.rotation.z >= .70 || bird.transform.rotation.z <= -.70)
                bird.gameObject.GetComponent<SpriteRenderer>().flipY = true;
            else
                bird.gameObject.GetComponent<SpriteRenderer>().flipY = false;

            checkCollision(bird);

            if (cone)
                ConeCheck(bird);
            else
                CollisionPredict(bird);

            float x_tot = 0;
            float y_tot = 0;
            foreach (YellowBird other_bird in flock)
            {
                x_tot += other_bird.transform.position.x;
                y_tot += other_bird.transform.position.y;
                Separate(bird, other_bird);
            }

            float x = (x_tot - bird.transform.position.x) / (flock.Length - 1);
            float y = (y_tot - bird.transform.position.y) / (flock.Length - 1);

            Vector2 target = new Vector2(x, y);
            Seek(bird, target);
            if (bird.GetComponent<Rigidbody2D>().velocity.magnitude > max_speed)
            {
                bird.GetComponent<Rigidbody2D>().velocity = bird.GetComponent<Rigidbody2D>().velocity.normalized * max_speed;
            }


            AlignToMovement(bird);
        }
    }

    void Separate(YellowBird bird, YellowBird other_bird)
    {
        Vector2 direction = bird.transform.position - other_bird.transform.position;
        float distance = direction.magnitude;
        float strength = Mathf.Min(k / distance * distance, max_accel);
        bird.GetComponent<Rigidbody2D>().AddForce(direction.normalized * strength);
    }

    void PathFollow()
    {
        float x_tot = 0;
        float y_tot = 0;
        foreach (YellowBird bird in flock)
        {
            x_tot += bird.transform.position.x;
            y_tot += bird.transform.position.y;
        }

        float x = x_tot / flock.Length;
        float y = y_tot / flock.Length;

        Vector2 flock_loc = new Vector2(x, y);

        float min_dist = Mathf.Infinity;
        int min_point_index = path_index;
        for (int i = path_index - 2; i <= path_index + 2; i++)
        {
            if (i < 0 || i > path_points.Count)
                continue;

            float distance = (flock_loc - path_points[i]).magnitude;
            if (distance < min_dist)
            {
                min_point_index = i;
                min_dist = distance;
            }
        }

        path_index = min_point_index;
        if (path_index + future_index >= path_points.Count)
        {
            print("HERE");
            Scene scene = SceneManager.GetActiveScene();
            for (int i = 0; i < 3; i++)
            {
                print(scene.name + "vs" + SceneManager.GetSceneAt(i).name);
                if (scene.name.Equals(SceneManager.GetSceneAt(i).name))
                {
                    print("NOTHERE");
                    SceneManager.LoadScene(i+1, LoadSceneMode.Single);
                }
            }

            path_index = path_points.Count - future_index - 1;
        }
        Vector2 target = path_points[path_index + future_index];

        foreach (YellowBird bird in flock)
        {
            Seek(bird, target);
        }
    }

    private void Seek(YellowBird bird, Vector2 target)
    {
        Vector2 accel = target - (Vector2)bird.transform.position;
        float strength = Mathf.Min(accel.magnitude, max_accel);
        bird.GetComponent<Rigidbody2D>().AddForce(accel.normalized * strength);
    }

    private void Flee(YellowBird bird, Vector2 target)
    {
        Vector2 accel = (Vector2)bird.transform.position - target;
        float strength = Mathf.Min(max_accel / accel.magnitude, max_accel);
        bird.GetComponent<Rigidbody2D>().AddForce(accel.normalized * strength);
    }

    public void CollisionVSCone()
    {
        cone = !cone;
        if(cone)
            button.GetComponentInChildren<Text>().text = "Cone Check Active";
        else
            button.GetComponentInChildren<Text>().text = "Collision Prediction Active";
    }

    public void ConeCheck(YellowBird bird)
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjects)
        {
            if (go.tag == "RedBird")
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
    }

    public void CollisionPredict(YellowBird bird)
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        float min_dist = Mathf.Infinity;
        Vector2 min_bird = new Vector2();
        Vector2 min_go = new Vector2();
        foreach (GameObject go in allObjects)
        {
            if (go.tag == "RedBird")
            {
                
                Vector2 dp = go.transform.position - bird.transform.position;

                if(dp.magnitude > consider_dist)
                    continue;

                Vector2 dv = go.GetComponent<Rigidbody2D>().velocity - bird.GetComponent<Rigidbody2D>().velocity;

                float t_2_closest = -1 * (Vector2.Dot(dp, dv) / (dv.magnitude * dv.magnitude));


                Vector2 p_bird_close = (Vector2)bird.transform.position + bird.GetComponent<Rigidbody2D>().velocity * t_2_closest;
                Vector2 p_go_close = (Vector2)go.transform.position + go.GetComponent<Rigidbody2D>().velocity * t_2_closest;

                if(Vector2.Distance(p_bird_close, p_go_close) < min_dist)
                {
                    min_dist = Vector2.Distance(p_bird_close, p_go_close);
                    min_bird = p_bird_close;
                    min_go = p_go_close;
                }
            }

        }

        if (Vector2.Distance(min_bird, min_go) < dist_tol)
        {
            //print("Yellow Bird" + bird.name + " is evading " + go.name);

            Vector2 accel = min_bird - min_go;
            float strength = Mathf.Min(max_accel / accel.magnitude, max_accel);
            bird.GetComponent<Rigidbody2D>().AddForce(accel.normalized * strength);
        }
    }

    public float isInsideDegree(YellowBird bird, GameObject target, float n)
    {
        Vector2 dir = target.transform.position - bird.transform.position;
        float angle = Vector2.Angle(dir, bird.transform.right);

        if (angle < n)
            return angle;

        return -1f;
    }

    public void checkCollision(YellowBird bird)
    {
        float angle = 30;
        angle *= Mathf.Deg2Rad;
        Vector3 red = (bird.transform.position + bird.transform.right);
        Debug.DrawLine(bird.transform.position, red, Color.red);

        float curent_angle = Mathf.Atan2(bird.transform.right.y, bird.transform.right.x);
        float greenx = (bird.transform.position.x + Mathf.Cos(curent_angle + angle));
        float greeny = (bird.transform.position.y + Mathf.Sin(curent_angle + angle));
        Vector3 green = new Vector3(greenx, greeny, 0);
        Debug.DrawLine(bird.transform.position, green, Color.green);

        float bluex = (bird.transform.position.x + Mathf.Cos(curent_angle - angle));
        float bluey = (bird.transform.position.y + Mathf.Sin(curent_angle - angle));
        Vector3 blue = new Vector3(bluex, bluey, 0);
        Debug.DrawLine(bird.transform.position, blue, Color.blue);
    }

    private void AlignToMovement(YellowBird bird)
    {
        Vector2 direction = (Vector2)bird.GetComponent<Rigidbody2D>().velocity.normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float rot_speed;
        if (Mathf.Abs(angle) > slowRadius)
            rot_speed = max_rot_speed;
        else
            rot_speed = angle / slowRadius;

        bird.transform.localRotation = Quaternion.Slerp(bird.transform.localRotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rot_speed);
    }
}
