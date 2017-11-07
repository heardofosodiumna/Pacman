using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToggleScenes : MonoBehaviour {

    public bool onFirst;
	// Use this for initialization
	void Start ()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            onFirst = true;
        else
            onFirst = false;

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Toggle()
    {
        onFirst = !onFirst;
        if (!onFirst)
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }

    }
}
