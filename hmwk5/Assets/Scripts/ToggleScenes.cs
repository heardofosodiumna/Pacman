using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToggleScenes : MonoBehaviour {

    bool onFirst = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Toggle()
    {
        onFirst = !onFirst;
        if (!onFirst)
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }

    }
}
