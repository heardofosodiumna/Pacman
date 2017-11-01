using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class livesManager : MonoBehaviour {
    public float lives = 3.0f;
    public GameObject pacman;
    public GameObject spawner;
    Quaternion no_rotate = new Quaternion(0, 0, 0, 0);
    // Use this for initialization
    void Start () {
        lives = 3;
        
    }

    public void death()
    {
        lives -= 1;
        Debug.Log("You died. You have " + lives + " left!");
        if (lives>0)
            Instantiate(pacman, spawner.transform.position, no_rotate);
        else
        {
            Time.timeScale = 0f;
        }
        
    }

    public void restartLives()
    {
        lives = 3;
    }

    public float GetLives()
    {
        return lives;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
