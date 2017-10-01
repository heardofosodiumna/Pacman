using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocking : MonoBehaviour {

    YellowBird[] flock;
    public float k;
    public float max_accel;
    public float max_speed;

    blackDot[] Path;
    int next;
    public float tolerance;
    public float arrive_radius;
    // Use this for initialization
    void Start () {
        flock = GetComponentsInChildren<YellowBird>();

        Path = GetComponentsInChildren<blackDot>();
        flock = GetComponentsInChildren<YellowBird>();
        foreach (YellowBird f in flock)
        {
            YellowBird script = (YellowBird)f.GetComponent(typeof(YellowBird));
            script.setPos(getNextPos(script.getNext()));
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        foreach (YellowBird bird in flock)
        {
            //float x_tot = 0;
            //float y_tot = 0;
            foreach (YellowBird other_bird in flock)
            {
                //x_tot += other_bird.transform.position.x;
                //y_tot += other_bird.transform.position.y;
                Cohesion(bird, other_bird);
                Separate(bird, other_bird);
            }

            PathFollow(bird);

            //float x = (x_tot - bird.transform.position.x) / (flock.Length - 1);
            //float y = (y_tot - bird.transform.position.y) / (flock.Length - 1);

            //Vector2 target = new Vector2(x, y);   
        }
    }

    void Separate(YellowBird bird, YellowBird other_bird)
    {
        Vector2 direction = bird.transform.position - other_bird.transform.position;
        float distance = direction.magnitude;
        float strength = Mathf.Min(k / distance * distance, max_accel);
        strength = Mathf.Min(strength, max_speed);
        bird.GetComponent<Rigidbody2D>().AddForce(direction.normalized * strength);
    }

    void Cohesion(YellowBird bird, YellowBird other_bird)
    {
        Vector2 direction = other_bird.transform.position - bird.transform.position;
        float distance = direction.magnitude;
        float cohesion_strength = Mathf.Min(distance * distance / k, max_accel);
        bird.GetComponent<Rigidbody2D>().AddForce(direction.normalized * cohesion_strength);
    }

    void PathFollow(YellowBird bird)
    {
        YellowBird script = (YellowBird)bird.GetComponent(typeof(YellowBird));
        if ((bird.transform.position - script.getPos()).magnitude < tolerance)
        {
            script.incNext();
            script.setPos(getNextPos(script.getNext()));
        }
        Vector2 direction = script.getPos() - bird.transform.position;
        float distance = direction.magnitude;
        float strength = max_speed;

        if (distance < arrive_radius)
        {
            float target_speed = max_speed * (distance / arrive_radius);
            Vector2 target_velocity = target_speed * direction;
            Rigidbody2D rb = bird.GetComponent<Rigidbody2D>();
            Vector2 thrust = target_velocity - rb.velocity;
            rb.AddForce(thrust / (distance / rb.velocity.magnitude));
        }
        else
        {
            bird.GetComponent<Rigidbody2D>().AddForce(direction.normalized * strength);
        }
    }

    Vector3 getNextPos(int n)
    {
        Vector3 x = Path[n].transform.position;
        return x;
    }
}
