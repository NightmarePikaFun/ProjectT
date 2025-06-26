using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class TerrainChankData
{
    public SaveVector3[] vertices;
    public int[] triangles;
    public SaveVector2[] uv;
    public float seed;
    public SaveVector2Int perlinNoiseStartCoord;
    public SaveVector2Int chunkCoord;
    public SaveVector3[] normals;

    public TerrainChankData()
    {
        perlinNoiseStartCoord = new SaveVector2Int();
        chunkCoord = new SaveVector2Int();
    }

    public void AddMesh(Mesh mesh)
    {
        vertices = new SaveVector3[mesh.vertices.Length];
        for(int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new SaveVector3();
            vertices[i].Convert(mesh.vertices[i]);
        }
        triangles = mesh.triangles;
        uv = new SaveVector2[mesh.uv.Length];
        for (int i = 0; i < uv.Length; i++)
        {
            uv[i] = new SaveVector2();
            uv[i].Convert(mesh.uv[i]);
        }
        normals = new SaveVector3[mesh.normals.Length];
        for(int i = 0; i < normals.Length; i++)
        {
            normals[i] = new SaveVector3();
            normals[i].Convert(mesh.normals[i]);
        }
    }

    public Vector3[] GetVertices()
    {
        Vector3[] retVertices = new Vector3[vertices.Length];
        for(int i = 0; i < retVertices.Length;i++)
        {
            retVertices[i] = vertices[i].Revert();
        }
        return retVertices;
    }

    public int[] GetTriangles()
    {
        return triangles;
    }

    public Vector2[] GetUv()
    {
        Vector2[] retUv = new Vector2[uv.Length];
        for(int i = 0; i < retUv.Length;i++)
        {
            retUv[i] = uv[i].Revert();
        }
        return retUv;
    }

    public Vector3[] GetNormals()
    {
        Vector3[] retNormals = new Vector3[normals.Length];
        for (int i = 0; i < retNormals.Length; i++)
        {
            retNormals[i] = normals[i].Revert();
        }
        return retNormals;
    }
}

[Serializable]
public class SaveVector2Int
{
    int x;
    int y;

    public void Convert(Vector2Int value)
    {
        x = value.x; 
        y = value.y;
    }

    public Vector2Int Revert()
    {
        return new Vector2Int(x, y);
    }
}

[Serializable]
public class SaveVector2
{
    float x;
    float y;

    public void Convert(Vector2 value)
    {
        x = value.x;
        y = value.y;
    }

    public Vector2 Revert()
    {
        return new Vector2(x, y);
    }
}

[Serializable]
public class SaveVector3
{
    float x;
    float y;
    float z;

    public void Convert(Vector3 value)
    {
        x = value.x;
        y = value.y;
        z = value.z;
    }

    public Vector3 Revert()
    {
        return new Vector3(x, y, z);
    }
}
