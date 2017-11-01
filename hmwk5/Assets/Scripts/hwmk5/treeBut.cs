using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class treeBut : MonoBehaviour {

    bool activateTree;
    Text text;
    string og;
    // Use this for initialization
    void Start()
    {
        activateTree = false;
        text = GetComponentInChildren<Text>();
        og = text.text;
    }

    public void setTree()
    {
        activateTree = !activateTree;
        if (activateTree)
        {
            text.text = "click to set location";
        }
        else
        {
            text.text = og;
        }
    }
    public bool GetTree()
    {
        return activateTree;
    }
}
