using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ResourcesGenerator : MonoBehaviour
{
    [SerializeField]
    private List<ResourcesBiome> resourcesPrefab; // TODO mb struct
    [SerializeField]
    public Biome biomeType;
    [SerializeField]
    public int spawnRange = 3;
    [SerializeField]
    public int chankSize = 8;

    TerrainMeshGenerator meshGen;

    private void Awake()
    {
        meshGen = GetComponent<TerrainMeshGenerator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("InitResources")]
    public void InitResources()
    {
        List<GameObject> spawnGO = new List<GameObject>();
        foreach(var item in resourcesPrefab)
        {
            if(item.BiomeEqual(biomeType))
            {
                spawnGO.Add(item.resourcePrefab);
            }
        }
        List<Vector3> occupedPos = new List<Vector3>();
        //Spawn
        foreach(var item in spawnGO)
        {
            int weight = UnityEngine.Random.Range(1, 6); //TODO remake it
            Vector2Int startCoord = new Vector2Int((int)transform.position.x, (int)transform.position.z);
            
            int x = UnityEngine.Random.Range(0, chankSize-1),
                    z = UnityEngine.Random.Range(0, chankSize-1);
            //occuped check
            Vector3Int coord = new Vector3Int(x + startCoord.x, 0, z + startCoord.y);//Zero is temporary. Need change to mesh point height. X and Z also need get in Mesh
            SpawnObject(item, coord, ref occupedPos);
            weight -= 1;

            while (weight > 0)
            {
                SpawnObject(item, coord, ref occupedPos,spawnRange);
                weight--;
            }
        }
    }

    private void SpawnObject(GameObject spawnObject, Vector3Int startPosition, ref List<Vector3> occupedPos, int spread = 0)
    {
        if (spread != 0)
            startPosition += new Vector3Int(UnityEngine.Random.Range(-spread, spread), 0, UnityEngine.Random.Range(-spread, spread));
        //occuped check
        int x = startPosition.x - (int)transform.position.x,
            z = startPosition.z - (int)transform.position.z;
        float y;
        if (x < 0 || z < 0 || x > 20 || z > 20)
            return;
        GameObject newObject = Instantiate(spawnObject);
        newObject.GetComponent<TMP_Block_Script>().pos = new Vector2Int(x, z);
        Vector3 spawnPos = startPosition - new Vector3(-0.5f, 0, -0.5f);
        y = meshGen.GetVerticesHeight(new Vector2Int(x, z));
        spawnPos.y = y + 0.5f;
        newObject.transform.position = spawnPos;
        occupedPos.Add(newObject.transform.position);
    }
}

[Serializable]
public struct ResourcesBiome
{
    public GameObject resourcePrefab;
    public List<Biome> bioms;

    public bool BiomeEqual(Biome biome)
    {
        return bioms.Contains(biome);
    }
}

public enum Biome
{
    Forest,
    Dessert,
    Hills,
    Water
}
