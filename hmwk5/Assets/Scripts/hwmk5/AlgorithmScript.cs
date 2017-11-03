using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgorithmScript : MonoBehaviour {

    GameObject start;
    GameObject end;
    Vector3 startPos;
    Vector3 endPos;
    bool go = true;
    List<Node> final;
    // Use this for initialization
    void Start() {

        findAndGetStart();
        findAndGetEnd();
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        //need to contantly get this because the user can relocate start and end
        //if either is relocated the reference of the original is deleted and this the reference is null
        //thus if it is null we relocate it agains
        if (!start)
            findAndGetStart();
        if (!end)
            findAndGetEnd();

        //for demonstrations purposes
        //destroys objects 2 units left,right,up,down from start
        //this will give an error
        //Destroy(getObjectDown(getObjectDown(start)));
        //Destroy(getObjectUp(getObjectUp(start)));
        //Destroy(getObjectLeft(getObjectLeft(start)));
        //Destroy(getObjectRight(getObjectRight(start)));

        if(start && end&& go)
        {
            Node start = new Node(true, startPos);
            Node end = new Node(true, endPos);
            final = FindPathActual(start, end);
            StartCoroutine(Example());
            go = false;
        }

    }
    IEnumerator Example()
    {
        yield return new WaitForSeconds(.1f);
        DeleteNext();
    }
    void DeleteNext()
    {
        if(final.Count > 1)
        {
            Destroy(FindAt(final[0].pos));
            final.Remove(final[0]);
            StartCoroutine(Example());
        }
    }
    private List<Node> FindPathActual(Node start, Node target)
    {
        //Typical A* algorythm from here and on

        List<Node> foundPath = new List<Node>();

        //We need two lists, one for the nodes we need to check and one for the nodes we've already checked
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        //We start adding to the open set
        openSet.Add(start);
        
        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 0; i < openSet.Count; i++)
            {
                //We check the costs for the current node
                //You can have more opt. here but that's not important now

               // print("Current " + currentNode.pos+ "    " + currentNode.fCost );
               // print("temp " + openSet[i].pos + "    " + openSet[i].fCost);
               // print(" ");

                if (openSet[i].fCost < currentNode.fCost ||
                    (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                {
                    //and then we assign a new current node
                    if (currentNode.pos != openSet[i].pos)
                    {
                        //print("switch");
                        currentNode = openSet[i];
                    }
                }
            }
            //print("-------------");
            //print("Current* " + currentNode.pos);

            //we remove the current node from the open set and add to the closed set
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            //if the current node is the target node
            if (currentNode.pos == target.pos)
            {
                //that means we reached our destination, so we are ready to retrace our path
                print("GOAL!");
                return RetracePath(start, currentNode);
            }
            
            //if we haven't reached our target, then we need to start looking the neighbours
            foreach (Node neighbour in GetNeighbours(currentNode))
            {
                if (!closedSet.Contains(neighbour))
                {
                    //we create a new movement cost for our neighbours
                    float newMovementCostToNeighbour = currentNode.gCost + (int)Vector3.Distance(currentNode.pos, neighbour.pos);

                    //and if it's lower than the neighbour's cost
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {

                        //we calculate the new costs
                        neighbour.gCost = (int)newMovementCostToNeighbour;
                        neighbour.hCost = (int)Vector3.Distance(neighbour.pos, target.pos);
                        //Assign the parent node
                        neighbour.parentNode = currentNode;
                        //And add the neighbour node to the open set
                        if (!openSet.Contains(neighbour))
                        {
                              //  print(neighbour.pos);
                                
                                openSet.Add(neighbour);
                        }
                    }
                }
            }
            closedSet.Add(currentNode);
        }
        
        //we return the path at the end
        return foundPath;
    }
    List<Node> RetracePath(Node a, Node b)
    {
        List<Node> path = new List<Node>();
        Node currentNode = b;

        while (currentNode != a)
        {
            path.Add(currentNode);
            //by taking the parentNodes we assigned
            currentNode = currentNode.parentNode;
        }

        //then we simply reverse the list
        path.Reverse();

        return path;
    }
    List<Node> GetNeighbours(Node node)
    {
        List<Node> retList = new List<Node>();
        bool u, d, l, r;
        u = d = l = r= false;
        Vector3 searchPos = new Vector3(node.pos.x + 1, node.pos.y , 0);
        GameObject searchObj = FindAt(searchPos);

        if (searchObj && searchObj.gameObject.tag == "openSpace" || searchObj.gameObject.tag == "end")
        {
            r = true;
            retList.Add(new Node(true, searchPos));
        }
        searchPos = new Vector3(node.pos.x -1, node.pos.y, 0);
        searchObj = FindAt(searchPos);

        if (searchObj && searchObj.gameObject.tag == "openSpace" || searchObj.gameObject.tag == "end")
        {
            l = true;
            retList.Add(new Node(true, searchPos));
        }

        searchPos = new Vector3(node.pos.x, node.pos.y - 1, 0);
        searchObj = FindAt(searchPos);

        if (searchObj && searchObj.gameObject.tag == "openSpace" || searchObj.gameObject.tag == "end")
        {
            d = true;
            retList.Add(new Node(true, searchPos));
        }

        searchPos = new Vector3(node.pos.x , node.pos.y  + 1, 0);
        searchObj = FindAt(searchPos);

        if (searchObj && searchObj.gameObject.tag == "openSpace" || searchObj.gameObject.tag == "end")
        {
            u = true;
            retList.Add(new Node(true, searchPos));
        }
        if (u)
        {
            if (l)
            {
                searchPos = new Vector3(node.pos.x - 1, node.pos.y+1, 0);
                searchObj = FindAt(searchPos);

                if (searchObj && searchObj.gameObject.tag == "openSpace" || searchObj.gameObject.tag == "end")
                {
                    retList.Add(new Node(true, searchPos));
                }
            }
            if (r)
            {
                searchPos = new Vector3(node.pos.x + 1, node.pos.y + 1, 0);
                searchObj = FindAt(searchPos);

                if (searchObj && searchObj.gameObject.tag == "openSpace" || searchObj.gameObject.tag == "end")
                {
                    retList.Add(new Node(true, searchPos));
                }
            }
        }
        if (d)
        {
            if (l)
            {
                searchPos = new Vector3(node.pos.x - 1, node.pos.y - 1, 0);
                searchObj = FindAt(searchPos);

                if (searchObj && searchObj.gameObject.tag == "openSpace" || searchObj.gameObject.tag == "end")
                {
                    retList.Add(new Node(true, searchPos));
                }
            }
            if (r)
            {
                searchPos = new Vector3(node.pos.x + 1, node.pos.y - 1, 0);
                searchObj = FindAt(searchPos);

                if (searchObj && searchObj.gameObject.tag == "openSpace" || searchObj.gameObject.tag == "end")
                {
                    retList.Add(new Node(true, searchPos));
                }
            }
        }



        return retList;
    }
    void destroyPathToEnd()
    {
        Vector3 temp = startPos;
        Vector3 dir = endPos - startPos;
        if (dir.x != 0)
            dir.x = dir.x / Mathf.Abs(dir.x);

        if (dir.y != 0)
            dir.y = dir.y / Mathf.Abs(dir.y);
        temp = temp + dir;
        while (temp != endPos)
        {
            dir = endPos - temp;
            if (dir.x != 0)
                dir.x = dir.x / Mathf.Abs(dir.x);

            if (dir.y != 0)
                dir.y = dir.y / Mathf.Abs(dir.y);
            Destroy(FindAt(temp));
            temp = temp + dir;
        }

    }
    GameObject getObjectDown(GameObject aGameObject)
    {
        if (aGameObject == null)
        {
            print("You gave me a null object. JERK!");
            return null;
        }
        Vector3 loc = aGameObject.transform.position + new Vector3(0, -1, 0);
        return FindAt(loc);
    }
    GameObject getObjectUp(GameObject aGameObject)
    {
        if (aGameObject == null)
        {
            print("You gave me a null object. JERK!");
            return null;
        }
        Vector3 loc = aGameObject.transform.position + new Vector3(0, 1, 0);
        return FindAt(loc);
    }
    GameObject getObjectRight(GameObject aGameObject)
    {
        if (aGameObject == null)
        {
            print("You gave me a null object. JERK!");
            return null;
        }
        Vector3 loc = aGameObject.transform.position + new Vector3(1, 0, 0);
        return FindAt(loc);
    }
    GameObject getObjectLeft(GameObject aGameObject)
    {
        if (aGameObject == null)
        {
            print("You gave me a null object. JERK!");
            return null;
        }
        Vector3 loc = aGameObject.transform.position + new Vector3(-1, 0, 0);
        return FindAt(loc);
    }
    GameObject FindAt(Vector3 pos) {
        // get all colliders that intersect pos:
        Collider2D col = Physics2D.OverlapBox(pos, new Vector2(.5f, .5f), 0);
        return col.gameObject;
     }
    void findAndGetStart()
    {
        start = GameObject.FindGameObjectWithTag("start");
        if (start != null)
        {
            startPos = start.transform.position;
        }
    }
    void findAndGetEnd()
    {
        end = GameObject.FindGameObjectWithTag("end");
        if (end != null)
        {
            endPos = end.transform.position;
        }
    }

}
public class Node
{
    public bool walkable;
    public Vector3 pos;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public Node parentNode;

    public Node(bool _walkable, Vector3 _worldPos)
    {
        walkable = _walkable;
        pos = _worldPos;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
