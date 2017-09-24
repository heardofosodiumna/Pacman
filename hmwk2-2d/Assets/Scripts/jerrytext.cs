using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class jerrytext : MonoBehaviour {
    Text text;
    public static string behavior = "";

    // Use this for initialization
    void Start () {
        text = GetComponent<Text>();
    }

    public void setPathfinding()
    {
        behavior = "Pathfinding";
    }

    public void setEvade()
    {
        behavior = "Dynamic Evade";
    }
    // Update is called once per frame
    void Update () {
        text.text = "Jerry: " + behavior;
    }
}
