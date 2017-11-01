﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MapGen))]
public class MapMouseOver : MonoBehaviour {

    MapGen mapgen;
    ArrayList map;

    GameObject startButton;
    GameObject endButton;
    GameObject treeButton;

    public GameObject start_object;
    public GameObject end_object;

    GameObject start_tile;
    GameObject end_tile;

    void Start()
    {
        mapgen = GetComponent<MapGen>();
        map = GetComponent<MapGen>().map;

        startButton = GameObject.Find("Set Start");
        endButton = GameObject.Find("Set End");
        treeButton = GameObject.Find("Set Tree");
    }

    // Update is called once per frame
    void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitinfo;

        if (GetComponent<Collider>().Raycast(ray, out hitinfo, Mathf.Infinity))
        {
            //3x3 tiles
            //If moving up, check x-1, x, and x+1 of row y+1 and the same x's for y+2
            //      if any have tree, cannot move up

            //CONVERT FROM PIXEL TO ENCOMPASSING TILE
            //tile_x = Mathf.FloorToInt(pixel_x / 3)
            //tile_y = Mathf.FloorToInt(pixel_y / 3)

            //CONVERT FROM TILE TO CENTER PIXEL
            //pixel_x = tile_x * tileWidth + 1
            //pixel_y = tile_y * tileWidth + 1

            //TODO
            //A* https://github.com/sharpaccent/Astar-for-Unity/blob/master/Assets/Scripts/Pathfinder.cs
            //Find priority queue (make one?) in C#


            int x = Mathf.FloorToInt(hitinfo.point.x / mapgen.tileSize);
            int y = Mathf.FloorToInt(hitinfo.point.y / mapgen.tileSize);
            Debug.Log("TILE: " + x + " " + y);
            if(Input.GetMouseButtonDown(0))
            {
                if(startButton.GetComponent<startBut>().GetStart())
                {
                    if (start_tile != null)
                    {
                        Destroy(start_tile.gameObject);
                    }

                    start_tile = Instantiate(start_object, new Vector3(x+mapgen.tileSize/2f, y + mapgen.tileSize / 2f, -2), new Quaternion(0, 0, 0, 0));

                    startButton.GetComponent<startBut>().setStartFalse();
                }
                else if (endButton.GetComponent<endBut>().GetEnd())
                {
                    if (end_tile != null)
                    {
                        Destroy(end_tile.gameObject);
                    }

                    end_tile = Instantiate(end_object, new Vector3(x + mapgen.tileSize / 2f, y + mapgen.tileSize / 2f, -2), new Quaternion(0, 0, 0, 0));

                    endButton.GetComponent<endBut>().setEndFalse();
                }
            }
        }
	}
}
