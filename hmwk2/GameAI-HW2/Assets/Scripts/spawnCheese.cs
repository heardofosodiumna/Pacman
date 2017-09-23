using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnCheese : MonoBehaviour {

    
    public Vector3 pos1 = new Vector3(-16, 1.5f ,14);
    public Vector3 pos2 = new Vector3(9, 1.5f, 10);
    public Vector3 pos3 = new Vector3(-5, 1.5f, -6);
    public GameObject cheese;

    void Start()
    { 

        Instantiate(cheese, pos1, Quaternion.identity);
        Instantiate(cheese, Vector3.Lerp(pos1, pos2, 0.25f), Quaternion.identity);
        Instantiate(cheese, Vector3.Lerp(pos1, pos2, 0.5f), Quaternion.identity);
        Instantiate(cheese, Vector3.Lerp(pos1, pos2, 0.75f), Quaternion.identity);
        Instantiate(cheese, pos2, Quaternion.identity);
        Instantiate(cheese, Vector3.Lerp(pos2, pos3, 0.25f), Quaternion.identity);
        Instantiate(cheese, Vector3.Lerp(pos2, pos3, 0.5f), Quaternion.identity);
        Instantiate(cheese, Vector3.Lerp(pos2, pos3, 0.75f), Quaternion.identity);
        Instantiate(cheese, pos3, Quaternion.identity);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
