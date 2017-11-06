using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class weightH : MonoBehaviour {
    public Slider hSlider;
    public AlgorithmScript playerScript;
    
    void FixedUpdate()
    {
        GetComponentInChildren<Text>().text = "Weight of Heurisitc = "+ hSlider.value.ToString("0.0");
    }
    public void OnhSliderChanged()
    {
        playerScript.weightH = hSlider.value;
    }
}
