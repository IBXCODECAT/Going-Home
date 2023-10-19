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
                FaceData frontFaceData = block.GetFace(Faces.FRONT);
                chunkVerts.AddRange(frontFaceData.verticies);
                chunkUVs.AddRange(frontFaceData.uvs);
                AddIndiciesForFace(1);
            }
        }

        private void AddIndiciesForFace(int amtFaces)
        {
            for(int i = 0; i < amtFaces; i++)
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
            chunkVertexBuffer.Bind();
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
