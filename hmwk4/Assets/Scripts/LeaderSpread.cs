using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderSpread : MonoBehaviour {

    public float spread;
    float true_spread;
    bool no_coll = true;
    // Use this for initialization
    void Start () {
        true_spread = spread;
    }

    void FixedUpdate()
    {
        if (no_coll)
        {
            if (spread <= 0)
                spread = true_spread / 10f;
            else if (spread <= true_spread)
                spread = spread * (4.0f / 3.0f);
        }
        else
            no_coll = true;

    }


    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Wall")
        {
            no_coll = false;
            spread = spread * .75f;
        }
        else
            no_coll = true;
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Wall")
        {
            no_coll = false;
            spread = spread * .5f;
        }
        else
            no_coll = true;
    }
}
