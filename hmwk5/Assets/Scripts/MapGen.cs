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
    public List<char[]> map = new List<char[]>();
    int size_x;
    int size_y;

    public float tileSize = 1.0f;
    public Texture2D terrainTiles;
    public int tileResolution;

    public Material line_mat;

    Texture2D mapText;

    // Use this for initialization
    void Start()
    {
        ParseFile();
        BuildMesh();
        BuildTexture();
        CreateOverlay();
    }

    public void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitinfo;
        if (GetComponent<Collider>().Raycast(ray, out hitinfo, Mathf.Infinity) && Input.GetMouseButtonDown(1))
        {
            //get the input of the mouse

            // this will be the color of the tree
            Color[] c = terrainTiles.GetPixels(tileResolution, 0, tileResolution, tileResolution);
            int x = Mathf.FloorToInt(hitinfo.point.x / tileSize);
            int y = Mathf.FloorToInt(hitinfo.point.y / tileSize);

            mapText.SetPixels(x * tileResolution, y * tileResolution, tileResolution, tileResolution, c);
            mapText.Apply();
            MeshRenderer mesh_renderer = GetComponent<MeshRenderer>();
            mesh_renderer.sharedMaterials[0].mainTexture = mapText;

            map[map.Count - 1 - y][x] = 'T';

            FindObjectOfType<AlgorithmScript>().ConvertPixelMaptoTiles();
        }
    }

    void ParseFile()
    {
        string file_path = "map.txt";
        StreamReader input = new StreamReader(file_path);

        while (!input.EndOfStream)
        {
            string line = input.ReadLine();
            char[] char_line = line.ToCharArray();
            map.Add(char_line);
            size_x = line.Length;
        }

        input.Close();

        size_y = map.Count;
    }

    void BuildTexture()
    {
        int textWidth = size_x * tileResolution;
        int textHeight = size_y * tileResolution;
        Texture2D texture = new Texture2D(textWidth, textHeight);

        for (int y = 0; y < size_y; y++) {
            for(int x = 0; x < size_x; x++)
            {
                char tile = map[size_y - 1 - y][x];

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

        mapText = texture;
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
        mesh_collider.sharedMesh = mesh;
    }

    void CreateOverlay()
    {
        GameObject parent = new GameObject();
        parent.name = "GridWorld";

        for(int x = 0; x < size_x+1; x++)
        {
            if (x % 3 != 0)
                continue;

            GameObject path_line = new GameObject();
            path_line.transform.position = new Vector3(x * tileSize, 0, -2);
            path_line.AddComponent<LineRenderer>();

            LineRenderer lr = path_line.GetComponent<LineRenderer>();
            lr.material = line_mat;
            lr.startColor = Color.green;
            lr.endColor = Color.green;

            lr.startWidth = .2f;
            lr.endWidth = .2f;

            Vector3 pos = new Vector3(x * tileSize, size_y * tileSize, -2);
            lr.SetPosition(0, pos);
            lr.SetPosition(1, new Vector3(x * tileSize, 0, -2));
            path_line.transform.parent = parent.transform;
        }

        for (int y = 0; y < size_y + 1; y++)
        {
            if (y % 3 != 0)
                continue;

            GameObject path_line = new GameObject();
            path_line.transform.position = new Vector3(0, y * size_y, -2);
            path_line.AddComponent<LineRenderer>();

            LineRenderer lr = path_line.GetComponent<LineRenderer>();
            lr.material = line_mat;
            lr.startColor = Color.green;
            lr.endColor = Color.green;

            lr.startWidth = .2f;
            lr.endWidth = .2f;

            Vector3 pos = new Vector3(size_x * tileSize, y * tileSize, -2);
            lr.SetPosition(0, pos);
            lr.SetPosition(1, new Vector3( 0, y * tileSize, -2));
            path_line.transform.parent = parent.transform;
        }

        for (int y = 0; y < size_y+1; y++)
        {
            for (int x = 0; x < size_x+1; x++)
            {   
                if ((x-1) % 3 == 0 && (y-1) % 3 == 0)
                {
                    GameObject dot = Instantiate(center_dot, new Vector3(x*tileSize + tileSize/2,y*tileSize+tileSize/2, -2), new Quaternion(0, 0, 0, 0));
                    dot.transform.localScale = new Vector3(1, 1, 1);
                    dot.transform.parent = parent.transform;
                }
            }
        }
    }
}