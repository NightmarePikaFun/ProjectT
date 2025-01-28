using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGenerator : MonoBehaviour
{
    [SerializeField]
    private Material waterMaterial;
    [SerializeField]
    private int size = 60;

    public Mesh mesh;
    private MeshCollider meshColider;

    Vector3[] vertices;

    private void Awake()
    {
        mesh = new Mesh();
        meshColider = GetComponent<MeshCollider>();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material = waterMaterial;
        InitWaterMesh();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitWaterMesh()
    {
        int aSize = size * size;
        vertices = new Vector3[aSize];
        int[] triangles = new int[(size - 1) * (size - 1) * 6];
        Vector2[] uv = new Vector2[aSize];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                vertices[i * size + j] = new Vector3(j, 0, i);
                uv[i * size + j] = new Vector2(j, i);
            }
        }

        for (int tN = 0, y = 0; y < size - 1; y++)
        {
            for (int x = 0; x < size - 1; x++)
            {
                triangles[tN] = size * y + x;
                triangles[tN + 1] = size * y + size + x;
                triangles[tN + 2] = size * y + size + x + 1;
                triangles[tN + 3] = size * y + x;
                triangles[tN + 4] = size * y + size + x + 1;
                triangles[tN + 5] = size * y + x + 1;

                tN += 6;
            }
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();
        meshColider.sharedMesh = mesh;
    }
}
