using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Hexoidra.Graphics;
using Hexoidra.World;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using static System.Formats.Asn1.AsnWriter;

namespace Hexoidra.UI
{
    internal struct CharacterData
    {
        public List<Vector3> vertices;
        public List<Vector2> uvs;
    }

    internal class Text
    {
        private const string CHAR_STRING = "ABCDEFGHIJKLMNOPQRSTUVWXYZ.,;:$#'!\"/?%&()@1234567890";
        private readonly char[] CHARACTERS = CHAR_STRING.ToCharArray();

        private const byte CHARACTER_HEIGHT = 8;
        private const byte CHARACTER_WIDTH = 8;

        private Texture textAtlas = new Texture("../../../Textures/text.png");
        
        // Create a VBO for the character
        private readonly List<Vector3> characterVertexData = new List<Vector3>()
        {
            // Vertex positions (x, y)
            Vector3.Zero,
            new Vector3(8, 0, 0),
            new Vector3(8, 0, 8),
            new Vector3(0, 0, 8)
        };

        private static List<Vector3> characterUVs = new List<Vector3>();

        private List<uint> charIndices = new List<uint>();

        VertexArrayObject charVAO;
        VertexBufferObject charVBO;
        VertexBufferObject charUVs;
        IndexBufferObject charIndexBuffer;

        

        internal void BuildText(string content)
        {
            foreach (char c in content)
            {
                if (CHARACTERS.Contains(c))
                {

                }
                else
                {
                    Console.WriteLine($"The character {c} is not availible in the character atals.");
                }
            }

            charVAO = new VertexArrayObject();
            charVAO.Bind();

            charVBO = new VertexBufferObject(characterVertexData);
            charVBO.Bind();
            charVAO.LinkVertexBufferObject(0, characterVertexData.Count * Vector3.SizeInBytes, charVBO);

            charUVs = new VertexBufferObject(characterUVs);
            charUVs.Bind();
            charVAO.LinkVertexBufferObject(1, 2, charUVs);

            charIndexBuffer = new IndexBufferObject(charIndices);

            textAtlas = new Texture("../../../Textures/text.png");

            
        }

        internal void Render(Shader shader)
        {
            shader.Bind();
            charVAO.Bind();
            charVBO.Bind();
            textAtlas.Bind();
            charIndexBuffer.Bind();

            GL.DrawElements(PrimitiveType.Triangles, characterVertexData.Count, DrawElementsType.UnsignedInt, 0);

            charIndexBuffer.Unbind();
            textAtlas.Unbind();
            charVBO.UnBind();
            charVAO.Unbind();
        }

        internal void Dispose()
        {
            textAtlas.Dispose();
            charUVs.Dispose();
            charVBO.Dispose();
            charIndexBuffer.Dispose();
            charVAO.Dispose();
        }
    }
}
