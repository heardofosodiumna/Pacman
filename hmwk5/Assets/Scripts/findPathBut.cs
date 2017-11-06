using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class findPathBut : MonoBehaviour {

    AlgorithmScript sc;
    // Use this for initialization
	void Start () {
        sc = FindObjectOfType<AlgorithmScript>();
        
    }
	
	// Update is called once per frame
	public void search()
    {
        sc.Search();
    }
}
