using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class highScoreManager : MonoBehaviour {
    public static int highscore;
    Text text;
    // Use this for initialization
    void Start () {
        text = GetComponent<Text>();
        highscore = PlayerPrefs.GetInt("highscore", highscore);
    }
	
	// Update is called once per frame
	void Update () {
		if (ScoreManager.Totalscore > highscore)
        {
            highscore = ScoreManager.Totalscore;
            PlayerPrefs.SetInt("highscore", highscore);
        }
        if (highscore < 10)
        {
            text.text = "0000" + highscore;
        }
        else if (highscore > 9 && highscore < 100)
        {
            text.text = "000" + highscore;
        }
        else if (highscore > 99 && highscore < 1000)
        {
            text.text = "00" + highscore;
        }
        else if (highscore > 999 && highscore < 10000)
        {
            text.text = "0" + highscore;
        }
        else if (highscore > 9999 && highscore < 100000)
        {
            text.text = "" + highscore;
        }
        else
        {
            text.text = "99999";
        }
    }
}
