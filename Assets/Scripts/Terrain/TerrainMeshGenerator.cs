using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;


public class TerrainMeshGenerator : MonoBehaviour
{
    //Ground scale it's point up not be <2
    [SerializeField]
    private Vector2Int groundScale;
    [SerializeField]
    private Material _inputMaterial;
    [SerializeField]
    private bool meshUp;
    [SerializeField]
    private bool meshDown;
    [SerializeField]
    private int quadNumber;

    private float seed;

    private float height = 0.5f;

    public int chunkHeight = 0;

    public Mesh mesh;
    Vector3[] vertices = new Vector3[4];


    //TODO tmp
    float perlinBaseAmplitude = 0.75f;
    float persistance = 2f;
    float lacunarity = 2f;
    int octaves = 4;

    public bool isActive = false;

    [SerializeField]
    Vector2Int perlinNoiseStartCoord = new Vector2Int(0,0);

    MeshCollider meshColider;

    private Vector2Int chunkCoord;

    [SerializeField]
    private GameObject blockTMP;
    [SerializeField]
    private bool generateBlock = false;

    public TerrainChankData saveData;


    // Start is called before the first frame update
    void Awake()
    {
        saveData = new TerrainChankData();
        groundScale += new Vector2Int(1, 1);
        meshColider = GetComponent<MeshCollider>();
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material = _inputMaterial;
        #region hide
        //InitMesh();
        /*vertices = new Vector3[9];
        int[] triangles = new int[24];
        Vector2[] uv = new Vector2[9];

        vertices[0] = Vector3.zero;
        vertices[1] = new Vector3(1, 0, 0);
        vertices[2] = new Vector3(1, height, 1);
        vertices[3] = new Vector3(0, 0, 1);
        vertices[4] = new Vector3(1, 0, 2);
        vertices[5] = new Vector3(0, 0, 2);
        vertices[6] = new Vector3(2, 0 ,0);
        vertices[7] = new Vector3(2, 0, 1);
        vertices[8] = new Vector3(2, 0, 2);
        // vertices[4] = vertices[2];
        //vertices[5] = ;

        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(1, 0);
        uv[2] = new Vector2(1, 1);
        uv[3] = new Vector2(0, 1);
        uv[4] = new Vector2(1, 2);
        uv[5] = new Vector2(0, 2);
        uv[6] = new Vector2(2, 0);
        uv[7] = new Vector2(2, 1);
        uv[8] = new Vector2(2, 2);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        triangles[3] = 0;
        triangles[4] = 2;
        triangles[5] = 3;
        triangles[6] = 3;
        triangles[7] = 2;
        triangles[8] = 4;
        triangles[9] = 3;
        triangles[10] = 4;
        triangles[11] = 5;
        triangles[12] = 1;
        triangles[13] = 6;
        triangles[14] = 7;
        triangles[15] = 1;
        triangles[16] = 7;
        triangles[17] = 2;
        triangles[18] = 2;
        triangles[19] = 7;
        triangles[20] = 8;
        triangles[21] = 2;
        triangles[22] = 8;
        triangles[23] = 4;

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();*/
        #endregion
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(meshUp)
        {
            ChangeMesh(1);
            meshUp = false;
        }
        if(meshDown)
        {
            ChangeMesh(-1);
            meshDown = false;
        }
    }

    public void InitTerrainMesh(int height)
    {
        chunkHeight = height;
    }

    public void SetSeed(float inSeed)
    {
        seed = inSeed;
        saveData.seed = seed;
    }

    public void CreateTerrain(Vector2Int coord)
    {
        chunkCoord = coord;
        transform.position = new Vector3(coord.x * 20, 0, coord.y * 20);
        perlinNoiseStartCoord = coord * 20;

        saveData.perlinNoiseStartCoord.Convert(perlinNoiseStartCoord);
        saveData.chunkCoord.Convert(chunkCoord);

        InitMesh();
    }

    public void LoadTerrain(Vector2Int coord, TerrainChankData chankData)
    {
        saveData = chankData;
        chunkCoord = coord;
        transform.position = new Vector3(coord.x*20, 0, coord.y*20);
        perlinNoiseStartCoord = coord * 20;

        vertices = saveData.GetVertices();
        mesh.vertices = saveData.GetVertices();
        mesh.triangles = saveData.GetTriangles();
        mesh.uv = saveData.GetUv();
        mesh.normals = saveData.GetNormals();
        //mesh.RecalculateNormals();
        meshColider.sharedMesh = mesh;
    }

