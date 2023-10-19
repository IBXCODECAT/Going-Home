using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;

namespace Hexoidra.World
{
    internal class Block
    {
        public Vector3 position;

        private Dictionary<Faces, FaceData> faces;

        private List<Vector2> uv = new List<Vector2>()
        {
            new Vector2(0f, 1f), //top left uv
            new Vector2(1f, 1f), //top right uv
            new Vector2(1f, 0f), //bottom right uv
            new Vector2(0f, 0f), //bottom left uv
        };

        public Block(Vector3 position)
        {
            this.position = position;
            faces = new Dictionary<Faces, FaceData>()
            {
                {Faces.FRONT, new FaceData {
                    verticies = TransformVerticies(RawFaceData.rawVertexData[Faces.FRONT]),
                    uvs = this.uv
                }},
                {Faces.BACK, new FaceData {
                    verticies = TransformVerticies(RawFaceData.rawVertexData[Faces.BACK]),
                    uvs = this.uv
                }},
                {Faces.LEFT, new FaceData {
                    verticies = TransformVerticies(RawFaceData.rawVertexData[Faces.LEFT]),
                    uvs = this.uv
                }},
                {Faces.RIGHT, new FaceData {
                    verticies = TransformVerticies(RawFaceData.rawVertexData[Faces.RIGHT]),
                    uvs = this.uv
                }},
                {Faces.TOP, new FaceData {
                    verticies = TransformVerticies(RawFaceData.rawVertexData[Faces.TOP]),
                    uvs = this.uv
                }},
                {Faces.BOTTOM, new FaceData {
                    verticies = TransformVerticies(RawFaceData.rawVertexData[Faces.BOTTOM]),
                    uvs = this.uv
                }},

            };
        }

        public List<Vector3> TransformVerticies(List<Vector3> verticies)
        {
            List<Vector3> transformedVerticies = new List<Vector3>();

            foreach(Vector3 vertex in verticies)
            {
                transformedVerticies.Add(vertex + position);
            }

            return transformedVerticies;
        }

        public FaceData GetFace(Faces face)
        {
            return faces[face];
        }
    }
}
