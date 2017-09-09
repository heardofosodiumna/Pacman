using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    Text text;
    public static int Totalscore = 0;

    // Use this for initialization
    void Start () {
        text = GetComponent<Text>();
    }

    public void addScore()
    {
        Totalscore += 1;
    }

    // Update is called once per frame
    void Update () {
        if (Totalscore < 10)
        {
            text.text = "0000" + Totalscore;
        }
        else if (Totalscore > 9)
        {
            text.text = "0000" + Totalscore;
        }

    }
}
