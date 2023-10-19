using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

using StbImageSharp;

namespace Hexoidra
{
    internal class Game : GameWindow
    {

        float[] verticies =
        {
            -0.5f, 0.5f, 0f, //top left vertex - 0
            0.5f, 0.5f, 0f, // top right vertex - 1
            0.5f, -0.5f, 0f, //bottom right vertex - 2
            -0.5f, -0.5f, 0f //botom left vertex -3
        };

        float[] texCoords =
        {
            0f, 1f, //top-left
            1f, 1f, //top-right
            1f, 0f, //bottom-right
            0f, 0f //bottom-left
        };

        uint[] indicies =
        {
            //top triangle
            0, 1, 2,
            //bottom triangle
            2, 3, 0
        };

        //Render pipeline vars
        int vao;
        int shaderProgram;
        int vbo;
        int ebo;
        int textureID;
        int textureVbo;

        private int width;
        private int height;

        public Game(int width, int height) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {

            this.width = width;
            this.height = height;

            //Center window on monitor
            CenterWindow(new Vector2i(width, height));
        }

        //Called every time the screen is resized
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);

            width = e.Width;
            height = e.Height;
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            //Create and bind the vao to the current OpenGL Context
            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);

            //====================VERTEX VBO====================//

            //generate and bind the vbo buffer
            vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

            //put data in the vbo
            GL.BufferData(
                BufferTarget.ArrayBuffer,
                verticies.Length * sizeof(float),
                verticies,
                BufferUsageHint.StaticDraw
            );

            //point slot (0) of the VAO to the currently bound VBO (vbo) and enable the slot
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexArrayAttrib(vao, 0);

            //unbind the vbo
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            //====================TEXTURES VBO====================//

            //generate a buffer and bind the buffer
            textureVbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVbo);

            //store the data in the vbo
            GL.BufferData(
                BufferTarget.ArrayBuffer,
                texCoords.Length * sizeof(float),
                texCoords,
                BufferUsageHint.StaticDraw
            );
            
            //point slot (1) of the VAO to our texture vbo and enable the slot
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexArrayAttrib(vao, 1);

            //unbind the vbo
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);


            //====================MORE STUFF====================//

            //unbind the vao and vbo respectively
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);


            //EBO Buffer
            ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indicies.Length * sizeof(uint), indicies, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            //Create the shader program
            shaderProgram = GL.CreateProgram();

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, LoadShaderSource("default.vert"));
            GL.CompileShader(vertexShader);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, LoadShaderSource("default.frag"));
            GL.CompileShader(fragmentShader);

            GL.AttachShader(shaderProgram, vertexShader);
            GL.AttachShader(shaderProgram, fragmentShader);

            GL.LinkProgram(shaderProgram);

            //Delete the shaders
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            //============Load Textures============

            textureID = GL.GenTexture();
            //Activate the fist texture in the unit
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureID);

            //Texture params
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            //mipmap params - does not use bluring due to pixelated vibes
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            //load image
            StbImage.stbi_set_flip_vertically_on_load(1); //StbImage reads in opisisate direction to opengl by default
            ImageResult coolTexture = ImageResult.FromStream(File.OpenRead("../../../Textures/tex.png"), ColorComponents.RedGreenBlueAlpha);

            //Give openGL texture data
            GL.TexImage2D(
                TextureTarget.Texture2D,
                0, PixelInternalFormat.Rgba,
                coolTexture.Width,
                coolTexture.Height,
                0,
                PixelFormat.Rgba,
                PixelType.UnsignedByte,
                coolTexture.Data
            );

            //unbind texture
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            GL.DeleteBuffer(vbo);
            GL.DeleteVertexArray(vao);
            GL.DeleteBuffer(ebo);
            GL.DeleteProgram(shaderProgram);

            GL.DeleteTexture(textureID);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(0.6f, 0.3f, 1f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            //Draw triangle
            GL.UseProgram(shaderProgram);

            GL.BindTexture(TextureTarget.Texture2D, textureID);

            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.DrawElements(PrimitiveType.Triangles, indicies.Length, DrawElementsType.UnsignedInt, 0);

            //GL.DrawArrays(PrimitiveType.Triangles, 0, 4);

            Context.SwapBuffers();

            base.OnRenderFrame(args);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
        }

        /// <summary>
        /// Method to load a shader file and return it's contents as a string
        /// </summary>
        /// <param name="filePath">The path of the file</param>
        /// <returns>Contents of the file as a string</returns>
        public static string LoadShaderSource(string filePath)
        {
            string shaderSource = "";

            try
            {
                using (StreamReader sr = new StreamReader("../../../Shaders/" + filePath))
                {
                    shaderSource = sr.ReadToEnd();
                }
                //Console.WriteLine(shaderSource);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to load shader source file: " + ex.Message);
            }

            return shaderSource;
        }
    }
}
