﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgorithmScript : MonoBehaviour
{

    GameObject start;
    GameObject end;
    Vector3 startPos;
    Vector3 endPos;

    MapGen mapgen;
    List<char[]> pixel_map;
    List<List<Node>> tile_map;

    public Material line_mat;

    bool done;


    // Use this for initialization
    void Start()
    {
        mapgen = GetComponent<MapGen>();
        pixel_map = mapgen.map;
        tile_map = new List<List<Node>>();

        ConvertPixelMaptoTiles();
        findAndGetStart();
        findAndGetEnd();

        done = false;
    }

    // Update is called once per frame
    void Update()
    {
        //need to contantly get this because the user can relocate start and end
        //if either is relocated the reference of the original is deleted and this the reference is null
        //thus if it is null we relocate it agains
        if (!start)
            findAndGetStart();
        if (!end)
            findAndGetEnd();

        
        if(start && end && !done)
        {
            Node start_node = tile_map[(int)startPos.x][(int)startPos.y];
            Node end_node = tile_map[(int)endPos.x][(int)endPos.y];

            List<Node> path = FindPathActual(start_node, end_node);
            if (path.Count != 0)
            {
                DrawPath(path);
                done = true;
            }
        }
    }

    public void ConvertPixelMaptoTiles()
    {
        List<List<Node>> new_tile_map = new List<List<Node>>();
        for(int x = 1; x < pixel_map.Count-1; x+=3)
        {
            List<Node> row = new List<Node>();
            for(int y = 1; y < pixel_map[0].Length - 1; y+=3)
            {
                Node tile = new Node(Mathf.FloorToInt(x/3), Mathf.FloorToInt(y/3));
                //check if walkable
                for (int k = -1; k < 2; k++)
                {
                    for (int l = -1; l < 2; l++)
                        if (pixel_map[x + k][y + l] != '.')
                        {
                            tile.walkable = false;
                            break;
                        }
                    if (!tile.walkable)
                        break;
                }
                row.Add(tile);
            }
            new_tile_map.Add(row);
        }

        tile_map = new_tile_map;
    }

    void findAndGetStart()
    {
        start = GameObject.FindGameObjectWithTag("start");
        if (start != null)
        {
            int x = Mathf.FloorToInt(start.transform.position.x / mapgen.tileSize);
            int y = Mathf.FloorToInt(start.transform.position.y / mapgen.tileSize);

            y = mapgen.map.Count - y;

            x = Mathf.FloorToInt(x / 3);
            y = Mathf.FloorToInt(y / 3);
            startPos = new Vector2(y, x);

            print(startPos);
        }
    }

    void findAndGetEnd()
    {
        end = GameObject.FindGameObjectWithTag("end");
        if (end != null)
        {
            int x = Mathf.FloorToInt(end.transform.position.x / mapgen.tileSize);
            int y = Mathf.FloorToInt(end.transform.position.y / mapgen.tileSize);

            y = mapgen.map.Count - y;

            x = Mathf.FloorToInt(x / 3);
            y = Mathf.FloorToInt(y / 3);
            endPos = new Vector2(y, x);
        }
    }

    private void DrawPath(List<Node> path)
    {
        GameObject path_line = new GameObject();
        path_line.transform.position = new Vector3(path[0].gridY, path[0].gridX, -2);
        path_line.AddComponent<LineRenderer>();

        LineRenderer lr = path_line.GetComponent<LineRenderer>();
        lr.material = line_mat;
        lr.startColor = Color.green;
        lr.endColor = Color.green;

        lr.startWidth = .5f;
        lr.endWidth = .5f;
        lr.positionCount = path.Count;

        for(int i = 0; i < path.Count; i++)
        {
            Vector3 pos = new Vector3((path[i].gridY) * 3 * mapgen.tileSize + mapgen.tileSize + .5f, (mapgen.map.Count - (path[i].gridX * 3)) * mapgen.tileSize-.5f, -3);
            
            lr.SetPosition(i, pos);
        }
    }

    private List<Node> FindPathActual(Node start, Node target)
    {
        //Typical A* algorythm from here and on
        //We need two lists, one for the nodes we need to check and one for the nodes we've already checked
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        //We start adding to the open set
        openSet.Add(start);
        //StartCoroutine(ChangedSearched());

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 0; i < openSet.Count; i++)
            {
                //We check the costs for the current node
                //You can have more opt. here but that's not important now
                if (openSet[i].fCost < currentNode.fCost ||
                    (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                {
                    //and then we assign a new current node
                    currentNode = openSet[i];
                }
            }

            //we remove the current node from the open set and add to the closed set
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            //if the current node is the target node
            if (currentNode.gridX == target.gridX && currentNode.gridY == target.gridY)
            {
                //that means we reached our destination, so we are ready to retrace our path
                return RetracePath(start, currentNode);
            }

            //if we haven't reached our target, then we need to start looking the neighbours
            foreach (Node neighbour in GetNeighbours(currentNode))
            {
                if (!closedSet.Contains(neighbour))
                {
                    //we create a new movement cost for our neighbours
                    float newMovementCostToNeighbour = currentNode.gCost + 1;

                    //and if it's lower than the neighbour's cost
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {

                        //we calculate the new costs
                        neighbour.gCost = (int)newMovementCostToNeighbour;
                        neighbour.hCost = Mathf.Abs(neighbour.gridX - target.gridX) + Mathf.Abs(neighbour.gridY - target.gridY);
                        //Assign the parent node
                        neighbour.parentNode = currentNode;
                        //And add the neighbour node to the open set

                        bool contains = false;
                        foreach (Node n in openSet)
                        {
                            if (n.gridX == neighbour.gridX && n.gridY == neighbour.gridY)
                                contains = true;
                        }

                        if (!contains)
                            openSet.Add(neighbour);
                    }
                }
            }
        }
        return new List<Node>();
    }

    List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            //by taking the parentNodes we assigned
            currentNode = currentNode.parentNode;
        }

        path.Add(startNode);

        //then we simply reverse the list
        path.Reverse();
    
        return path;
    }

    List<Node> GetNeighbours(Node node)
    {
        List<Node> retList = new List<Node>();

        int x = node.gridX;
        int y = node.gridY;

        if (x+1 < tile_map.Count && tile_map[x+1][y].walkable)
        {
            retList.Add(tile_map[x+1][y]);
        }
        if (x-1 >= 0 && tile_map[x-1][y].walkable)
        {
            retList.Add(tile_map[x-1][y]);
        }
        if (y+1 < tile_map[0].Count && tile_map[x][y+1].walkable)
        {
            retList.Add(tile_map[x][y+1]);
        }
        if (y-1 >= 0 && tile_map[x][y-1].walkable)
        {
            retList.Add(tile_map[x][y-1]);
        }
        return retList;
    }
}

public class Node
{
    public bool walkable;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public Node parentNode;

    public Node(int x, int y)
    {
        gridX = x;
        gridY = y;

        walkable = true;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
