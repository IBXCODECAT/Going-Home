using Hexoidra.Data;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace Hexoidra.World
{
    internal class TextureData
    {
        internal const float ATLAS_ITEM_SIZE = 16f;
        internal const float TEXT_ITEM_SIZE = 8f;

        internal static Dictionary<char, List<Vector2>> characterUVCoordinates = new Dictionary<char, List<Vector2>>()
        {
            {  'A', new List<Vector2>()
            {
                new Vector2(0f, 7f),
                new Vector2(1f, 7f),
                new Vector2(1f, 6f),
                new Vector2(0f, 6f),
            }},
            { 'B', new List<Vector2> 
            { 
                new Vector2(1f, 7f),
                new Vector2(2f, 7f),
                new Vector2(2f, 6f),
                new Vector2(1f, 6f)
            }}
        };

        internal static Dictionary<BlockType, Dictionary<BlockFace, Vector2>> blockTypeUVCoordinates = new Dictionary<BlockType, Dictionary<BlockFace, Vector2>>()
        {
            {BlockType.DIRT , new Dictionary<BlockFace, Vector2>() {
                {BlockFace.FRONT, new Vector2(2f, 15f) },
                {BlockFace.BACK, new Vector2(2f, 15f) },
                {BlockFace.LEFT, new Vector2(2f, 15f) },
                {BlockFace.RIGHT, new Vector2(2f, 15f) },
                {BlockFace.TOP, new Vector2(2f, 15f) },
                {BlockFace.BOTTOM, new Vector2(2f, 15f) },
            } },
            {BlockType.GRASS , new Dictionary<BlockFace, Vector2>() {
                {BlockFace.FRONT, new Vector2(3f, 15f) },
                {BlockFace.BACK, new Vector2(3f, 15f) },
                {BlockFace.LEFT, new Vector2(3f, 15f) },
                {BlockFace.RIGHT, new Vector2(3f, 15f) },
                {BlockFace.TOP, new Vector2(7f, 13f) },
                {BlockFace.BOTTOM, new Vector2(2f, 15f) },
            } },
            {BlockType.STONE , new Dictionary<BlockFace, Vector2>() {
                {BlockFace.FRONT, new Vector2(1f, 15f) },
                {BlockFace.BACK, new Vector2(1f, 15f) },
                {BlockFace.LEFT, new Vector2(1f, 15f) },
                {BlockFace.RIGHT, new Vector2(1f, 15f) },
                {BlockFace.TOP, new Vector2(1f, 15f) },
                {BlockFace.BOTTOM, new Vector2(1f, 15f) },
            } },
        };
    }
}
