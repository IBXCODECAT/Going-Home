using Hexoidra.Graphics;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace Hexoidra.World
{
    internal class Chunk
    {
        private List<Vector3> chunkVerts;
        private List<Vector2> chunkUVs;
        private List<uint> chunkIndices;

        private const int CHUNK_SIZE = 16;
        private const int CHUNK_HEIGHT = 32;

        internal Vector3 position;

        private uint indexCount;


        private VertexArrayObject chunkVertexArray;
        private VertexBufferObject chunkVertexBuffer;
        private VertexBufferObject chunkUVBuffer;
        private IndexBufferObject chunkIndexBuffer;
        private Texture texture;

        internal Chunk(Vector3 position)
        {
            this.position = position;

            chunkVerts = new List<Vector3>();
            chunkUVs = new List<Vector2>();
            chunkIndices = new List<uint>();

            GenBlocks();
            BuildChunk();
        }

        internal void GenChunk() { }

        private void GenBlocks()
        {
            for(int i = 0; i < 3; i++)
            {
                Block block = new Block(new Vector3(i, 0, 0));

                int faceCount = 0;

                if (i == 0)
                {
                    FaceData leftFaceData = block.GetFace(Faces.LEFT);
                    chunkVerts.AddRange(leftFaceData.vertices);
                    chunkUVs.AddRange(leftFaceData.uvs);
                    faceCount++;
                }
                if (i == 2)
                {
                    FaceData rightFaceData = block.GetFace(Faces.RIGHT);
                    chunkVerts.AddRange(rightFaceData.vertices);
                    chunkUVs.AddRange(rightFaceData.uvs);
                    faceCount++;
                }

                FaceData frontFaceData = block.GetFace(Faces.FRONT);
                chunkVerts.AddRange(frontFaceData.vertices);
                chunkUVs.AddRange(frontFaceData.uvs);

                FaceData backFaceData = block.GetFace(Faces.BACK);
                chunkVerts.AddRange(backFaceData.vertices);
                chunkUVs.AddRange(backFaceData.uvs);

                FaceData topFaceData = block.GetFace(Faces.TOP);
                chunkVerts.AddRange(topFaceData.vertices);
                chunkUVs.AddRange(topFaceData.uvs);

                FaceData bottomFaceData = block.GetFace(Faces.BOTTOM);
                chunkVerts.AddRange(bottomFaceData.vertices);
                chunkUVs.AddRange(bottomFaceData.uvs);

                faceCount += 4;

                AddIndiciesForFace(faceCount);
            }
        }

        private void AddIndiciesForFace(int amtFaces)
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
