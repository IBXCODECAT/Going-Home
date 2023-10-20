using OpenTK.Mathematics;

namespace Hexoidra.World
{
    internal class Block
    {
        private Vector3 position;
        internal BlockType blockType;

        private Dictionary<Faces, FaceData> faces;

        internal Dictionary<Faces, List<Vector2>> blockUV = new Dictionary<Faces, List<Vector2>>()
        {
            { Faces.FRONT, new List<Vector2>() },
            { Faces.BACK, new List<Vector2>() },
            { Faces.LEFT, new List<Vector2>() },
            { Faces.RIGHT, new List<Vector2>() },
            { Faces.TOP, new List<Vector2>() },
            { Faces.BOTTOM, new List<Vector2>() },
        };

        public Block(Vector3 position, BlockType blockType = BlockType.AIR)
        {
            this.position = position;
            this.blockType = blockType;

            
            if (blockType != BlockType.AIR)
            {
                blockUV = GetUvsFromAtlasCoordinates(TextureData.blockTypeUVCoordinates[blockType]);
            }
            
            faces = new Dictionary<Faces, FaceData>()
            {
                {Faces.FRONT, new FaceData {
                    vertices = TransformVerticies(FaceDataRaw.rawVertexData[Faces.FRONT]),
                    uvs = blockUV[Faces.FRONT]
                }},
                {Faces.BACK, new FaceData {
                    vertices = TransformVerticies(FaceDataRaw.rawVertexData[Faces.BACK]),
                    uvs = blockUV[Faces.BACK]
                }},
                {Faces.LEFT, new FaceData {
                    vertices = TransformVerticies(FaceDataRaw.rawVertexData[Faces.LEFT]),
                    uvs = blockUV[Faces.LEFT]
                }},
                {Faces.RIGHT, new FaceData {
                    vertices = TransformVerticies(FaceDataRaw.rawVertexData[Faces.RIGHT]),
                    uvs = blockUV[Faces.RIGHT]
                }},
                {Faces.TOP, new FaceData {
                    vertices = TransformVerticies(FaceDataRaw.rawVertexData[Faces.TOP]),
                    uvs = blockUV[Faces.TOP]
                }},
                {Faces.BOTTOM, new FaceData {
                    vertices = TransformVerticies(FaceDataRaw.rawVertexData[Faces.BOTTOM]),
                    uvs = blockUV[Faces.BOTTOM]
                }},

            };
        }

        private Dictionary<Faces, List<Vector2>> GetUvsFromAtlasCoordinates(Dictionary<Faces, Vector2> coords)
        {
            Dictionary<Faces, List<Vector2>> faceData = new Dictionary<Faces, List<Vector2>>();

            foreach(KeyValuePair<Faces, Vector2> faceCoord in coords)
            {
                faceData[faceCoord.Key] = new List<Vector2>
                {
                    new Vector2((faceCoord.Value.X + 1f) / TextureData.ATLAS_SIZE, (faceCoord.Value.Y + 1f) /TextureData.ATLAS_SIZE),
                    new Vector2(faceCoord.Value.X / TextureData.ATLAS_SIZE, (faceCoord.Value.Y + 1f) / TextureData.ATLAS_SIZE),
                    new Vector2(faceCoord.Value.X / TextureData.ATLAS_SIZE, faceCoord.Value.Y / TextureData.ATLAS_SIZE),
                    new Vector2((faceCoord.Value.X + 1f) / TextureData.ATLAS_SIZE, faceCoord.Value.Y / TextureData.ATLAS_SIZE),
                };
            }

            return faceData;
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
