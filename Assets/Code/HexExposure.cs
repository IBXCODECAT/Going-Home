using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Assigns a boolean to each side of a hex face
/// </summary>
public struct HexFaceBooleans
{
    internal bool top;
    internal bool bottom;
    internal HexSides sides;

    public HexFaceBooleans(bool top, bool bottom, bool side0, bool side1, bool side2, bool side3, bool side4, bool side5)
    {
        this.top = top;
        this.bottom = bottom;

        this.sides = new HexSides(side0, side1, side2, side3, side4, side5);
    }

    public HexFaceBooleans(bool top, bool bottom, HexSides sides)
    {
        this.top = top;
        this.bottom = bottom;
        this.sides = sides;
    }
}

/// <summary>
/// Represents the sides of a hex
/// </summary>
public struct HexSides
{

    internal bool side0;
    internal bool side1;
    internal bool side2;
    internal bool side3;
    internal bool side4;
    internal bool side5;

    public HexSides(bool side0, bool side1, bool side2, bool side3, bool side4, bool side5)
    {
        this.side0 = side0;
        this.side1 = side1;
        this.side2 = side2;
        this.side3 = side3;
        this.side4 = side4;
        this.side5 = side5;
    }
}

public struct HexMeshFace
{
    public List<Vector3> verticies { get; private set; }
    public List<int> triangles { get; private set; }

    public List<Vector2> uvs { get; private set; }

    public HexMeshFace(List<Vector3> verticies, List<int> triangles, List<Vector2> uvs)
    {
        this.verticies = verticies;
        this.triangles = triangles;
        this.uvs = uvs;
    }
}