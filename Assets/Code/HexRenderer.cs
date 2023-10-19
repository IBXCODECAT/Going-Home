using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class HexRenderer : MonoBehaviour
{
    private Mesh m_mesh;
    private MeshFilter m_meshFilter;
    private MeshRenderer m_meshRenderer;

    private List<HexMeshFace> m_faces;

    [SerializeField] private Material material;

    [SerializeField] internal float innerSize;
    [SerializeField] internal float outerSize;
    [SerializeField] internal float height;

    [Tooltip("Make top flat perpendicular to the positive Z forward axis")]
    [SerializeField] internal bool useFlatTop;

    private void Awake()
    {
        m_meshFilter= GetComponent<MeshFilter>();
        m_meshRenderer = GetComponent<MeshRenderer>();

        m_mesh = new Mesh();
        m_mesh.name = "hex";

        m_meshFilter.mesh = m_mesh;
        m_meshRenderer.material = material;
    }

    internal void SetMaterial(Material material)
    {
        this.material = material;
    }

    internal void DrawMesh(HexFaceBooleans hexExposure)
    {
        DrawFaces(hexExposure);
        CombineFaces();
    }

    private void DrawFaces(HexFaceBooleans exposure)
    {
        m_faces = new List<HexMeshFace>();

        bool[] renderHexSides = {
            exposure.sides.side0, 
            exposure.sides.side1, 
            exposure.sides.side2, 
            exposure.sides.side3,
            exposure.sides.side4,
            exposure.sides.side5
        };


        if (exposure.top)
        {
            //Top Faces
            for (int point = 0; point < 6; point++)
            {
                m_faces.Add(CreateFace(innerSize, outerSize, height / 2f, height / 2f, point));
            }
        }

        if(exposure.bottom)
        {
            //Bottom Faces
            for (int point = 0; point < 6; point++)
            {
                m_faces.Add(CreateFace(innerSize, outerSize, -height / 2f, -height / 2f, point, true));
            }
        }

        //Outer Sides
       
        for (int point = 0; point < 6; point++)
        {
            bool renderThisSide = renderHexSides[point];

            if(renderThisSide)
            {
                m_faces.Add(CreateFace(outerSize, outerSize, height / 2f, -height / 2f, point, true));
            }
        }

        //Only draw inner sides if radius is greater than zero
        if(innerSize >= 0)
        {
            for(int point = 0; point < 6; point++)
            {
                m_faces.Add(CreateFace(innerSize, innerSize, height / 2f, -height / 2f, point, false));
            }
        }
    }

    private void CombineFaces()
    {
        List<Vector3> verticies = new List<Vector3>();
        List<int> tris = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        for(int i = 0; i < m_faces.Count; i++)
        {
            //Add the verticies
            verticies.AddRange(m_faces[i].verticies);
            uvs.AddRange(m_faces[i].uvs);

            //Offset the triangles
            int offset = (4 * i);
            foreach(int triangle in m_faces[i].triangles)
            {
                tris.Add(triangle + offset);
            }
        }

        m_mesh.vertices = verticies.ToArray();
        m_mesh.triangles = tris.ToArray();
        m_mesh.uv = uvs.ToArray();
        m_mesh.RecalculateNormals();
    }

    private Vector3 GetPoint(float size, float height, int index)
    {
        //360 degrees / 6 sides = 60 degrees (subtract 30 for a flat top perpendicular to forward axis)
        float angle_deg = useFlatTop ? 60 * index : 60 * index - 30;

        //Convert the degrees to radians
        float angle_rad = Mathf.PI / 180f * angle_deg;

        //Math Magic :)
        return new Vector3((size * Mathf.Cos(angle_rad)), height, size * Mathf.Sin(angle_rad));
    }

    private HexMeshFace CreateFace(float innerRad, float outerRad, float heightA, float heightB, int point, bool reverse = false)
    {
        //Calculate the position of our points
        //We filter the positons of pointB and pointC so that our last face triangle connects properly to our first
        Vector3 pointA = GetPoint(innerRad, heightB, point);
        Vector3 pointB = GetPoint(innerRad, heightB, (point < 5) ? point + 1 : 0);
        Vector3 pointC = GetPoint(outerRad, heightA, (point < 5) ? point + 1 : 0);
        Vector3 pointD = GetPoint(outerRad, heightA, point);

        //Construct a list of points
        List<Vector3> verticies = new List<Vector3>() { pointA, pointB, pointC, pointD };
        if (reverse) verticies.Reverse();

        //Specify triangle verticies
        List<int> triangles = new List<int> { 0, 1, 2, 2, 3, 0 };

        //Specify the corners of the UV Map
        List<Vector2> uvs = new List<Vector2>() { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) };

        return new HexMeshFace(verticies, triangles, uvs);
    }
}
