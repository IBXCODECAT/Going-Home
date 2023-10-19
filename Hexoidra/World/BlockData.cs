using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexoidra.World
{
    public enum Faces
    {
        FRONT,
        BACK,
        LEFT,
        RIGHT,
        TOP,
        BOTTOM,
    }

    public struct FaceData
    {
        public List<Vector3> verticies;
        public List<Vector2> uvs;
    }

    public struct RawFaceData
    {
        public static readonly Dictionary<Faces, List<Vector3>> rawVertexData = new Dictionary<Faces, List<Vector3>>()
        {
            { Faces.FRONT, new List<Vector3>()
            {
                new Vector3(-0.5f, 0.5f, -0.5f), // Top left vertex
                new Vector3(0.5f, 0.5f, -0.5f),  // Top right vertex
                new Vector3(0.5f, -0.5f, -0.5f), // Bottom right vertex
                new Vector3(-0.5f, -0.5f, -0.5f), // Bottom left vertex
            }},
            { Faces.BACK, new List<Vector3>()
            {
                new Vector3(-0.5f, 0.5f, 0.5f),  // Top left vertex
                new Vector3(0.5f, 0.5f, 0.5f),   // Top right vertex
                new Vector3(0.5f, -0.5f, 0.5f),  // Bottom right vertex
                new Vector3(-0.5f, -0.5f, 0.5f),  // Bottom left vertex
            }},
            { Faces.LEFT, new List<Vector3>()
            {
                new Vector3(-0.5f, 0.5f, 0.5f),  // Top left vertex
                new Vector3(-0.5f, 0.5f, -0.5f), // Top right vertex
                new Vector3(-0.5f, -0.5f, -0.5f), // Bottom right vertex
                new Vector3(-0.5f, -0.5f, 0.5f),  // Bottom left vertex
            }},
            { Faces.RIGHT, new List<Vector3>()
            {
                new Vector3(-0.5f, 0.5f, 0.5f),  // Top left vertex
                new Vector3(0.5f, 0.5f, 0.5f),   // Top right vertex
                new Vector3(0.5f, 0.5f, -0.5f),  // Bottom right vertex
                new Vector3(-0.5f, 0.5f, -0.5f),  // Bottom left vertex
            }},
            { Faces.TOP, new List<Vector3>()
            { 
                // Front face
                new Vector3(-0.5f, 0.5f, -0.5f), // Top left vertex
                new Vector3(0.5f, 0.5f, -0.5f),  // Top right vertex
                new Vector3(0.5f, -0.5f, -0.5f), // Bottom right vertex
                new Vector3(-0.5f, -0.5f, -0.5f), // Bottom left vertex
            }},
            { Faces.BOTTOM, new List<Vector3>()
            { 
                // Front face
                new Vector3(-0.5f, -0.5f, 0.5f),  // Top left vertex
                new Vector3(0.5f, -0.5f, 0.5f),   // Top right vertex
                new Vector3(0.5f, -0.5f, -0.5f),  // Bottom right vertex
                new Vector3(-0.5f, -0.5f, -0.5f),  // Bottom left vertex
            }},

        };
    }
}
