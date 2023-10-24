using Hexoidra.Graphics;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using SimplexNoise;
using System.Runtime.CompilerServices;

using Hexoidra.Globals;

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
                this.chunkXYOriginInWorldSpace = position * Globals.Settings.CHUNK_SIZE;
            }
        }

        private List<Vector3> chunkVerts;
        private List<Vector2> chunkUVs;
        private List<uint> chunkIndices;

        internal ChunkPositionInfo position;

        private uint indexCount;

        private VertexArrayObject chunkVertexArray;
        private VertexBufferObject chunkVertexBuffer;
        private VertexBufferObject chunkUVBuffer;
        private IndexBufferObject chunkIndexBuffer;
        private Texture texture;

        private static List<Chunk> chunks = new List<Chunk>();

        private Block[,,] chunkBlocks = new Block[Globals.Settings.CHUNK_SIZE, Globals.Settings.CHUNK_HEIGHT, Globals.Settings.CHUNK_SIZE];
        
        internal Chunk(ChunkPositionInfo position)
        {
            this.position = position;

            chunkVerts = new List<Vector3>();
            chunkUVs = new List<Vector2>();
            chunkIndices = new List<uint>();

            chunkBlocks = WorldGen.GenChunkBlocks(position);

            ConstructOptimizedMesh();
            BuildChunk();

            chunks.Add(this);
        }

        internal static Chunk GetChunkAtPosition(Vector2i position)
        {
            Console.WriteLine(position);

            foreach(Chunk chunk in chunks)
            {
                if(chunk.position.chunkCoords == position)
                {
                    return chunk;
                }
            }

            throw new Exception("Must call GetChunkAtPosition() at a position that actually contains a chunk.");
        }

        /// <summary>
        /// Constructs an optimized chunk mesh so that each block only renders
        /// its face if it is visible to the player (touching air)
        /// [or is on chunk edge]
        /// </summary>
        private void ConstructOptimizedMesh()
        {
            for(int x = 0; x < Globals.Settings.CHUNK_SIZE;  x++)
            {
                for(int z = 0; z < Globals.Settings.CHUNK_SIZE; z++)
                {
                    for(int y = 0; y < Globals.Settings.CHUNK_HEIGHT; y++)
                    {
                        int totalFaces = 0;

                        if (chunkBlocks[x, y, z].blockType != BlockType.AIR)
                        {
                            //front face - block to front is air or is farthest front in chunk
                            if (z < Globals.Settings.CHUNK_SIZE - 1)
                            {
                                if (chunkBlocks[x, y, z + 1].blockType == BlockType.AIR)
                                {
                                    GenerateBlockFace(chunkBlocks[x, y, z], Faces.FRONT);
                                    totalFaces++;
                                }
                            }
                            else
                            {
                                GenerateBlockFace(chunkBlocks[x, y, z], Faces.FRONT);
                                totalFaces++;
                            }

                            //back face

                            if (z > 0)
                            {
                                if (chunkBlocks[x, y, z - 1].blockType == BlockType.AIR)
                                {
                                    GenerateBlockFace(chunkBlocks[x, y, z], Faces.BACK);
                                    totalFaces++;
                                }
                            }
                            else
                            {
                                GenerateBlockFace(chunkBlocks[x, y, z], Faces.BACK);
                                totalFaces++;
                            }

                            //Left faces - block to left is air or is farthest left in chunk
                            if (x > 0)
                            {
                                if (chunkBlocks[x - 1, y, z].blockType == BlockType.AIR)
                                {
                                    GenerateBlockFace(chunkBlocks[x, y, z], Faces.LEFT);
                                    totalFaces++;
                                }
                            }
                            else
                            {
                                GenerateBlockFace(chunkBlocks[x, y, z], Faces.LEFT);
                                totalFaces++;
                            }

                            //Right faces - block to right is air or is furthest right in chunk
                            if (x < Settings.CHUNK_SIZE - 1)
                            {
                                if (chunkBlocks[x + 1, y, z].blockType == BlockType.AIR)
                                {
                                    GenerateBlockFace(chunkBlocks[x, y, z], Faces.RIGHT);
                                    totalFaces++;
                                }
                            }
                            else
                            {
                                GenerateBlockFace(chunkBlocks[x, y, z], Faces.RIGHT);
                                totalFaces++;
                            }

                            //Top faces - block above is empty or is the furthest up in chunk

                            if (y < Settings.CHUNK_HEIGHT - 1)
                            {
                                if (chunkBlocks[x, y + 1, z].blockType == BlockType.AIR)
                                {
                                    GenerateBlockFace(chunkBlocks[x, y, z], Faces.TOP);
                                    totalFaces++;
                                }
                            }
                            else
                            {
                                GenerateBlockFace(chunkBlocks[x, y, z], Faces.TOP);
                                totalFaces++;
                            }
                            AddIndiciesForFaces(totalFaces);

                            //Bottom faces - block below is empty or is the furthest down in chunk
                            if (y > 0)
                            {
                                if (chunkBlocks[x, y - 1, z].blockType == BlockType.AIR)
                                {
                                    GenerateBlockFace(chunkBlocks[x, y, z], Faces.BOTTOM);
                                    totalFaces++;
                                }
                            }
                            else
                            {
                                GenerateBlockFace(chunkBlocks[x, y, z], Faces.BOTTOM);
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
        private void GenerateBlockFace(Block block, Faces face)
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
            chunkVertexArray = new VertexArrayObject();
            chunkVertexArray.Bind();

            chunkVertexBuffer = new VertexBufferObject(chunkVerts);
            chunkVertexBuffer.Bind();
            chunkVertexArray.LinkVertexBufferObject(0, 3, chunkVertexBuffer);

            chunkUVBuffer = new VertexBufferObject(chunkUVs);
            chunkUVBuffer.Bind();
            chunkVertexArray.LinkVertexBufferObject(1, 2, chunkUVBuffer);

            chunkIndexBuffer = new IndexBufferObject(chunkIndices);

            texture = new Texture("atlas.png");
        }

        internal void Render(Shader shader)
        {
            shader.Bind();
            chunkVertexArray.Bind();
            chunkIndexBuffer.Bind();
            texture.Bind();

            GL.DrawElements(PrimitiveType.Triangles, chunkIndices.Count, DrawElementsType.UnsignedInt, 0);

            //Console.WriteLine($"Rendering Chunk @ {position.chunkCoords.X}, {position.chunkCoords.Y}");

            texture.Unbind();
            chunkIndexBuffer.Unbind();
            chunkVertexArray.Unbind();
        }

        internal void Dispose()
        {
            chunkVertexArray.Dispose();
            chunkVertexBuffer.Dispose();
            chunkUVBuffer.Dispose();
            chunkIndexBuffer.Dispose();
            texture.Dispose();
        }
    }
}
