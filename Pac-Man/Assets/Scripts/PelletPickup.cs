using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelletPickup : MonoBehaviour {
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "pacman")
        {
            Destroy(this.gameObject);
            ScoreManager.Totalscore += 1;
        }
    }
}
