using OpenTK.Mathematics;

namespace Hexoidra.Data
{
    public enum BlockType
    {
        AIR,
        GRASS,
        DIRT,
        STONE
    }

    public enum BlockFace
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
        public List<Vector3> vertices;
        public List<Vector2> uvs;
    }

    public static class CharacterData
    {
        public struct CharacterFaceDataRaw
        {
            public static readonly List<Vector3> rawVertexData = new List<Vector3>()
            {
                new Vector3(0, 0, 0), //top left
                new Vector3(1, 0, 0), //top right
                new Vector3(1, -1, 0), // bottom right
                new Vector3(0, -1, 0), //bottom left
            };
        }
    }
    

    public struct FaceDataRaw
    {
        public static readonly Dictionary<BlockFace, List<Vector3>> rawVertexData = new Dictionary<BlockFace, List<Vector3>>()
        {
            {BlockFace.FRONT, new List<Vector3>()
            {
                new Vector3(-0.5f, 0.5f, 0.5f), // topleft vert
                new Vector3(0.5f, 0.5f, 0.5f), // topright vert
                new Vector3(0.5f, -0.5f, 0.5f), // bottomright vert
                new Vector3(-0.5f, -0.5f, 0.5f), // bottomleft vert
            } },
            {BlockFace.BACK, new List<Vector3>()
            {
                new Vector3(0.5f, 0.5f, -0.5f), // topleft vert
                new Vector3(-0.5f, 0.5f, -0.5f), // topright vert
                new Vector3(-0.5f, -0.5f, -0.5f), // bottomright vert
                new Vector3(0.5f, -0.5f, -0.5f), // bottomleft vert
            } },
            {BlockFace.LEFT, new List<Vector3>()
            {
                new Vector3(-0.5f, 0.5f, -0.5f), // topleft vert
                new Vector3(-0.5f, 0.5f, 0.5f), // topright vert
                new Vector3(-0.5f, -0.5f, 0.5f), // bottomright vert
                new Vector3(-0.5f, -0.5f, -0.5f), // bottomleft vert
            } },
            {BlockFace.RIGHT, new List<Vector3>()
            {
                new Vector3(0.5f, 0.5f, 0.5f), // topleft vert
                new Vector3(0.5f, 0.5f, -0.5f), // topright vert
                new Vector3(0.5f, -0.5f, -0.5f), // bottomright vert
                new Vector3(0.5f, -0.5f, 0.5f), // bottomleft vert
            } },
            {BlockFace.TOP, new List<Vector3>()
            {
                new Vector3(-0.5f, 0.5f, -0.5f), // topleft vert
                new Vector3(0.5f, 0.5f, -0.5f), // topright vert
                new Vector3(0.5f, 0.5f, 0.5f), // bottomright vert
                new Vector3(-0.5f, 0.5f, 0.5f), // bottomleft vert
            } },
            {BlockFace.BOTTOM, new List<Vector3>()
            {
                new Vector3(-0.5f, -0.5f, 0.5f), // topleft vert
                new Vector3(0.5f, -0.5f, 0.5f), // topright vert
                new Vector3(0.5f, -0.5f, -0.5f), // bottomright vert
                new Vector3(-0.5f, -0.5f, -0.5f), // bottomleft vert
            } },
        };
    }
}
