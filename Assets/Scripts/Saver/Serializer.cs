using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Newtonsoft;
using Newtonsoft.Json;

public static class Serializer
{
    public static int WorldSeed;

    private static string path = Application.persistentDataPath + "/";

    public static void SetWorldName(string worldName)
    {
        path += worldName + ".ProjectT";
    }

    public static void SaveWorld(TerrainMeshGenerator[,] chunkMesh)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        
        FileStream stream = new FileStream(path, FileMode.Create);

        TerrainChankData[,] chunkData = new TerrainChankData[chunkMesh.GetLength(0), chunkMesh.GetLength(1)];
        for(int i = 0; i < chunkMesh.GetLength(0); i++)
        {
            for(int j = 0; j < chunkMesh.GetLength(1); j++)
            {
                if (chunkMesh[i,j] != null)
                    chunkData[i, j] = chunkMesh[i,j].saveData;
            }
        }

        formatter.Serialize(stream, chunkData);
        stream.Close();

        Debug.Log("Save succes!");
    } 


    public static TerrainChankData[,] LoadWorld() 
    {
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream (path, FileMode.Open);

            TerrainChankData[,] chunkData = formatter.Deserialize(stream) as TerrainChankData[,];

            stream.Close();
            Debug.Log("Load succes!");

            return chunkData;

            TerrainMeshGenerator[,] terrainMeshGenerators = new TerrainMeshGenerator[chunkData.GetLength(0), chunkData.GetLength(1)];
            for (int i = 0; i < chunkData.GetLength(0); i++)
            {
                for (int j = 0; j < chunkData.GetLength(1); j++)
                {
                    if (chunkData[i, j] != null)
                    {
                        terrainMeshGenerators[i, j] = new TerrainMeshGenerator();
                        terrainMeshGenerators[i, j].saveData = chunkData[i, j];
                    }
                }
            }
            //return terrainMeshGenerators;
        }
        else
        {
            Debug.LogWarning("File path is not found!");
            return null;
        }
    }
}
