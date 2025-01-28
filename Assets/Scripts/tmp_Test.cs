using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using VoronoiLib;
using VoronoiLib.Structures;

public class tmp_Test : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private int size = 64;
    [SerializeField]
    private int pointsCount = 20;
    [SerializeField]
    private int seed = 16;
    [SerializeField]
    private List<Vector2Int> dataPoints;

    // Start is called before the first frame update
    LinkedList<VEdge> edges;
    void Start()
    {
        System.Random rand = new System.Random(seed);
        for(int i = 0; i < pointsCount; i++)
        {
            dataPoints.Add(new Vector2Int(rand.Next(1,size-1),rand.Next(1,size - 1)));
        }
        List<FortuneSite> points = new List<FortuneSite>();
        foreach(var p in dataPoints)
        {
            points.Add( new FortuneSite(p.x, p.y));
        }
        /*edges = FortunesAlgorithm.Run(points, 0, 0, 64, 64);*/
        edges = FortunesAlgorithm.Run(points, 0, 0, size, size);
        CreateObject(edges);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateObject(LinkedList<VEdge> edges)
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;

        Gizmos.DrawCube(new Vector3(size / 2, 0, size / 2), new Vector3(size, 0, size));
        if (edges == null)
            return;
        foreach (VEdge e in edges)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(new Vector3((float)e.Start.X, 0, (float)e.Start.Y),
                new Vector3((float)e.End.X, 0, (float)e.End.Y));
        }
    }
}
