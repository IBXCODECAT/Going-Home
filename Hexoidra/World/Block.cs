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
                    vertices = TransformVerticies(FaceDataRaw.rawVertexData[Faces.FRONT]),
                    uvs = uv
                }},
                {Faces.BACK, new FaceData {
                    vertices = TransformVerticies(FaceDataRaw.rawVertexData[Faces.BACK]),
                    uvs = uv
                }},
                {Faces.LEFT, new FaceData {
                    vertices = TransformVerticies(FaceDataRaw.rawVertexData[Faces.LEFT]),
                    uvs = uv
                }},
                {Faces.RIGHT, new FaceData {
                    vertices = TransformVerticies(FaceDataRaw.rawVertexData[Faces.RIGHT]),
                    uvs = uv
                }},
                {Faces.TOP, new FaceData {
                    vertices = TransformVerticies(FaceDataRaw.rawVertexData[Faces.TOP]),
                    uvs = uv
                }},
                {Faces.BOTTOM, new FaceData {
                    vertices = TransformVerticies(FaceDataRaw.rawVertexData[Faces.BOTTOM]),
                    uvs = uv
                }},

            };
        }

        public List<Vector3> TransformVerticies(List<Vector3> vertices)
        {
            List<Vector3> transformedVertices = new List<Vector3>();

            foreach (var vert in vertices)
            {
                transformedVertices.Add(vert + position);
            }

            return transformedVertices;
        }

        public FaceData GetFace(Faces face)
        {
            return faces[face];
        }
    }
}
