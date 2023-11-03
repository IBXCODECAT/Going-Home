using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

/// <summary>
/// Assigns a boolean to each side of a hex face
/// </summary>
public struct HexMeshFacesBooleans
{
    internal bool top;
    internal bool bottom;
    internal HexMeshSides sides;

    public HexMeshFacesBooleans(bool top, bool bottom, bool side0, bool side1, bool side2, bool side3, bool side4, bool side5)
    {
        this.top = top;
        this.bottom = bottom;

        this.sides = new HexMeshSides(side0, side1, side2, side3, side4, side5);
    }

    public HexMeshFacesBooleans(bool top, bool bottom, HexMeshSides sides)
    {
        this.top = top;
        this.bottom = bottom;
        this.sides = sides;
    }
}

/// <summary>
/// Represents the sides of a hex
/// </summary>
public struct HexMeshSides
{

    internal bool side0;
    internal bool side1;
    internal bool side2;
    internal bool side3;
    internal bool side4;
    internal bool side5;

    public HexMeshSides(bool side0, bool side1, bool side2, bool side3, bool side4, bool side5)
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

public class HexoidInstance
{
    public bool CAN_BURN { get; private set; } = false; //This hexoid can be burnt, turning it into air or ash
    public bool COLLISION { get; private set; } = true; //This hexoid should have collision    
    public bool COLLECTABLE_WITH_AXE { get; private set; } = false; //Collectable with axe
    public bool COLLECTABLE_WITH_PICKAXE { get; private set; } = false; //Collectable with pickaxe
    public bool COLLECTABLE_WITH_SHOVEL { get; private set; } = false; //Collectable with Shovel
    public bool COLLECTABLE_WITH_BRUSH { get; private set; } = false; //Collectable with Brush
    public bool CONVERTABLE_TO_MUD { get; private set; } = false; //Turns into mud when wet
    public bool HARVESTABLE { get; private set; } = false; //Can be harvested by both the player and machines
    public bool ELECTRICAL_SOURCE { get; private set; } = false; //Generates electricity
    public bool ELECTRICAL_CONDUCTOR_GOLD { get; private set; } = false; //Conducts electricity (hard powered)
    public bool ELECTRICAL_CONDUCTOR_COPPER { get; private set; } = false; //COnducst electricity (soft powered)
    public bool ELECTRICAL_HAZARD { get; private set; } = false; //Can cause electricution

    public enum HexType
    {
        AIR,
        DIRT,
        SOIL,
        STONE,
        
        COAL_DEPOSIT,
        IRON_DEPOSIT,
        COPPER_DEPOSIT,
        GOLD_DEPOSIT,

        DIAMOND_DEPOSIT

    }
};



public enum HEX_Type
{
    AIR,
    DIRT,
    SOIL,

}

public enum LiquidType
{
    GAS_SOURCE,
    GAS_FLOWING,
    OIL_SOURCE,
    OIL_FLOWING,
    WATER_SOURCE,
    WATER_FLOWING,
}