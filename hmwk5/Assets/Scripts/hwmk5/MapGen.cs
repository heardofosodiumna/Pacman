using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapGen : MonoBehaviour
{
    public Gridd grid;
    public GameObject tree;
    public GameObject openSpace;
    public GameObject wall;
    Ray ray;
    RaycastHit hit;
    public ArrayList map = new ArrayList();


    // Use this for initialization
    void Start()
    {
        string file_path = "map.txt";
        StreamReader input = new StreamReader(file_path);

        while (!input.EndOfStream)
        {
            string line = input.ReadLine();
            map.Add(line.ToCharArray());
        }

        input.Close();

        for (int i = 0; i < map.Count; i++)
        {
            char[] line = (char[])map[i];
            for (int j = 0; j < line.Length; j++)
            {
                char tile = line[j];
                Vector3 loc = new Vector3(j, -1 * i, 0);
                Quaternion no_rotate = new Quaternion(0, 0, 0, 0);
                switch (tile)
                {
                    case '@':
                        Instantiate(wall, loc, no_rotate);
                        break;
                    case 'T':
                        Instantiate(tree, loc, no_rotate);
                        break;
                    case '.':
                        Instantiate(openSpace, loc, no_rotate);
                        break;

                }
            }
        }
        grid.CreateGrid();
    }

   
}