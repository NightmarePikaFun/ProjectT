using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoronoiDiagramm
{
    private Site[] sites;
    private Face[] faces;
    List<Vertex> vectors;
    List<HalfEdge> edges;
}

public class Site
{
    int index;
    Vertex point;
    Face face;
}

public class Face
{
    Site site;
    HalfEdge edge;
}

public class HalfEdge
{
    Vertex origin;
    Vertex destination;
    HalfEdge twin;
    Face incidentFace;
    HalfEdge prev;
    HalfEdge next;
}

public class Vertex
{
    Vector2 point;
}

//TODO priority queue

