using UnityEngine;
using System.Collections;

public class PacmanMove : MonoBehaviour {
    public float speed = 0.4f;
	// Use this for initialization
	void Start () {
        dest = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
	}

    bool valid(Vector2 dir) {
        // Cast Line from 'next to Pac-Man' to 'Pac-Man'
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
        return (hit.collider == GetComponent<Collider2D>());
    }

}
