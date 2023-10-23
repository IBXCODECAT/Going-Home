using Hexoidra.Data;
using OpenTK.Mathematics;

namespace Hexoidra.World
{
    internal class Block
    {
        private Vector3 position;
        internal BlockType blockType;

        private Dictionary<BlockFace, FaceData> faces;

        internal Dictionary<BlockFace, List<Vector2>> blockUV = new Dictionary<BlockFace, List<Vector2>>()
        {
            { BlockFace.FRONT, new List<Vector2>() },
            { BlockFace.BACK, new List<Vector2>() },
            { BlockFace.LEFT, new List<Vector2>() },
            { BlockFace.RIGHT, new List<Vector2>() },
            { BlockFace.TOP, new List<Vector2>() },
            { BlockFace.BOTTOM, new List<Vector2>() },
        };

        public Block(Vector3 position, BlockType blockType = BlockType.AIR)
        {
            this.position = position;
            this.blockType = blockType;

            
            if (blockType != BlockType.AIR)
            {
                blockUV = GetUvsFromAtlasCoordinates(TextureData.blockTypeUVCoordinates[blockType]);
            }
            
            faces = new Dictionary<BlockFace, FaceData>()
            {
                {BlockFace.FRONT, new FaceData {
                    vertices = TransformVerticies(FaceDataRaw.rawVertexData[BlockFace.FRONT]),
                    uvs = blockUV[BlockFace.FRONT]
                }},
                {BlockFace.BACK, new FaceData {
                    vertices = TransformVerticies(FaceDataRaw.rawVertexData[BlockFace.BACK]),
                    uvs = blockUV[BlockFace.BACK]
                }},
                {BlockFace.LEFT, new FaceData {
                    vertices = TransformVerticies(FaceDataRaw.rawVertexData[BlockFace.LEFT]),
                    uvs = blockUV[BlockFace.LEFT]
                }},
                {BlockFace.RIGHT, new FaceData {
                    vertices = TransformVerticies(FaceDataRaw.rawVertexData[BlockFace.RIGHT]),
                    uvs = blockUV[BlockFace.RIGHT]
                }},
                {BlockFace.TOP, new FaceData {
                    vertices = TransformVerticies(FaceDataRaw.rawVertexData[BlockFace.TOP]),
                    uvs = blockUV[BlockFace.TOP]
                }},
                {BlockFace.BOTTOM, new FaceData {
                    vertices = TransformVerticies(FaceDataRaw.rawVertexData[BlockFace.BOTTOM]),
                    uvs = blockUV[BlockFace.BOTTOM]
                }},

            };
        }

        private Dictionary<BlockFace, List<Vector2>> GetUvsFromAtlasCoordinates(Dictionary<BlockFace, Vector2> coords)
        {
            Dictionary<BlockFace, List<Vector2>> faceData = new Dictionary<BlockFace, List<Vector2>>();

            foreach(KeyValuePair<BlockFace, Vector2> faceCoord in coords)
            {
                faceData[faceCoord.Key] = new List<Vector2>
                {
                    new Vector2((faceCoord.Value.X + 1f) / TextureData.ATLAS_ITEM_SIZE, (faceCoord.Value.Y + 1f) /TextureData.ATLAS_ITEM_SIZE),
                    new Vector2(faceCoord.Value.X / TextureData.ATLAS_ITEM_SIZE, (faceCoord.Value.Y + 1f) / TextureData.ATLAS_ITEM_SIZE),
                    new Vector2(faceCoord.Value.X / TextureData.ATLAS_ITEM_SIZE, faceCoord.Value.Y / TextureData.ATLAS_ITEM_SIZE),
                    new Vector2((faceCoord.Value.X + 1f) / TextureData.ATLAS_ITEM_SIZE, faceCoord.Value.Y / TextureData.ATLAS_ITEM_SIZE),
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

        public FaceData GetFace(BlockFace face)
        {
            return faces[face];
        }
    }
}
