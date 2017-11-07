using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AlgorithmScript : MonoBehaviour
{

    GameObject start;
    GameObject end;
    Vector3 startPos;
    Vector3 endPos;

    List<Node> toChange;
    List<NodeAway> toChange2;
    public GameObject outer;
    public GameObject inner;
    bool found=false;
    MapGen mapgen;
    public Sprite aSprite;
    List<char[]> pixel_map;
    List<List<Node>> tile_map;
    List<Node> openSet = new List<Node>();
    HashSet<Node> closedSet = new HashSet<Node>();
    List<NodeAway> openSetW = new List<NodeAway>();
    HashSet<NodeAway> closedSetW = new HashSet<NodeAway>();
    List<NodeAway> allNodeAways = new List<NodeAway>();

    List<Node> path;
    List<NodeAway> path2;
    GameObject currline;
    GameObject engeline;
    bool manhattan = true;
    public float weightH;
    public WaypointToggle wt;
    public GameObject waypoints;

    public Material line_mat;

    public Material line_mat2;



    // Use this for initialization
    void Start()
    {
        weightH = 1;
        mapgen = GetComponent<MapGen>();
        pixel_map = mapgen.map;
        tile_map = new List<List<Node>>();

        toChange = new List<Node>();
        toChange2 = new List<NodeAway>();
        ConvertPixelMaptoTiles();
        StartCoroutine(ChangedSearched());

        StartCoroutine(ChangedSearched2());
        drawEdges();

    }
    // Update is called once per frame
    void FixedUpdate()
    {
   //     print(wt.waypointOn);
    }
    void drawEdges()
    {
        foreach (Transform first in waypoints.GetComponentInChildren<Transform>())
        {
            NodeAway tmp = new NodeAway(first.position.x, first.position.y);
            tmp.wayp = first.gameObject;
            allNodeAways.Add(tmp);

            foreach (Transform second in waypoints.GetComponentInChildren<Transform>())
            {
                if (first.localPosition != second.localPosition)
                {
                    if (clearPath(first, second))
                    {
                        engeline = new GameObject();
                        engeline.transform.position = new Vector3(first.localPosition.x, first.localPosition.y, -2);
                        engeline.AddComponent<LineRenderer>();
                        engeline.transform.parent = first;

                        LineRenderer lr = engeline.GetComponent<LineRenderer>();
                        lr.material = line_mat;
                        lr.startColor = Color.green;
                        lr.endColor = Color.green;

                        lr.startWidth = .3f;
                        lr.endWidth = .3f;
                        lr.positionCount = 2;
                        lr.SetPosition(0, first.localPosition);
                        lr.SetPosition(1, second.localPosition);
                    }
                }
            }
        }
    }
    IEnumerator finalPath()
    {
        yield return new WaitUntil(() => toChange.Count == 1);
        DeleteNext();
    }
    IEnumerator finalPath2()
    {
        yield return new WaitUntil(() => toChange2.Count == 1);
        DeleteNext2();
    }

    void DeleteNext()
    {
        DrawPath(path);
    }
    void DeleteNext2()
    {
        DrawPath2(path2);
    }

    IEnumerator ChangedSearched()
    {
        yield return new WaitForSeconds(0f);
        changeNext();
    }
    IEnumerator ChangedSearched2()
    {
        yield return new WaitForSeconds(.5f);
        changeNext2();
    }

    GameObject FindAt(Vector3 pos)
    {
        // get all colliders that intersect pos:
        Collider2D col = Physics2D.OverlapBox(pos, new Vector2(.5f, .5f), 0);
        if (col)
            return col.gameObject;
        return null;
    }

    void changeNext()
    {
        if (toChange.Count > 1)
        {
            Vector3 p = new Vector3((toChange[0].gridY) * 3 * mapgen.tileSize + mapgen.tileSize + .5f, (mapgen.map.Count - (toChange[0].gridX * 3)) * mapgen.tileSize - .5f, -1);
            toChange.Remove(toChange[0]);
            Instantiate(outer, p, new Quaternion(0, 0, 0, 0));
        }
        StartCoroutine(ChangedSearched());
    }
    void changeNext2()
    {
        if (toChange2.Count > 1)
        {
            Vector3 p = new Vector3((toChange2[0].gridX), toChange2[0].gridY , -10);
            toChange2.Remove(toChange2[0]);
            Instantiate(inner, p, new Quaternion(0, 0, 0, 0));
        }
        StartCoroutine(ChangedSearched2());
    }

    public void Search()
    {
        if (currline != null)
            Destroy(currline);

        toChange.Clear();
        toChange2.Clear();
        closedSet.Clear();
        closedSetW.Clear();
        openSet.Clear();
        openSetW.Clear();
        foreach (GameObject x in GameObject.FindGameObjectsWithTag("wall"))
        {
            Destroy(x);
        }
        //need to contantly get this because the user can relocate start and end
        //if either is relocated the reference of the original is deleted and this the reference is null
        //thus if it is null we relocate it agains
        findAndGetStart();
        findAndGetEnd();
        if (start && end)
        {
           
            Node start_node = tile_map[(int)startPos.x][(int)startPos.y];
            Node end_node = tile_map[(int)endPos.x][(int)endPos.y];

            if (!wt.waypointOn)
            {
                path = FindPathActual(start_node, end_node);
                if (path.Count != 0)
                {
                    StartCoroutine(finalPath());
                }
            }
            else
            {
                path2 = FindPathWayPoint();
                if (path2.Count != 0)
                {
                    StartCoroutine(finalPath2());
                }
            }
        }
    }

    public void ConvertPixelMaptoTiles()
    {
        List<List<Node>> new_tile_map = new List<List<Node>>();
        for(int x = 0; x < pixel_map.Count-1; x+=3)
        {
            List<Node> row = new List<Node>();
            for(int y = 1; y < pixel_map[0].Length - 1; y+=3)
            {
                Node tile = new Node(Mathf.FloorToInt(x/3), Mathf.FloorToInt(y/3));
                //check if walkable
                for (int k = -1; k < 2; k++)
                {
                    for (int l = -1; l < 2; l++)
                        if(x+k>0 && y+l>0)
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

        currline = new GameObject();
        currline.transform.position = new Vector3(path[0].gridY, path[0].gridX, -2);
        currline.AddComponent<LineRenderer>();

        LineRenderer lr = currline.GetComponent<LineRenderer>();
        lr.material = line_mat2;
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
    private void DrawPath2(List<NodeAway> path)
    {

        currline = new GameObject();
        currline.transform.position = new Vector3(path[0].gridX, path[0].gridY, -4);
        currline.AddComponent<LineRenderer>();

        LineRenderer lr = currline.GetComponent<LineRenderer>();
        lr.material = line_mat2;
        lr.startColor = Color.green;
        lr.endColor = Color.green;

        lr.startWidth = 1f;
        lr.endWidth = 1f;
        lr.positionCount = path.Count;

        for (int i = 0; i < path.Count; i++)
        {
            Vector3 pos = new Vector3(path[i].gridX, path[i].gridY, -4);

            lr.SetPosition(i, pos);
        }
    }

    public void toggleH()
    {
        manhattan = !manhattan;
        if (manhattan)
            GameObject.FindGameObjectWithTag("hbut").GetComponentInChildren<Text>().text = "Heuristic is: Manhattan ";
        else
            GameObject.FindGameObjectWithTag("hbut").GetComponentInChildren<Text>().text = "Heuristic is: Euclidian ";
    }

    private List<Node> FindPathActual(Node start, Node target)
    {
        //Typical A* algorythm from here and on
        //We need two lists, one for the nodes we need to check and one for the nodes we've already checked

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
            toChange.Add(currentNode);

            //if the current node is the target node
            if (currentNode.gridX == target.gridX && currentNode.gridY == target.gridY)
            {
                //that means we reached our destination, so we are ready to retrace our path
                found = true;
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
                        if (manhattan)
                            neighbour.hCost = (int)( Mathf.Abs(neighbour.gridX - target.gridX) + Mathf.Abs(neighbour.gridY - target.gridY));
                        else
                            neighbour.hCost = (int)( Mathf.Sqrt(  Mathf.Pow(Mathf.Abs(neighbour.gridX - target.gridX), 2) + Mathf.Pow(Mathf.Abs(neighbour.gridY - target.gridY), 2)));
                        neighbour.hCost = (int)(neighbour.hCost * weightH);
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
                        {
                           openSet.Add(neighbour);
                           toChange.Add(neighbour);
                        }
                    }
                }
            }
        }
        return new List<Node>();
    }

   bool clearPath(Transform a, Transform b)
   {
      //  float dist = Vector3.Distance(a.localPosition, b.localPosition);
        float dist = pixel_map.Count*pixel_map[0].Length;
        dist = dist/10;
        for (int j=1; j<dist; j++)
        {
            //print("Towards: " + b.position);
            Vector3 point = Vector3.Lerp(a.localPosition, b.localPosition, j / dist);
            if (pixel_map[(int)(pixel_map.Count - point.y)][(int)point.x] != '.')
            {
                return false;
            }
        }
        return true;
   }

    private List<NodeAway> FindPathWayPoint()
    {
        Vector3 startloc = GameObject.FindGameObjectWithTag("start").transform.position;
        Vector3 closestS = new Vector3(-999, -999, 0);
        Vector3 endloc = GameObject.FindGameObjectWithTag("end").transform.position;
        Vector3 closestE = new Vector3(-999, -999, 0);
        GameObject startW = new GameObject();
        GameObject endW = new GameObject();
        float closestDistS = Vector3.Distance(startloc, closestS);
        float closestDistE = Vector3.Distance(startloc, closestE);
        //get the nearest waypoint
        foreach (Transform x in waypoints.GetComponentInChildren<Transform>())
        {
            if (Vector3.Distance(startloc, x.localPosition) < closestDistS)
            {
                closestDistS = Vector3.Distance(startloc, x.localPosition);
                closestS = x.localPosition;
                startW = x.gameObject;
            }
            if (Vector3.Distance(endloc, x.localPosition) < closestDistE)
            {
                closestDistE = Vector3.Distance(endloc, x.localPosition);
                closestE = x.localPosition;
                endW = x.gameObject;
            }
        }
        //closest is the locaitons of the closest  object in word space

        NodeAway startN = new NodeAway(closestS.x, closestS.y);
        startN.wayp = startW;
        openSetW.Add(startN);
        StartCoroutine(ChangedSearched2());
        while (openSetW.Count > 0)
        {
            NodeAway currentNode = openSetW[0];
            for (int i = 0; i < openSetW.Count; i++)
            {
                //We check the costs for the current node
                //You can have more opt. here but that's not important now
                if (openSetW[i].fCost < currentNode.fCost ||
                    (openSetW[i].fCost == currentNode.fCost && openSetW[i].hCost < currentNode.hCost))
                {
                    //and then we assign a new current node
                    currentNode = openSetW[i];
                }
            }


            //we remove the current node from the open set and add to the closed set
            openSetW.Remove(currentNode);
            closedSetW.Add(currentNode);
           // print("curr: " + currentNode.gridX + " " + currentNode.gridY);
            toChange2.Add(currentNode);

            //if the current node is the target node
            if (currentNode.gridX == closestE.x && currentNode.gridY == closestE.y)
            {
                //that means we reached our destination, so we are ready to retrace our path
               // print("GOAL");
                // found = true;
                return RetracePathW(startN, currentNode);
            }
            foreach (NodeAway neighbour in getWayNei(currentNode))
            {
                if (!closedSetW.Contains(neighbour))
                {
                    float newMovementCostToNeighbour = Vector2.Distance(new Vector2(neighbour.gridX, neighbour.gridY), new Vector2(closestS.x, closestS.y));

                    if (newMovementCostToNeighbour < neighbour.gCost || !openSetW.Contains(neighbour))
                    {
                        neighbour.gCost = (int)newMovementCostToNeighbour;
                        if (manhattan)
                            neighbour.hCost = (int)(Vector2.Distance(new Vector2(neighbour.gridX,neighbour.gridY), closestE));
                        else
                            neighbour.hCost = (int)(Mathf.Sqrt(Mathf.Pow(Mathf.Abs(neighbour.gridX - endloc.x), 2) + Mathf.Pow(Mathf.Abs(neighbour.gridY - endloc.y), 2)));

                        neighbour.hCost = (int)(neighbour.hCost * weightH);

                        neighbour.parentNode = currentNode;
                        bool contains = false;
                        foreach (NodeAway n in openSetW)
                        {
                            if (n.gridX == neighbour.gridX && n.gridY == neighbour.gridY)
                                contains = true;
                        }

                        if (!contains)
                        {
                            openSetW.Add(neighbour);
                        }
                    }
                }
            }
        }
        return new List<NodeAway>();

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

    List<NodeAway> RetracePathW(NodeAway startNode, NodeAway endNode)
    {
       // print("Start: " + startNode.gridX + " "+startNode.gridY);
        //print("End: " + endNode.gridX + " "+endNode.gridY);
        List<NodeAway> path = new List<NodeAway>();
        NodeAway currentNode = endNode;

        while (currentNode.gridX != startNode.gridX || currentNode.gridY != startNode.gridY )
        {
            //print(lol);
            path.Add(currentNode);
            //by taking the parentNodes we assigned
            currentNode = currentNode.parentNode;
           // print(currentNode.gridX + " " + currentNode.gridY);
        }
        path.Add(startNode);

        //then we simply reverse the list
        path.Reverse();

        return path;
    }


    List<NodeAway> getWayNei(NodeAway a)
    {
        List<NodeAway> neighs = new List<NodeAway>();
        foreach (Transform line in a.wayp.GetComponentInChildren<Transform>())
        {
            Vector3 pos = new Vector3(line.GetComponent<LineRenderer>().GetPosition(1).x, line.GetComponent<LineRenderer>().GetPosition(1).y, -2);
            foreach(NodeAway nd in allNodeAways)
            {
                if(nd.gridX == pos.x && nd.gridY == pos.y)
                {
                    neighs.Add(nd);
                }
            }
        }
        return neighs;
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
public class NodeAway
{
    public float gridX;
    public float gridY;

    public int gCost;
    public int hCost;
    public NodeAway parentNode;
    public GameObject wayp;
    public List<NodeAway> neighs;

    public NodeAway(float x, float y)
    {
        gridX = x;
        gridY = y;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
    
}

