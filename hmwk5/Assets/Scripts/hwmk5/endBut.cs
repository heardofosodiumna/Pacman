using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class endBut : MonoBehaviour {
    bool activateEnd;
    Text text;
    string og;
    // Use this for initialization
    void Start()
    {
        activateEnd = false;
        text = GetComponentInChildren<Text>();
        og = text.text;
    }

    public void setEndFalse()
    {
        activateEnd = false;
        text.text = og;
    }
    public void setEndTrue()
    {

        activateEnd = true;
        text.text = "click to set location";

    }
    public bool GetEnd()
    {
        return activateEnd;
    }
}
