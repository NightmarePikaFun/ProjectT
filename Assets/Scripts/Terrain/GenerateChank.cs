using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GenerateChank : MonoBehaviour
{
    //TMP
    private GameObject Player;

    [SerializeField]
    private string WorldName;

    [SerializeField]
    private int chunkSize = 20;
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
    [SerializeField, Range(1,10)]
    public int playerLoadRadius = 3;
    [SerializeField]
    private Vector2Int playerChunk;

    private List<TerrainMeshGenerator> terrrainMeshes;
    private List<float> randomizedValue;
    private System.Random randomGenerator;

    private TerrainMeshGenerator[,] chunkMesh;
    private int[,] chunkHeight;

    public static GenerateChank Instance;
    private List<ChunkData> activeChunk;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
        chunkHeight = new int[chunkNumber, chunkNumber];
        chunkMesh = new TerrainMeshGenerator[chunkNumber,chunkNumber];
        terrrainMeshes = new List<TerrainMeshGenerator>();
        randomGenerator = new System.Random(seed);
        randomizedValue = new List<float>();
        activeChunk = new List<ChunkData>();

        //TMP height set
        

        Serializer.SetWorldName(WorldName);
        Serializer.WorldSeed = seed;

        StartCoroutine(ChunkUnloader());

        Player = GameObject.FindGameObjectWithTag("Player");
    }

    [ContextMenu("Generate world")]
    private void Generate()
    {
        chunkHeight[chunkNumber / 2 + 8, chunkNumber / 2 + 8] = 3;
        chunkHeight[chunkNumber / 2 + 7, chunkNumber / 2 + 8] = 2;
        chunkHeight[chunkNumber / 2 + 7, chunkNumber / 2 + 8] = 2;
        chunkHeight[chunkNumber / 2 + 8, chunkNumber / 2 + 7] = 2;
        chunkHeight[chunkNumber / 2 + 8, chunkNumber / 2 + 6] = 1;
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
            for(int i = playerChunk.y-playerLoadRadius; i <= playerChunk.y+playerLoadRadius;i++)
            {
                for(int j = playerChunk.x-playerLoadRadius; j <= playerChunk.x+playerLoadRadius;j++)
                {
                    if (chunkMesh[i,j] != null)
                    {
                        chunkMesh[i, j].isActive = true;
                        chunkMesh[i, j].gameObject.SetActive(true);
                    }
                    else
                    {
                        CreateChunk((ushort)i, (ushort)j);
                    }
                }
            }
            /*createChunk = false;
            GameObject newTerrain = Instantiate(terrainPrefab);
            terrrainMeshes.Add(newTerrain.GetComponent<TerrainMeshGenerator>());
            randomizedValue.Add((float)randomGenerator.NextDouble());
            terrrainMeshes[terrrainMeshes.Count - 1].SetSeed(randomizedValue[randomizedValue.Count-1]);
            terrrainMeshes[terrrainMeshes.Count-1].CreateTerrain(newChank);
            chankList.Add(newChank);
            if(terrrainMeshes.Count>1)
                TerrainCompare(terrrainMeshes[terrrainMeshes.Count - 1]);*/
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
            activeChunk.Add(newTerrain.GetComponent<ChunkData>());
            newTerrain.GetComponent<ChunkData>().Load();
            newTerrain.GetComponent<ChunkData>().SetChunkNumber(new Vector2Int(x, y));
            chunkMesh[x, y] = newTerrain.GetComponent<TerrainMeshGenerator>();
            chunkMesh[x, y].InitTerrainMesh(chunkHeight[x, y]);
            chunkMesh[x,y].isActive = true;
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

    public void LoadChunk(ushort x, ushort y)
    {
        if (chunkMesh[x, y] == null)
        {
            CreateChunk(x, y);
        }
        else
        {
            if (!chunkMesh[x, y].isActive)
            {
                chunkMesh[x, y].isActive = true;
                chunkMesh[x, y].GetComponent<ChunkData>().Load();
                activeChunk.Add(chunkMesh[x,y].GetComponent<ChunkData>());
            }
            //Load chunk
        }
    }

    [ContextMenu("RandomizeSeed")]
    public void RandomizeSeed()
    {
        seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
    }

    private IEnumerator ChunkUnloader()
    {
        TimeSpan timeSpan = new TimeSpan();
        List<ChunkData> unloadChunk = new List<ChunkData>();
        Vector2Int chunkNumber;
        while (true)
        {
            for(int i = 0; i < activeChunk.Count; i++)
            {

                timeSpan = DateTime.Now.Subtract(activeChunk[i].GetStartTime());
                if(timeSpan.Minutes>2)
                {
                    if (Vector2.Distance(new Vector2(Player.transform.position.x, Player.transform.position.z),
                        new Vector2(activeChunk[i].transform.position.x, activeChunk[i].transform.position.z)) > 86)
                        unloadChunk.Add(activeChunk[i]);
                    else
                        activeChunk[i].UpdateTime();
                }
            }
            foreach(ChunkData chunk in unloadChunk)
            {
                activeChunk.Remove(chunk);
                chunkNumber = chunk.GetChunkNumder();
                chunkMesh[chunkNumber.x, chunkNumber.y].isActive = false;
                chunk.Unload();
            }
            unloadChunk.Clear();
            yield return new WaitForSecondsRealtime(60);
        }
    }

    [ContextMenu("Save world")]
    private void SaveWorld()
    {
        Serializer.SaveWorld(chunkMesh);
    }
    [ContextMenu("Load world")]
    private void LoadWorld()
    {
        TerrainChankData[,] saveChankData = Serializer.LoadWorld();
        if (saveChankData == null)
            return;
        for (int i = 0; i < chunkMesh.GetLength(0); i++)
        {
            for(int j = 0; j < chunkMesh.GetLength(1); j++)
            {
                if (saveChankData[i, j] != null)
                {
                    GameObject newTerrain = Instantiate(terrainPrefab);
                    activeChunk.Add(newTerrain.GetComponent<ChunkData>());
                    newTerrain.GetComponent<ChunkData>().Load();
                    newTerrain.GetComponent<ChunkData>().SetChunkNumber(new Vector2Int(i, j));
                    chunkMesh[i, j] = newTerrain.GetComponent<TerrainMeshGenerator>();
                    chunkMesh[i, j].InitTerrainMesh(chunkHeight[i, j]);
                    chunkMesh[i, j].isActive = true;
                    chunkMesh[i, j].SetSeed(saveChankData[i, j].seed);
                    chunkMesh[i, j].LoadTerrain(new Vector2Int(i,j), saveChankData[i,j]);
                }
            }
        }
    }
    
    public float GetHeight(Vector2Int point)
    {
        Vector2Int selectedChunkNumber = (point / chunkSize);
        if (selectedChunkNumber.x < 0 || selectedChunkNumber.y < 0 || selectedChunkNumber.x > chunkMesh.GetLength(0) || selectedChunkNumber.y > chunkNumber)
            return -1000;
        return chunkMesh[(int)selectedChunkNumber.x, (int)selectedChunkNumber.y].GetHeight(point - selectedChunkNumber * chunkSize);
    }

    public void UpHeight(Vector2Int point, Terraformin_Type terType, bool retro = false)
    {
        Vector2Int selectedChunkNumber = new Vector2Int(point.x / chunkSize, point.y/chunkSize);
        if (selectedChunkNumber.x < 0 || selectedChunkNumber.y < 0 || selectedChunkNumber.x > chunkMesh.GetLength(0) || selectedChunkNumber.y > chunkNumber)
            return;
        List<Side> outputSide = new List<Side>();
        switch(terType)
        {
            case Terraformin_Type.Up:
                outputSide = chunkMesh[selectedChunkNumber.x, selectedChunkNumber.y].UpHeight(point - selectedChunkNumber * chunkSize);
                break;
            case Terraformin_Type.Down:
                outputSide = chunkMesh[selectedChunkNumber.x, selectedChunkNumber.y].DownHeight(point - selectedChunkNumber * chunkSize);
                break;
            case Terraformin_Type.Middle:
                outputSide = chunkMesh[selectedChunkNumber.x, selectedChunkNumber.y].MiddleHeight(point - selectedChunkNumber * chunkSize);
                break;
        }
        if (!retro)
            return;
        List<Vector2Int> worldPoints = new List<Vector2Int>();
        List<int[]> chankPoints = new List<int[]>();
        //TODO: need to edit scaling border cells in chank. Calc border chank same may be wrong. Need create another algorythm
        foreach(Side side in outputSide)
        {
            switch (side)
            {
                case Side.Top:
                    worldPoints.Add(new Vector2Int(0, 1));
                    chankPoints.Add(new int[] { 2, 3 });

                    break;
                case Side.Bottom:
                    worldPoints.Add(new Vector2Int(0, -1));
                    chankPoints.Add(new int[] { 0, 1 });
                    break;
                case Side.Left:
                    worldPoints.Add(new Vector2Int(1, 0));
                    chankPoints.Add(new int[] { 1, 3 });
                    break;
                case Side.Right:
                    worldPoints.Add(new Vector2Int(-1, 0));
                    chankPoints.Add(new int[] { 0, 2 });
                    break;
            }
            UpHeight(point, terType);
        }
        if(outputSide.Count > 1)
        {
            //UpHeight(worldPoints[0] + worldPoints[1],)
            int[] tmp = chankPoints[0].Intersect(chankPoints[1]).ToArray();
            string tmp_s = "";
            for(int i = 0; i < tmp.Length; i++)
            {
                tmp_s+= tmp[i]+" ";
            }
            Debug.Log(tmp_s);
        }
    }
}

