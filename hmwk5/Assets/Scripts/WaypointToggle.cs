using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WaypointToggle : MonoBehaviour {

    bool drawnOnce = true;
    public bool waypointOn = false;
    public GameObject wayPoints;
    //public GameObject tiles;
    public MapGen map;
    GameObject grid;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (map.hasDrawn)
        {
            if (drawnOnce)
            {
                grid = GameObject.FindGameObjectWithTag("Tiles");
                drawnOnce = false;
            }
            if (waypointOn)
            {
                wayPoints.SetActive(true);
                grid.SetActive(false);
            }
            else
            {
                wayPoints.SetActive(false);
                grid.SetActive(true);
            }
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
