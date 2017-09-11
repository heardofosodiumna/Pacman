using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    Text text;
    public static int Totalscore = 0;
    

    // Use this for initialization
    void Start () {
        Totalscore = 0;
        text = GetComponent<Text>();
    }

    public void addScore()
    {
        Totalscore += 1;
    }

    public static void restartScore()
    {
        Totalscore = 0;
    }
    // Update is called once per frame
    void Update () {
        if (Totalscore < 10)
        {
            text.text = "0000" + Totalscore;
        }
        else if (Totalscore > 9 && Totalscore < 100)
        {
            text.text = "000" + Totalscore;
        }
        else if (Totalscore > 99 && Totalscore < 1000)
        {
            text.text = "00" + Totalscore;
        }
        else if (Totalscore > 999 && Totalscore < 10000)
        {
            text.text = "0" + Totalscore;
        }
        else if (Totalscore > 9999 && Totalscore < 100000)
        {
            text.text = ""+Totalscore;
        }
        else
        {
            text.text = "99999";
        }
    }
}
