using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapGen : MonoBehaviour {

    public GameObject big_bottom_left_corner;
    public GameObject big_bottom_right_corner;
    public GameObject big_top_left_corner;
    public GameObject bottom_double;
    public GameObject bottom_left_corner_double;
    public GameObject bottom_left_corner_single;
    public GameObject bottom_right_corner_double;
    public GameObject bottom_right_corner_single;
    public GameObject ghosthouse_bottom_left_corner;
    public GameObject ghosthouse_bottom_right_corner;
    public GameObject ghosthouse_bottom;
    public GameObject ghosthouse_left;
    public GameObject ghosthouse_right;
    public GameObject ghosthouse_top;
    public GameObject ghosthouse_top_left_corner;
    public GameObject ghosthouse_top_right_corner;
    public GameObject left_double;
    public GameObject right_double;
    public GameObject small_bottom;
    public GameObject small_bottom_left_corner;
    public GameObject small_bottom_right_corner;
    public GameObject small_left;
    public GameObject small_right;
    public GameObject small_top;
    public GameObject small_top_left_corner;
    public GameObject small_top_right_corner;
    public GameObject square_bottom_left_corner;
    public GameObject square_bottom_right_corner;
    public GameObject square_top_left_corner;
    public GameObject square_top_right_corner;
    public GameObject straight_double_bottom_left_corner;
    public GameObject straight_double_bottom_right_corner;
    public GameObject straight_double_top_left_corner;
    public GameObject straight_double_top_right_corner;
    public GameObject straight_vertical_double_bottom_left_corner;
    public GameObject straight_vertical_double_bottom_right_corner;
    public GameObject straight_vertical_double_top_left_corner;
    public GameObject straight_vertical_double_top_right_corner;
    public GameObject top_double;
    public GameObject top_left_corner_double;
    public GameObject top_left_corner_single;
    public GameObject top_right_corner_double;
    public GameObject top_right_corner_single;

    public ArrayList map = new ArrayList();



    // Use this for initialization
    void Start () {
       string file_path = "map.txt";
        StreamReader input= new StreamReader(file_path);

        while (!input.EndOfStream)
        {
            string line = input.ReadLine();
            map.Add(line.ToCharArray());
        }

        input.Close();

        for (int i = 0; i < map.Count; i++)
        {
            char[] line = (char[])map[i];
            for(int j = 0; j < line.Length; j++)
            {
                char tile = line[j];
                Vector3 loc = new Vector3(j, -1 * i, 0);
                Quaternion no_rotate = new Quaternion(0, 0, 0, 0);
                switch(tile)
                {
                    case 'T':
                        Instantiate(top_double, loc, no_rotate);
                        break;
                    case 'L':
                        Instantiate(left_double, loc, no_rotate);
                        break;
                    case 'R':
                        Instantiate(right_double, loc, no_rotate);
                        break;
                    case 'B':
                        Instantiate(bottom_double, loc, no_rotate);
                        break;
                }
            }
        }

    }
}
