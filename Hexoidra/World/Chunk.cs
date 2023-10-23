using Hexoidra.Graphics;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using SimplexNoise;
using System.Runtime.CompilerServices;
using Hexoidra.Data;

namespace Hexoidra.World
{
    public class Chunk
    {
        /// <summary>
        /// Contains the coordinates for a chunk in both chunk coords  (EX: (1, 1)) and
        /// and position (Ex: (1 * CHUNK_SIZE, 1 * CHUNK_SIZE)
        /// </summary>
        internal struct ChunkPositionInfo
        {
            internal Vector2i chunkCoords;
            internal Vector2i chunkXYOriginInWorldSpace;

            internal ChunkPositionInfo(Vector2i position)
            {
                this.chunkCoords = position;
                this.chunkXYOriginInWorldSpace = position * CHUNK_SIZE;
            }
        }

        private List<Vector3> chunkVerts;
        private List<Vector2> chunkUVs;
        private List<uint> chunkIndices;

        public const int CHUNK_SIZE = 32;
        public const int CHUNK_HEIGHT = 128;

        internal ChunkPositionInfo position;

        private uint indexCount;

        private VertexArrayObject chunkVAO;
        private VertexBufferObject chunkVBO;
        private VertexBufferObject chunkUVBuffer;
        private IndexBufferObject chunkIndexBuffer;
        private Texture texture;

        private Block[,,] chunkBlocks = new Block[CHUNK_SIZE, CHUNK_HEIGHT, CHUNK_SIZE];
        
        internal Chunk(ChunkPositionInfo position)
        {
            this.position = position;

            chunkVerts = new List<Vector3>();
            chunkUVs = new List<Vector2>();
            chunkIndices = new List<uint>();

            chunkBlocks = WorldGen.GenChunkBlocks(position);

            ConstructOptimizedMesh();
            BuildChunk();
        }

        /// <summary>
        /// Constructs an optimized chunk mesh so that each block only renders
        /// its face if it is visible to the player (touching air)
        /// [or is on chunk edge]
        /// </summary>
        private void ConstructOptimizedMesh()
        {
            for(int x = 0; x < CHUNK_SIZE;  x++)
            {
                for(int z = 0; z < CHUNK_SIZE; z++)
                {
                    for(int y = 0; y < CHUNK_HEIGHT; y++)
                    {
                        int totalFaces = 0;

                        if (chunkBlocks[x, y, z].blockType != BlockType.AIR)
                        {
                            //front face - block to front is air or is farthest front in chunk
                            if (z < CHUNK_SIZE - 1)
                            {
                                if (chunkBlocks[x, y, z + 1].blockType == BlockType.AIR)
                                {
                                    GenerateBlockFace(chunkBlocks[x, y, z], BlockFace.FRONT);
                                    totalFaces++;
                                }
                            }
                            else
                            {
                                GenerateBlockFace(chunkBlocks[x, y, z], BlockFace.FRONT);
                                totalFaces++;
                            }

                            //back face

                            if (z > 0)
                            {
                                if (chunkBlocks[x, y, z - 1].blockType == BlockType.AIR)
                                {
                                    GenerateBlockFace(chunkBlocks[x, y, z], BlockFace.BACK);
                                    totalFaces++;
                                }
                            }
                            else
                            {
                                GenerateBlockFace(chunkBlocks[x, y, z], BlockFace.BACK);
                                totalFaces++;
                            }

                            //Left faces - block to left is air or is farthest left in chunk
                            if (x > 0)
                            {
                                if (chunkBlocks[x - 1, y, z].blockType == BlockType.AIR)
                                {
                                    GenerateBlockFace(chunkBlocks[x, y, z], BlockFace.LEFT);
                                    totalFaces++;
                                }
                            }
                            else
                            {
                                GenerateBlockFace(chunkBlocks[x, y, z], BlockFace.LEFT);
                                totalFaces++;
                            }

                            //Right faces - block to right is air or is furthest right in chunk
                            if (x < CHUNK_SIZE - 1)
                            {
                                if (chunkBlocks[x + 1, y, z].blockType == BlockType.AIR)
                                {
                                    GenerateBlockFace(chunkBlocks[x, y, z], BlockFace.RIGHT);
                                    totalFaces++;
                                }
                            }
                            else
                            {
                                GenerateBlockFace(chunkBlocks[x, y, z], BlockFace.RIGHT);
                                totalFaces++;
                            }

                            //Top faces - block above is empty or is the furthest up in chunk

                            if (y < CHUNK_HEIGHT - 1)
                            {
                                if (chunkBlocks[x, y + 1, z].blockType == BlockType.AIR)
                                {
                                    GenerateBlockFace(chunkBlocks[x, y, z], BlockFace.TOP);
                                    totalFaces++;
                                }
                            }
                            else
                            {
                                GenerateBlockFace(chunkBlocks[x, y, z], BlockFace.TOP);
                                totalFaces++;
                            }
                            AddIndiciesForFaces(totalFaces);

                            //Bottom faces - block below is empty or is the furthest down in chunk
                            if (y > 0)
                            {
                                if (chunkBlocks[x, y - 1, z].blockType == BlockType.AIR)
                                {
                                    GenerateBlockFace(chunkBlocks[x, y, z], BlockFace.BOTTOM);
                                    totalFaces++;
                                }
                            }
                            else
                            {
                                GenerateBlockFace(chunkBlocks[x, y, z], BlockFace.BOTTOM);
                                totalFaces++;
                            }

                            AddIndiciesForFaces(totalFaces);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Generate a block face
        /// </summary>
        /// <param name="block">The block to generate the face for</param>
        /// <param name="face">The face to generate on the specified block</param>
        private void GenerateBlockFace(Block block, BlockFace face)
        {
            FaceData data = block.GetFace(face);
            chunkVerts.AddRange(data.vertices);
            chunkUVs.AddRange(data.uvs);
        }

        private void AddIndiciesForFaces(int amtFaces)
        {
            for (int i = 0; i < amtFaces; i++)
            {
                chunkIndices.Add(0 + indexCount);
                chunkIndices.Add(1 + indexCount);
                chunkIndices.Add(2 + indexCount);
                chunkIndices.Add(2 + indexCount);
                chunkIndices.Add(3 + indexCount);
                chunkIndices.Add(0 + indexCount);

                indexCount += 4;
            }
        }

        private void BuildChunk()
        {
            chunkVAO = new VertexArrayObject();
            chunkVAO.Bind();

            chunkVBO = new VertexBufferObject(chunkVerts);
            chunkVBO.Bind();
            chunkVAO.LinkVertexBufferObject(0, 3, chunkVBO);

            chunkUVBuffer = new VertexBufferObject(chunkUVs);
            chunkUVBuffer.Bind();
            chunkVAO.LinkVertexBufferObject(1, 2, chunkUVBuffer);

            chunkIndexBuffer = new IndexBufferObject(chunkIndices);

            texture = new Texture("atlas.png");
        }

        internal void Render(Shader shader)
        {
            shader.Bind();
            chunkVAO.Bind();
            chunkIndexBuffer.Bind();
            texture.Bind();

            GL.DrawElements(PrimitiveType.Triangles, chunkIndices.Count, DrawElementsType.UnsignedInt, 0);

            texture.Unbind();
            chunkIndexBuffer.Unbind();
            chunkVAO.Unbind();
        }

        internal void Dispose()
        {
            chunkVAO.Dispose();
            chunkVBO.Dispose();
            chunkUVBuffer.Dispose();
            chunkIndexBuffer.Dispose();
            texture.Dispose();
        }
    }
}
