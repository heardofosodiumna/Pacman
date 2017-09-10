using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PacmanMove : MonoBehaviour {
    public float speed = 0.4f;
    Vector2 direction;
    Vector2 dest;
    public livesManager livesManager;
    // Use this for initialization
    void Start () {
        dest = transform.position;
        
	}
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "ghost")
        {
            livesManager.death();
            Destroy(this.gameObject);
            Debug.Log("collided");
        }
       
    }
    // Update is called once per frame
    void FixedUpdate () {
        Vector2 pos = Vector2.MoveTowards(transform.position, dest, speed);
        GetComponent<Rigidbody2D>().MovePosition(pos);

        if ((Vector2)transform.position == dest)
        {
            if (Input.GetKey(KeyCode.UpArrow) && valid(Vector2.up + Vector2.up / 10))
            {
                direction = Vector2.up;
            }
            if (Input.GetKey(KeyCode.RightArrow) && valid(Vector2.right + Vector2.right / 10))
            {
                direction = Vector2.right;
            }
            if (Input.GetKey(KeyCode.DownArrow) && valid(Vector2.down + Vector2.down / 10))
            {
                direction = Vector2.down;
            }
            if (Input.GetKey(KeyCode.LeftArrow) && valid(Vector2.left + Vector2.left / 10))
            {
                direction = Vector2.left;
            }
            if(valid(direction+direction/10))
                dest = (Vector2)transform.position + direction;
        }

        Vector2 dir = dest - (Vector2)transform.position;
        GetComponent<Animator>().SetFloat("DirX", dir.x);
        GetComponent<Animator>().SetFloat("DirY", dir.y);
    }

    bool valid(Vector2 dir) {
        // Cast Line from 'next to Pac-Man' to 'Pac-Man'
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
        return (hit.collider == GetComponent<Collider2D>());
    }

}
