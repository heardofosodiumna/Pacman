using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class startBut : MonoBehaviour {

    bool activateStart;
    Text text;
    string og;
    // Use this for initialization
    void Start () {
        activateStart =false;
        text = GetComponentInChildren<Text>();
        og = text.text;
    }

    public void setStartFalse()
    {
        activateStart = false;
        text.text = og;
    }
    public void setStartTrue()
    {
        activateStart = true;
        text.text = "click to set location"; 
    }
    public bool GetStart()
    {
        return activateStart;
    }
}
