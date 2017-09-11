using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapGen : MonoBehaviour {

    public GameObject big_bottom_left_corner;
    public GameObject big_bottom_right_corner;
    public GameObject big_top_left_corner;
    public GameObject big_top_right_corner;
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
    public GameObject ghosthouse_left_door;
    public GameObject ghosthouse_right_door;
    public GameObject ghosthouse_door;

    public livesManager lm;


    public GameObject pellet;

    public ArrayList map = new ArrayList();



    // Use this for initialization
    void Start () {
        lm.lives = 3;
        
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
                    case '-':
                        Instantiate(pellet, loc, no_rotate);
                        break;
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
                    case 'Q':
                        Instantiate(top_left_corner_double, loc, no_rotate);
                        break;
                    case 'P':
                        Instantiate(top_right_corner_double, loc, no_rotate);
                        break;
                    case 'Z':
                        Instantiate(bottom_left_corner_double, loc, no_rotate);
                        break;
                    case 'M':
                        Instantiate(bottom_right_corner_double, loc, no_rotate);
                        break;
                    case 'A':
                        Instantiate(straight_double_top_right_corner, loc, no_rotate);
                        break;
                    case 'S':
                        Instantiate(straight_double_top_left_corner, loc, no_rotate);
                        break;
                    case 'W':
                        Instantiate(small_top_left_corner, loc, no_rotate);
                        break;
                    case 'O':
                        Instantiate(small_top_right_corner, loc, no_rotate);
                        break;
                    case 'X':
                        Instantiate(small_bottom_left_corner, loc, no_rotate);
                        break;
                    case 'N':
                        Instantiate(small_bottom_right_corner, loc, no_rotate);
                        break;
                    case 'k':
                        Instantiate(small_bottom_left_corner, loc, no_rotate);
                        break;
                    case ';':
                        Instantiate(small_bottom_right_corner, loc, no_rotate);
                        break;
                    case 'i':
                        Instantiate(small_left, loc, no_rotate);
                        break;
                    case 'o':
                        Instantiate(small_right, loc, no_rotate);
                        break;
                    case 'e':
                        Instantiate(ghosthouse_top_left_corner, loc, no_rotate);
                        break;
                    case 'y':
                        Instantiate(ghosthouse_top_right_corner, loc, no_rotate);
                        break;
                    case 'c':
                        Instantiate(ghosthouse_bottom_left_corner, loc, no_rotate);
                        break;
                    case 'v':
                        Instantiate(ghosthouse_bottom_right_corner, loc, no_rotate);
                        break;
                    case 'a':
                        Instantiate(ghosthouse_left, loc, no_rotate);
                        break;
                    case 's':
                        Instantiate(ghosthouse_right, loc, no_rotate);
                        break;
                    case 'g':
                        Instantiate(ghosthouse_top, loc, no_rotate);
                        break;
                    case 'h':
                        Instantiate(ghosthouse_bottom, loc, no_rotate);
                        break;
                    case 'l':
                        Instantiate(small_left, loc, no_rotate);
                        break;
                    case 'r':
                        Instantiate(small_right, loc, no_rotate);
                        break;
                    case 't':
                        Instantiate(small_top, loc, no_rotate);
                        break;
                    case 'b':
                        Instantiate(small_bottom, loc, no_rotate);
                        break;
                    case 'q':
                        Instantiate(small_top_left_corner, loc, no_rotate);
                        break;
                    case 'p':
                        Instantiate(small_top_right_corner, loc, no_rotate);
                        break;
                    case 'm':
                        Instantiate(small_bottom_right_corner, loc, no_rotate);
                        break;
                    case 'z':
                        Instantiate(small_bottom_left_corner, loc, no_rotate);
                        break;
                    case '1':
                        Instantiate(big_top_right_corner, loc, no_rotate);
                        break;
                    case '2':
                        Instantiate(big_top_left_corner, loc, no_rotate);
                        break;
                    case '3':
                        Instantiate(big_bottom_left_corner, loc, no_rotate);
                        break;
                    case '4':
                        Instantiate(big_bottom_right_corner, loc, no_rotate);
                        break;
                    case '5':
                        Instantiate(straight_vertical_double_bottom_left_corner, loc, no_rotate);
                        break;
                    case '6':
                        Instantiate(straight_vertical_double_top_left_corner, loc, no_rotate);
                        break;
                    case '7':
                        Instantiate(straight_vertical_double_bottom_right_corner, loc, no_rotate);
                        break;
                    case '8':
                        Instantiate(straight_vertical_double_top_right_corner, loc, no_rotate);
                        break;
                    case '9':
                        Instantiate(ghosthouse_left_door, loc, no_rotate);
                        break;
                    case '0':
                        Instantiate(ghosthouse_right_door, loc, no_rotate);
                        break;
                    case 'd':
                        Instantiate(ghosthouse_door, loc, no_rotate);
                        break;


                }
            }
        }

    }
}
