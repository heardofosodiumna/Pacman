using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class secondlife : MonoBehaviour {

    public livesManager livesManager;
    public float lives;

    // Use this for initialization
    void Start () {
        lives = livesManager.GetLives();
	}
	
	// Update is called once per frame
	void Update () {
        lives = livesManager.GetLives();
		if (lives == 2)
        {
            Destroy(this.gameObject);
            Debug.Log("2 lives left");
        }
	}
}