    private void ChangeMesh(int modifaer)
    {
        vertices[(int)(quadNumber / groundScale.x) + quadNumber - 1].y += height * modifaer; //= new Vector3(0, height, 0);
        vertices[(int)(quadNumber / groundScale.x) + quadNumber].y += height * modifaer; //= new Vector3(1, height, 0);
        vertices[(int)(quadNumber / groundScale.x) + quadNumber - 1 + groundScale.x].y += height * modifaer; //= new Vector3(1, height, 1);
        vertices[(int)(quadNumber / groundScale.x) + quadNumber + groundScale.x].y += height * modifaer; //= new Vector3(0, height, 1);
        mesh.vertices = vertices;
        meshColider.sharedMesh = mesh;

        saveData.AddMesh(mesh);

    }

    private void InitMesh()
    {
        int aSize = groundScale.x * groundScale.y;
        vertices = new Vector3[aSize];
        int[] triangles = new int[(groundScale.x-1)*(groundScale.y-1)*6];
        Vector2[] uv = new Vector2[aSize];

        float amplitude = perlinBaseAmplitude;

        for (int i = 0; i < groundScale.y; i++)
        {   
            for(int j = 0; j < groundScale.x; j++)
            {

                float freq = 1;
                float noiseHeight = 0;
                amplitude = perlinBaseAmplitude;
                for(int k = 0; k < octaves; k++)
                {
                    float px = ((float)perlinNoiseStartCoord.y/4 + (int)(j*.25f))/ 20 * freq+ perlinNoiseStartCoord.y/4,
                    py = ((float)perlinNoiseStartCoord.x/4 + (int)(i*.25f)) / 20 * freq+ perlinNoiseStartCoord.x/4;

                    float perlinVal = Mathf.PerlinNoise(px+seed, py+seed) * 2 - 1;

                    noiseHeight += perlinVal*amplitude;
                    amplitude *= persistance;
                    freq *= lacunarity;
                }
                
                //float yCoord = Mathf.Clamp(noiseHeight, -10000, 10000);
                //float yCoord = Mathf.PerlinNoise((perlinNoiseStartCoord.y+(int)(j*0.25f)) * seed, (perlinNoiseStartCoord.x+ (int)(i *0.25f)) * seed)*1.5f;

                vertices[i * groundScale.x + j] = new Vector3(j, noiseHeight*0.1f+ chunkHeight, i);
                uv[i * groundScale.x + j] = new Vector2(j, i);
            }
        }
        //tN -> triangleNumber
        //For new normals
        GameObject parent = new GameObject("parent");

        parent.transform.parent = this.transform;
        for(int tN = 0, y = 0; y < groundScale.x - 1; y++)
        {
            for(int x = 0; x < groundScale.y - 1; x++)
            {
                if (generateBlock)
                {
                    GameObject newObject = Instantiate(blockTMP, parent.transform);
                    newObject.transform.position = new Vector3(x + 0.5f + transform.position.x, (vertices[groundScale.y * y + x].y + vertices[groundScale.y * y + groundScale.x + x].y +
                        vertices[groundScale.y * y + x + 1].y + vertices[groundScale.y * y + groundScale.x + x + 1].y) / 4, y + 0.5f + transform.position.z);
                    newObject.GetComponent<TMP_Block_Script>().pos = new Vector2Int(x,y);
                }
                triangles[tN] = groundScale.y * y + x;
                triangles[tN + 1] = groundScale.y * y + groundScale.x + x;
                triangles[tN + 2] = groundScale.y * y + groundScale.x + x + 1;
                triangles[tN + 3] = groundScale.y * y + x;
                triangles[tN + 4] = groundScale.y * y + groundScale.x + x + 1;
                triangles[tN + 5] = groundScale.y * y + x + 1;

                tN += 6;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();
        meshColider.sharedMesh = mesh;

        saveData.AddMesh(mesh);
    }

    public void RecalcChunkBorder(TerrainMeshGenerator parentMesh, Side parentBorderSide, Side borderSide)
    {
        float[] parentBorderValue = parentMesh.GetBorder(parentBorderSide);
        float[] borderValue = GetBorder(borderSide);
        //Debug.Log("DoRecalc");
        float midCoef;
        switch (borderSide)
        {
            //TODO переписать перерасчет краев
            case Side.Right:
                for(int i = 0; i < parentBorderValue.Length;i++)
                {
                    CalcPointHeight(parentMesh, groundScale.x * i + 20, groundScale.x * i, i, parentBorderValue, borderValue);
                }
                break;
            case Side.Left:
                for (int i = 0; i < parentBorderValue.Length; i++)
                {
                    CalcPointHeight(parentMesh, groundScale.x * i, groundScale.x * i + 20, i, parentBorderValue, borderValue);
                }
                break;
            case Side.Top:
                for (int i = 0; i < parentBorderValue.Length; i++)
                {
                    CalcPointHeight(parentMesh, groundScale.x * 20 + i, i, i, parentBorderValue, borderValue);
                }
                break;
            case Side.Bottom:
                for (int i = 0; i < parentBorderValue.Length; i++)
                {
                    CalcPointHeight(parentMesh, i, groundScale.x * 20 + i, i, parentBorderValue, borderValue);
                }
                break;
        }
        mesh.vertices = vertices;
        meshColider.sharedMesh = mesh;

        saveData.AddMesh(mesh);


        RecalcNormals(parentMesh.GetBorderNormals(parentBorderSide),borderSide, parentMesh);
    }

    private void CalcPointHeight(TerrainMeshGenerator parentMesh,int boredrCell, int parentCell, int index, float[] parentBorderValue, float[] borderValue)
    {
        float midCoef;
        midCoef = parentBorderValue[index] + borderValue[index];
        midCoef /= 2;
        if (index == 0 || index == parentBorderValue.Length - 1)
            ChangeVertices(boredrCell, parentBorderValue[index]);
        else
        {
            if (parentBorderValue[index] < borderValue[index])
            {
                ChangeVertices(boredrCell, parentBorderValue[index]);
                parentMesh.ChangeVertices(parentCell, parentBorderValue[index]);
            }
            else
            {
                ChangeVertices(boredrCell, borderValue[index]);
                parentMesh.ChangeVertices(parentCell, borderValue[index]);
            }
        }
    }

    public void ChangeVerticesToHalf(int verticesNumber, float height)
    {
        vertices[verticesNumber].y = (vertices[verticesNumber].y + height) / 2;
        mesh.vertices = vertices;
        meshColider.sharedMesh = mesh;

        saveData.AddMesh(mesh);

    }

    public void ChangeVertices(int verticesNumber, float height)
    {
        vertices[verticesNumber].y = height;
        //Debug.Log(mesh.normals[verticesNumber]);
        mesh.vertices = vertices;
        meshColider.sharedMesh = mesh;

        saveData.AddMesh(mesh);

    }

    public float GetVerticesHeight(Vector2Int pos)
    {
        int index = pos.y * groundScale.y + pos.x;
        float retHeight = vertices[index].y;
        retHeight += vertices[index + groundScale.x].y;
        retHeight += vertices[index + groundScale.x + 1].y;
        retHeight += vertices[index+ 1].y;
        retHeight = retHeight / 4;
        return retHeight;

    }

    public float[] GetBorder(Side side)
    {
        float[] border = new float[groundScale.x];
        switch(side)
        {
            case Side.Left:
                for(int i = 0; i < border.Length;i++)
                {
                    border[i] = vertices[groundScale.x * i].y;
                }
                break;
            case Side.Right:
                for (int i = 0; i < border.Length; i++)
                {
                    border[i] = vertices[groundScale.x * i + 20].y;
                }
                break;
            case Side.Top:
                for (int i = 0; i < border.Length; i++)
                {
                    border[i] = vertices[groundScale.x * 20 + i].y;
                }
                break;
            case Side.Bottom:
                for (int i = 0; i < border.Length; i++)
                {
                    border[i] = vertices[i].y;
                }
                break;
        }
        return border;
    }

    public Vector3[] GetBorderNormals(Side side)
    {
        Vector3[] border = new Vector3[groundScale.x];
        switch (side)
        {
            case Side.Left:
                for (int i = 0; i < border.Length; i++)
                {
                    border[i] = meshColider.sharedMesh.normals[groundScale.x * i+1];
                }
                break;
            case Side.Right:
                for (int i = 0; i < border.Length; i++)
                {
                    border[i] = meshColider.sharedMesh.normals[groundScale.x * i + 19];
                }
                break;
            case Side.Top:
                for (int i = 0; i < border.Length; i++)
                {
                    border[i] = meshColider.sharedMesh.normals[groundScale.x * 19 + i];
                }
                break;
            case Side.Bottom:
                for (int i = 0; i < border.Length; i++)
                {
                    border[i] = meshColider.sharedMesh.normals[i+20];
                }
                break;
        }
        return border;
    }

    public float GetHeight(Vector2Int point)
    {
        float returnHeight = 0;
        point.y *= groundScale.y;
        returnHeight += vertices[point.x + point.y].y;
        returnHeight += vertices[point.x + point.y + 1].y;
        returnHeight += vertices[point.x + point.y + groundScale.y].y;
        returnHeight += vertices[point.x + point.y + groundScale.y+1].y;
        returnHeight /= 4;
        return returnHeight;
    }

    //TODO is TMP need remake this function
    #region heightChange
    public List<Side> UpHeight(Vector2Int point, out float height)
    {
        float max = float.NegativeInfinity;
        float equalNumber = 0;
        for(int i = 0; i < 2; i++)
        {
            for(int j = 0 ; j < 2; j++)
            {
                if (max < vertices[point.x + (point.y + j) * groundScale.y + i].y)
                    max = vertices[point.x + (point.y + j) * groundScale.y + i].y;
                if(max == vertices[point.x + (point.y + j) * groundScale.y + i].y)
                    equalNumber++;
            }
        }
        height = max;
        for(int i = 0; i < 2; i++)
        {
            for(int j = 0 ; j < 2; j++)
            {
                if(equalNumber==4)
                    vertices[point.x + (point.y + j) * groundScale.y + i].y += 0.25f;
                else
                    vertices[point.x + (point.y + j) * groundScale.y + i].y = max;
            }
        }
        mesh.vertices = vertices;
        meshColider.sharedMesh = mesh;

        saveData.AddMesh(mesh);

        return CheckSide(point);
        /*vertices[point.x + point.y * groundScale.y].y += 1;
vertices[point.x + point.y * groundScale.y+1].y += 1;
vertices[point.x + (point.y+1) * groundScale.y].y += 1;
vertices[point.x + (point.y+1) * groundScale.y+1].y += 1;*/
    }

    public List<Side> DownHeight(Vector2Int point)
    {
        //TODO if border chunk change borde in another chunk
        float min = float.PositiveInfinity;
        float equalNumber = 0;
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                if (min > vertices[point.x + (point.y + j) * groundScale.y + i].y)
                    min = vertices[point.x + (point.y + j) * groundScale.y + i].y;
                if (min == vertices[point.x + (point.y + j) * groundScale.y + i].y)
                    equalNumber++;
            }
        }
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                if(equalNumber == 4)
                    vertices[point.x + (point.y + j) * groundScale.y + i].y -= 0.25f;
                else
                    vertices[point.x + (point.y + j) * groundScale.y + i].y = min;
            }
        }
        mesh.vertices = vertices;
        meshColider.sharedMesh = mesh;

        saveData.AddMesh(mesh);

        return CheckSide(point);
    }

    public List<Side> MiddleHeight(Vector2Int point)
    {
        float middle = 0;
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                    middle += vertices[point.x + (point.y + j) * groundScale.y + i].y;
            }
        }
        middle /= 4;
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                vertices[point.x + (point.y + j) * groundScale.y + i].y = middle;
            }
        }
        mesh.vertices = vertices;
        meshColider.sharedMesh = mesh;

        saveData.AddMesh(mesh);

        return CheckSide(point);
    }

    public void ChangeHeight(Vector2Int point, int[] quadNumber, float height)
    {
        int index = 0;
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                if(quadNumber.Contains(index++))
                    vertices[point.x + (point.y + j) * groundScale.y + i].y += 0.25f;
            }
        }
        mesh.vertices = vertices;
        meshColider.sharedMesh = mesh;

        saveData.AddMesh(mesh);
    }

    private List<Side> CheckSide(Vector2Int point)
    {
        List<Side> retSide = new List<Side>();
        if (point.x <= 0)
            retSide.Add(Side.Right);
        if (point.y <= 0)
            retSide.Add(Side.Bottom);
        if(point.x >= groundScale.x-2)
            retSide.Add(Side.Left);
        if (point.y >= groundScale.y - 2)
            retSide.Add(Side.Top);
        return retSide;
    }

    public void PointHeightUp(Vector2 point)
    {

    }
    public void PointHeightDown(Vector2 point)
    {

    }
    #endregion

    #region ContextMenuToZero
    [ContextMenu("ZeroLeft")]
    public void ZeroLeft()
    {
        for(int i = 0; i < groundScale.x;i++)
        {
            vertices[groundScale.x * i].y = 0;
        }
        mesh.vertices = vertices;
        meshColider.sharedMesh = mesh;

        saveData.AddMesh(mesh);

    }

    [ContextMenu("ZeroRight")]
    public void ZeroRight()
    {
        for (int i = 0; i < groundScale.x; i++)
        {
            vertices[groundScale.x * i + 20].y = 0;
        }
        mesh.vertices = vertices;
        meshColider.sharedMesh = mesh;

        saveData.AddMesh(mesh);
    }


    [ContextMenu("ZeroBottom")]
    public void ZeroBottom()
    {
        for (int i = 0; i < groundScale.x; i++)
        {
            vertices[i].y = 0;
        }
        mesh.vertices = vertices;
        meshColider.sharedMesh = mesh;

        saveData.AddMesh(mesh);
    }

    [ContextMenu("ZeroTop")]
    public void ZeroTop()
    {
        for (int i = 0; i < groundScale.x; i++)
        {
            vertices[groundScale.x * 20 + i].y = 0;
        }
        mesh.vertices = vertices;
        meshColider.sharedMesh = mesh;

        saveData.AddMesh(mesh);
    }
    #endregion

    [ContextMenu("RecalcNormals")]
    public void RecalcNormals(Vector3[] additionalParentNormals, Side changeSide, TerrainMeshGenerator meshGen)
    {
        Vector3[] additionalNormals = GetBorderNormals(changeSide);
        Vector3[] smNormals = meshColider.sharedMesh.normals;
        Vector3[] parentSmNormals = meshGen.meshColider.sharedMesh.normals;
        for(int i = 1; i < groundScale.x-1;i++)
        {
            switch (changeSide)
            {
                case Side.Bottom:
                    smNormals[i] = (smNormals[i - 1] + smNormals[i + 1] + additionalParentNormals[i] + additionalNormals[i]) / 4;
                    parentSmNormals[groundScale.x * 20 + i] = smNormals[i];
                    break;
                case Side.Top:
                    smNormals[groundScale.x * 20 + i] = (smNormals[groundScale.x + 20*i - 1] + smNormals[groundScale.x + 20*i + 1] + additionalNormals[i] + parentSmNormals[i])/4;
                    parentSmNormals[i] = smNormals[groundScale.x * 20 + i];
                    break;
                case Side.Right:
                    smNormals[groundScale.x * i + 20] = (smNormals[groundScale.x * (i + 1) + 20] + smNormals[groundScale.x * (i - 1) + 20] 
                        + additionalParentNormals[i] + additionalNormals[i]) / 4;
                    parentSmNormals[groundScale.x * i] = smNormals[groundScale.x * i + 20];
                    break;
                case Side.Left:
                    smNormals[groundScale.x * i] = (smNormals[groundScale.x * (i-1)]+ smNormals[groundScale.x * (i + 1)] + additionalNormals[i] + additionalNormals[i]) / 4;
                    parentSmNormals[groundScale.x * i + 20] = smNormals[groundScale.x * i];
                    break;
            }
        }
        //TODO change corner points
        meshColider.sharedMesh.normals = smNormals;
        meshGen.meshColider.sharedMesh.normals = parentSmNormals;

    }

    //TODO add coroutine for check player not in range;


}

public enum Side
{
    Left,
    Right,
    Top,
    Bottom,
    None
}
