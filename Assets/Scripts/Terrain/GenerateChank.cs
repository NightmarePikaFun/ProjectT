using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateChank : MonoBehaviour
{
    //TODO static seed to pseudo random (pseudo random is static)
    [SerializeField]
    private ushort chunkNumber = 64; 
    [SerializeField]
    private bool createChunk = false;
    [SerializeField]
    private Vector2Int newChank;
    [SerializeField]
    private List<Vector2Int> chankList;
    [SerializeField]
    private GameObject terrainPrefab;
    [SerializeField]
    private int seed;

    private List<TerrainMeshGenerator> terrrainMeshes;
    private List<float> randomizedValue;
    private System.Random randomGenerator;

    private TerrainMeshGenerator[,] chunkMesh;

    private void Awake()
    {
        chunkMesh = new TerrainMeshGenerator[chunkNumber,chunkNumber];
        terrrainMeshes = new List<TerrainMeshGenerator>();
        randomGenerator = new System.Random(seed);
        randomizedValue = new List<float>();

        //Create start chank
        CreateChunk((ushort)(chunkNumber / 2 - 1), (ushort)(chunkNumber / 2 - 1));
        CreateChunk((ushort)(chunkNumber / 2 - 1), (ushort)(chunkNumber / 2));
        CreateChunk((ushort)(chunkNumber / 2), (ushort)(chunkNumber / 2 - 1));
        CreateChunk((ushort)(chunkNumber / 2), (ushort)(chunkNumber / 2));
    }

    // Update is called once per frame
    void Update()
    {
        if(createChunk)
        {
            createChunk = false;
            GameObject newTerrain = Instantiate(terrainPrefab);
            terrrainMeshes.Add(newTerrain.GetComponent<TerrainMeshGenerator>());
            randomizedValue.Add((float)randomGenerator.NextDouble());
            terrrainMeshes[terrrainMeshes.Count - 1].SetSeed(randomizedValue[randomizedValue.Count-1]);
            terrrainMeshes[terrrainMeshes.Count-1].CreateTerrain(newChank);
            chankList.Add(newChank);
            if(terrrainMeshes.Count>1)
                TerrainCompare(terrrainMeshes[terrrainMeshes.Count - 1]);
        }
    }

    private void TerrainCompare(TerrainMeshGenerator terrainMesh)
    {
        //terrainMesh.RecalcChunkBorder(terrrainMeshes[0].GetBorder(Side.Left),Side.Right);
    }

    private void TerrainRecalc(TerrainMeshGenerator terrainMesh, int x, int y, Side changeSide, Side parentSide)
    {
        terrainMesh.RecalcChunkBorder(chunkMesh[x, y], parentSide, changeSide);
    }

    private void CreateChunk(ushort x, ushort y)
    {
        if (chunkMesh[x,y] == null)
        {
            GameObject newTerrain = Instantiate(terrainPrefab);
            chunkMesh[x, y] = newTerrain.GetComponent<TerrainMeshGenerator>();
            float randomizedChunkValue = (float)randomGenerator.NextDouble();
            chunkMesh[x, y].SetSeed(randomizedChunkValue);
            chunkMesh[x, y].CreateTerrain(new Vector2Int(x, y));
            if(x > 0)
            {
                if (chunkMesh[x - 1, y] != null)
                    TerrainRecalc(chunkMesh[x, y], x - 1, y, Side.Left, Side.Right);
                //check x-1
            }
            if (x < chunkMesh.Length-1)
            {
                if (chunkMesh[x + 1, y] != null)
                    TerrainRecalc(chunkMesh[x, y], x + 1, y, Side.Right, Side.Left);
            }
            //check x+1
            if (y > 0)
            {
                if (chunkMesh[x, y - 1] != null)    
                    TerrainRecalc(chunkMesh[x, y], x, y - 1, Side.Bottom, Side.Top);
            }
            if( y < chunkMesh.Length-1)
            {
                if (chunkMesh[x, y + 1] != null)
                    TerrainRecalc(chunkMesh[x, y], x, y + 1, Side.Top, Side.Bottom);
            }
        }
    }

    [ContextMenu("RandomizeSeed")]
    public void RandomizeSeed()
    {
        seed = Random.Range(int.MinValue, int.MaxValue);
    }
}

