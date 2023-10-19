using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using StbImageSharp;

namespace Hexoidra
{
    internal class Game : GameWindow
    {
        Camera camera;

        List<Vector3> vertices = new List<Vector3>()
        {
            // Front face
            new Vector3(-0.5f, 0.5f, -0.5f), // Top left vertex
            new Vector3(0.5f, 0.5f, -0.5f),  // Top right vertex
            new Vector3(0.5f, -0.5f, -0.5f), // Bottom right vertex
            new Vector3(-0.5f, -0.5f, -0.5f), // Bottom left vertex

            // Back face
            new Vector3(-0.5f, 0.5f, 0.5f),  // Top left vertex
            new Vector3(0.5f, 0.5f, 0.5f),   // Top right vertex
            new Vector3(0.5f, -0.5f, 0.5f),  // Bottom right vertex
            new Vector3(-0.5f, -0.5f, 0.5f),  // Bottom left vertex

            // Left face
            new Vector3(-0.5f, 0.5f, 0.5f),  // Top left vertex
            new Vector3(-0.5f, 0.5f, -0.5f), // Top right vertex
            new Vector3(-0.5f, -0.5f, -0.5f), // Bottom right vertex
            new Vector3(-0.5f, -0.5f, 0.5f),  // Bottom left vertex

            // Right face
            new Vector3(0.5f, 0.5f, 0.5f),   // Top left vertex
            new Vector3(0.5f, 0.5f, -0.5f),  // Top right vertex
            new Vector3(0.5f, -0.5f, -0.5f), // Bottom right vertex
            new Vector3(0.5f, -0.5f, 0.5f),   // Bottom left vertex

            // Top face
            new Vector3(-0.5f, 0.5f, 0.5f),  // Top left vertex
            new Vector3(0.5f, 0.5f, 0.5f),   // Top right vertex
            new Vector3(0.5f, 0.5f, -0.5f),  // Bottom right vertex
            new Vector3(-0.5f, 0.5f, -0.5f),  // Bottom left vertex

            // Bottom face
            new Vector3(-0.5f, -0.5f, 0.5f),  // Top left vertex
            new Vector3(0.5f, -0.5f, 0.5f),   // Top right vertex
            new Vector3(0.5f, -0.5f, -0.5f),  // Bottom right vertex
            new Vector3(-0.5f, -0.5f, -0.5f),  // Bottom left vertex
        };

        List<Vector2> texCoords = new List<Vector2>()
        {
            //Face 0
            new Vector2(0f, 1f), //top left uv
            new Vector2(1f, 1f), //top right uv
            new Vector2(1f, 0f), //bottom right uv
            new Vector2(0f, 0f), //bottom left uv

            //Face 1
            new Vector2(0f, 1f), //top left uv
            new Vector2(1f, 1f), //top right uv
            new Vector2(1f, 0f), //bottom right uv
            new Vector2(0f, 0f), //bottom left uv

            //Face 2
            new Vector2(0f, 1f), //top left uv
            new Vector2(1f, 1f), //top right uv
            new Vector2(1f, 0f), //bottom right uv
            new Vector2(0f, 0f), //bottom left uv

            //Face 3
            new Vector2(0f, 1f), //top left uv
            new Vector2(1f, 1f), //top right uv
            new Vector2(1f, 0f), //bottom right uv
            new Vector2(0f, 0f), //bottom left uv

            //Face 4
            new Vector2(0f, 1f), //top left uv
            new Vector2(1f, 1f), //top right uv
            new Vector2(1f, 0f), //bottom right uv
            new Vector2(0f, 0f), //bottom left uv

            //Face 5
            new Vector2(0f, 1f), //top left uv
            new Vector2(1f, 1f), //top right uv
            new Vector2(1f, 0f), //bottom right uv
            new Vector2(0f, 0f), //bottom left uv
        };

        uint[] indices =
        {
            //first face
            0, 1, 2,
            2, 3, 0,

            4, 5, 6,
            6, 7, 4,

            8, 9, 10,
            10, 11, 8,

            12, 13, 14,
            14, 15, 12,

            16, 17, 18,
            18, 18, 16,

            20, 21, 22,
            22, 23, 20
        };

        //Render pipeline vars
        int vao;
        int defaultShaderProgram;
        int vbo;
        int ebo;
        int textureID;
        int textureVbo;

        //transformation vars
        float yRot = 0f;

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
                vertices.Count * Vector3.SizeInBytes,
                vertices.ToArray(),
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
                texCoords.Count * Vector2.SizeInBytes,
                texCoords.ToArray(),
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
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            //Create the shader program
            defaultShaderProgram = GL.CreateProgram();

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, LoadShaderSource("default.vert"));
            GL.CompileShader(vertexShader);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, LoadShaderSource("default.frag"));
            GL.CompileShader(fragmentShader);

            GL.AttachShader(defaultShaderProgram, vertexShader);
            GL.AttachShader(defaultShaderProgram, fragmentShader);

            GL.LinkProgram(defaultShaderProgram);

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

            //Enable Depth Tests (Render closer objects on top of others)
            GL.Enable(EnableCap.DepthTest);


            camera = new Camera(width, height, Vector3.Zero);
            CursorState = CursorState.Grabbed;
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            GL.DeleteBuffer(vbo);
            GL.DeleteVertexArray(vao);
            GL.DeleteBuffer(ebo);
            GL.DeleteProgram(defaultShaderProgram);

            GL.DeleteTexture(textureID);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(0.6f, 0.3f, 1f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //Draw triangle
            GL.UseProgram(defaultShaderProgram);

            GL.BindTexture(TextureTarget.Texture2D, textureID);

            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);



            // transformation matricies
            Matrix4 model = Matrix4.Identity;
            Matrix4 view = camera.GetViewMatrix();
            Matrix4 projection = camera.GetProjectionMatrix();

            model = Matrix4.CreateRotationY(yRot);
            yRot += 0.001f;

            Matrix4 translation = Matrix4.CreateTranslation(0f, 0f, -3f);

            model *= translation;

            int modelLocation = GL.GetUniformLocation(defaultShaderProgram, "model");
            int viewLocation = GL.GetUniformLocation(defaultShaderProgram, "view");
            int projectionLocation = GL.GetUniformLocation(defaultShaderProgram, "projection");

            GL.UniformMatrix4(modelLocation, true, ref model);
            GL.UniformMatrix4(viewLocation, true, ref view);
            GL.UniformMatrix4(projectionLocation, true, ref projection);

            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

            //GL.DrawArrays(PrimitiveType.Triangles, 0, 4);

            Context.SwapBuffers();

            base.OnRenderFrame(args);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            MouseState mouseInput = MouseState;
            KeyboardState keyboardInput = KeyboardState;

            camera.Update(keyboardInput, mouseInput, args);
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
