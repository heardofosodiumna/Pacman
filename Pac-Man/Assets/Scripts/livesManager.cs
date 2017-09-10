using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class livesManager : MonoBehaviour {
    public float lives = 3.0f;
    // Use this for initialization
    void Start () {
        lives = 3;
        
    }

    public void death()
    {
        lives -= 1;
        Debug.Log("You died. You have " + lives + " left!");
        print("death");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
