using OpenTK.Mathematics;
using System.Collections.Generic;

namespace Hexoidra.World
{
    internal class TextureData
    {
        internal const float ATLAS_SIZE = 16f;
        internal static readonly Dictionary<BlockType, Dictionary<Faces, List<Vector2>>> blockTypeUvs =
            new Dictionary<BlockType, Dictionary<Faces, List<Vector2>>>
            {   
                {
                    BlockType.DIRT, new Dictionary<Faces, List<Vector2>>()
                    {
                        {
                            Faces.FRONT, new List<Vector2>
                            {
                                new Vector2(2f / ATLAS_SIZE, 15f / ATLAS_SIZE),
                                new Vector2(3f / ATLAS_SIZE, 15f / ATLAS_SIZE),
                                new Vector2(3f / ATLAS_SIZE, 1f),
                                new Vector2(2f / ATLAS_SIZE, 1f)
                            }
                        },
                        {
                            Faces.BACK, new List<Vector2>
                            {
                                new Vector2(2f / ATLAS_SIZE, 15f / ATLAS_SIZE),
                                new Vector2(3f / ATLAS_SIZE, 15f / ATLAS_SIZE),
                                new Vector2(3f / ATLAS_SIZE, 1f),
                                new Vector2(2f / ATLAS_SIZE, 1f)
                            }
                        },
                        {
                            Faces.LEFT, new List<Vector2>
                            {
                                new Vector2(2f / ATLAS_SIZE, 15f / ATLAS_SIZE),
                                new Vector2(3f / ATLAS_SIZE, 15f / ATLAS_SIZE),
                                new Vector2(3f / ATLAS_SIZE, 1f),
                                new Vector2(2f / ATLAS_SIZE, 1f)
                            }
                        },
                        {
                            Faces.RIGHT, new List<Vector2>
                            {
                                new Vector2(3f / ATLAS_SIZE, 1f),
                                new Vector2(2f / ATLAS_SIZE, 1f),
                                new Vector2(2f / ATLAS_SIZE, 15f / ATLAS_SIZE),
                                new Vector2(3f / ATLAS_SIZE, 15f / ATLAS_SIZE),
                            }
                        },
                        {
                            Faces.TOP, new List<Vector2>
                            {
                                new Vector2(3f / ATLAS_SIZE, 1f),
                                new Vector2(2f / ATLAS_SIZE, 1f),
                                new Vector2(2f / ATLAS_SIZE, 15f / ATLAS_SIZE),
                                new Vector2(3f / ATLAS_SIZE, 15f / ATLAS_SIZE),
                            }
                        },
                        {
                            Faces.BOTTOM, new List<Vector2>
                            {
                                new Vector2(3f / ATLAS_SIZE, 1f),
                                new Vector2(2f / ATLAS_SIZE, 1f),
                                new Vector2(2f / ATLAS_SIZE, 15f / ATLAS_SIZE),
                                new Vector2(3f / ATLAS_SIZE, 15f / ATLAS_SIZE),
                            }
                        }
                    }
                },
                {
                    BlockType.GRASS, new Dictionary<Faces, List<Vector2>>()
                    {
                        {
                            Faces.FRONT, new List<Vector2>
                            {
                                new Vector2(4f / ATLAS_SIZE, 1f),
                                new Vector2(3f / ATLAS_SIZE, 1f),
                                new Vector2(3f / ATLAS_SIZE, 15f / ATLAS_SIZE),
                                new Vector2(4f / ATLAS_SIZE, 15f / ATLAS_SIZE),
                            }
                        },
                        {   
                            Faces.BACK, new List<Vector2>
                            {
                                new Vector2(4f / ATLAS_SIZE, 1f),
                                new Vector2(3f / ATLAS_SIZE, 1f),
                                new Vector2(3f / ATLAS_SIZE, 15f / ATLAS_SIZE),
                                new Vector2(4f / ATLAS_SIZE, 15f / ATLAS_SIZE),
                            }
                        },
                        {
                            Faces.LEFT, new List<Vector2>
                            {
                                new Vector2(4f / ATLAS_SIZE, 1f),
                                new Vector2(3f / ATLAS_SIZE, 1f),
                                new Vector2(3f / ATLAS_SIZE, 15f / ATLAS_SIZE),
                                new Vector2(4f / ATLAS_SIZE, 15f / ATLAS_SIZE),
                            }
                        },
                        {
                            Faces.RIGHT, new List<Vector2>
                            {
                                new Vector2(4f / ATLAS_SIZE, 1f),
                                new Vector2(3f / ATLAS_SIZE, 1f),
                                new Vector2(3f / ATLAS_SIZE, 15f / ATLAS_SIZE),
                                new Vector2(4f / ATLAS_SIZE, 15f / ATLAS_SIZE),
                            }
                        },
                        {
                            Faces.TOP, new List<Vector2>
                            {
                                new Vector2(8f / ATLAS_SIZE, 14f / ATLAS_SIZE),
                                new Vector2(7f / ATLAS_SIZE, 14f / ATLAS_SIZE),
                                new Vector2(7f / ATLAS_SIZE, 13f / ATLAS_SIZE),
                                new Vector2(8f / ATLAS_SIZE, 13f / ATLAS_SIZE),
                            }
                        },
                        {
                            Faces.BOTTOM, new List<Vector2>
                            {
                                new Vector2(3f / ATLAS_SIZE, 1f),
                                new Vector2(2f / ATLAS_SIZE, 1f),
                                new Vector2(2f / ATLAS_SIZE, 15f / ATLAS_SIZE),
                                new Vector2(3f / ATLAS_SIZE, 15f / ATLAS_SIZE),
                            }
                        }
                    }
                },
            };
    }
}
