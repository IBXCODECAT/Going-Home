using Hexoidra.Graphics;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using SimplexNoise;

namespace Hexoidra.World
{
    internal class Chunk
    {
        private List<Vector3> chunkVerts;
        private List<Vector2> chunkUVs;
        private List<uint> chunkIndices;

        private const int CHUNK_SIZE = 16;
        private const int CHUNK_HEIGHT = 128;

        internal Vector3 position;

        private uint indexCount;



        private VertexArrayObject chunkVertexArray;
        private VertexBufferObject chunkVertexBuffer;
        private VertexBufferObject chunkUVBuffer;
        private IndexBufferObject chunkIndexBuffer;
        private Texture texture;

        private int[,] columnHeight = new int[CHUNK_SIZE, CHUNK_SIZE];
        private Block[,,] chunkBlocks = new Block[CHUNK_SIZE, CHUNK_HEIGHT, CHUNK_SIZE];
        
        internal Chunk(Vector3 position)
        {
            this.position = position;

            chunkVerts = new List<Vector3>();
            chunkUVs = new List<Vector2>();
            chunkIndices = new List<uint>();

            float[,] heightmap = GenChunk();

            Console.WriteLine(heightmap.Length);

            GenBlocks(heightmap);
            GenerateRequiredBlockFaces(heightmap);
            BuildChunk();
        }

        private float[,] GenChunk()
        {
            float[,] heightmap = new float[CHUNK_SIZE, CHUNK_SIZE];

            Noise.Seed = 123456;

            for(int x = 0; x < CHUNK_SIZE; x++)
            {
                for(int z  = 0; z < CHUNK_SIZE; z++)
                {
                    heightmap[x, z] = Noise.CalcPixel2D(x, z, 0.01f);
                }
            }

            return heightmap;
        }

        private void GenBlocks(float[,] heightmap)
        {
            for (int x = 0; x < CHUNK_SIZE; x++)
            {
                for (int z = 0; z < CHUNK_SIZE; z++)
                {
                    columnHeight[x, z] = (int)(heightmap[x, z] / 10);

                    Console.WriteLine($"{columnHeight}");

                    for (int y = 0; y < CHUNK_HEIGHT; y++)
                    {
                        if(y < columnHeight[x, z])
                        {
                            chunkBlocks[x, y, z] = new Block(new Vector3(x, y, z), BlockType.DIRT);
                        }
                        else
                        {
                            chunkBlocks[x, y, z] = new Block(new Vector3(x, y, z), BlockType.AIR);
                        }
                    }
                }
            }
        }

        private void GenerateRequiredBlockFaces(float[,] heightmap)
        {
            for(int x = 0; x < CHUNK_SIZE;  x++)
            {
                for(int z = 0; z < CHUNK_SIZE; z++)
                {
                    for(int y = 0; y < columnHeight[x, z]; y++)
                    {
                        int totalFaces = 0;


                        //front face - block to front is air or is farthest front in chunk
                        if(z < CHUNK_SIZE - 1)
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
                        if(x < CHUNK_SIZE - 1)
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

                        if(y < columnHeight[x, z] - 1)
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

            texture = new Texture("tex.png");
        }

        internal void Render(Shader shader)
        {
            shader.Bind();
            chunkVertexArray.Bind();
            chunkIndexBuffer.Bind();
            texture.Bind();

            GL.DrawElements(PrimitiveType.Triangles, chunkIndices.Count, DrawElementsType.UnsignedInt, 0);
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
