using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WaypointToggle : MonoBehaviour {

    public bool waypointOn = false;
    public GameObject wayPoints;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (waypointOn)
        {
            wayPoints.SetActive(true);
        }
        else
        {
            wayPoints.SetActive(false);
        }
	}

    public void toggleOn()
    {
        waypointOn = !waypointOn;
        if (waypointOn)
            GameObject.FindGameObjectWithTag("Waybut").GetComponentInChildren<Text>().text = "Representation: Waypoints";
        else
            GameObject.FindGameObjectWithTag("Waybut").GetComponentInChildren<Text>().text = "Representation: Tiles";
    }
}
