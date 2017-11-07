using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class map2MouseOver : MonoBehaviour
{
    public MapMouseOver mmo;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitinfo;

        if (GetComponent<Collider>().Raycast(ray, out hitinfo, Mathf.Infinity))
        {
            mmo.handleInput(hitinfo);
        }
    }
}
