using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghostMove : MonoBehaviour {
    public LayerMask whatIsPellet;
    public float speed = 0.4f;
    Vector2 direction;
    Vector2 dest;
    public float choosedir = 1;
    
    public GameObject pellet;
    // Use this for initialization
    void Start () {
        choosedir = 2;
        dest = transform.position;
        Physics2D.IgnoreCollision(pellet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "ghost")
        {
            dest = (Vector2)transform.position - direction;
            direction = -direction;
            if (choosedir == 1)
                choosedir = 3;
            else if (choosedir == 3)
                choosedir = 1;
            else if (choosedir == 2)
                choosedir = 4;
            else
                choosedir = 1;

            dest.x = Mathf.Round(dest.x);
            dest.y = Mathf.Round(dest.y);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 pos = Vector2.MoveTowards(transform.position, dest, speed);
        GetComponent<Rigidbody2D>().MovePosition(pos);

        if ((Vector2)transform.position == dest)
        {
            if ((choosedir == 1) && valid(Vector2.up + Vector2.up / 10))
            {
                direction = Vector2.up;
            }
            if ((choosedir == 2) && valid(Vector2.right + Vector2.right / 10))
            {
                direction = Vector2.right;
            }
            if ((choosedir == 3) && valid(Vector2.down + Vector2.down / 10))
            {
                direction = Vector2.down;
            }
            if ((choosedir == 4) && valid(Vector2.left + Vector2.left / 10))
            {
                direction = Vector2.left;
            }
            if (valid(direction + direction / 10))
                dest = (Vector2)transform.position + direction;
            else
            {
                choosedir = Random.Range(1, 5);
                //Debug.Log(choosedir);
            }
        }

        Vector2 dir = dest - (Vector2)transform.position;
        GetComponent<Animator>().SetFloat("DirX", dir.x);
        GetComponent<Animator>().SetFloat("DirY", dir.y);
    }
    
    
    bool valid(Vector2 dir)
    {
        // Cast Line from 'next to Pac-Man' to 'Pac-Man'
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos, whatIsPellet);
        return ((hit.collider == GetComponent<Collider2D>()));
    }



}
