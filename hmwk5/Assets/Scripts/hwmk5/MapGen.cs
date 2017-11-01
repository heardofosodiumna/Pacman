using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MapGen : MonoBehaviour
{

    /*public GameObject tree;
    public GameObject openSpace;
    public GameObject wall;*/
    public GameObject center_dot;
    Ray ray;
    RaycastHit hit;
    public ArrayList map = new ArrayList();
    int size_x;
    int size_y;

    float tileSize = 1.0f;
    public Texture2D terrainTiles;
    public int tileResolution;

    // Use this for initialization
    void Start()
    {
        ParseFile();
        BuildMesh();
        BuildTexture();
    }

    void ParseFile()
    {
        string file_path = "map.txt";
        StreamReader input = new StreamReader(file_path);

        while (!input.EndOfStream)
        {
            string line = input.ReadLine();
            map.Add(line.ToCharArray());
            size_x = line.Length;
        }

        input.Close();

        size_y = map.Count;
        Debug.Log(size_x);
        Debug.Log(size_y);
    }

    void BuildTexture()
    {
        int textWidth = size_x * tileResolution;
        int textHeight = size_y * tileResolution;
        Texture2D texture = new Texture2D(textWidth, textHeight);

        for (int y = 0; y < size_y; y++) {
            for(int x = 0; x < size_x; x++)
            {
                char[] line = (char[])map[size_y - 1 - y];
                char tile = line[x];

                int texture_index = 0;
                switch(tile)
                {
                    case '@':
                        texture_index = 4;
                        break;
                    case 'T':
                        texture_index = 1;
                        break;
                    case '.':
                        texture_index = 2;
                        break;
                }

                Color[] c = terrainTiles.GetPixels(texture_index*tileResolution, 0, tileResolution, tileResolution);
                texture.SetPixels(x * tileResolution, y * tileResolution, tileResolution, tileResolution, c);
            }
        }
        

        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();

        MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
        mesh_renderer.sharedMaterials[0].mainTexture = texture;
    }


    void BuildMesh()
    {
        int numTiles = size_x * size_y;
        int numTris = numTiles * 2;

        int vsize_x = size_x + 1;
        int vsize_y = size_y + 1;
        int numVerts = vsize_x * vsize_y;

        Vector3[] vertices = new Vector3[numVerts];
        Vector3[] normals = new Vector3[numVerts];
        Vector2[] uv = new Vector2[numVerts];

        int[] triangles = new int[numTris * 3];

        int x, y;
        for (y = 0; y < vsize_y; y++)
        {
            for (x = 0; x < vsize_x; x++)
            {
                vertices[y * vsize_x + x] = new Vector3(x * tileSize, y * tileSize, 0);
                normals[y * vsize_x + x] = Vector3.up;
                uv[y * vsize_x + x] = new Vector2((float)x / size_x, (float)y / size_y);
            }
        }

        for (y = 0; y < size_y; y++)
        {
            for (x = 0; x < size_x; x++)
            {
                int squareIndex = y * size_x + x;
                int triOffset = squareIndex * 6;

                triangles[triOffset + 0] = y * vsize_x + x + 0;
                triangles[triOffset + 1] = y * vsize_x + x + vsize_x + 0;
                triangles[triOffset + 2] = y * vsize_x + x + vsize_x + 1;

                triangles[triOffset + 3] = y * vsize_x + x + 0;
                triangles[triOffset + 4] = y * vsize_x + x + vsize_x + 1;
                triangles[triOffset + 5] = y * vsize_x + x + 1;
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uv;

        MeshFilter mesh_filter = GetComponent<MeshFilter>();
        MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
        MeshCollider mesh_collider = GetComponent<MeshCollider>();

        mesh_filter.mesh = mesh;
    }

}