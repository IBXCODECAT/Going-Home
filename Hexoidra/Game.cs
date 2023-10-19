using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Hexoidra
{
    internal class Game : GameWindow
    {

        float[] verticies =
        {
            0f, 0.5f, 0f, //top vertex
            -0.5f, -0.5f, 0f, //bottom left
            0.5f, -0.5f, 0f //botom right
        };

        //Render pipeline vars
        int vao;
        int shaderProgram;

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

            vao = GL.GenVertexArray();

            int vbo = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, verticies.Length * sizeof(float), verticies, BufferUsageHint.StaticDraw);

            //bind the vao
            GL.BindVertexArray(vao);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.EnableVertexArrayAttrib(vao, 0);


            GL.BindBuffer(BufferTarget.ArrayBuffer, 0); //unbinding the vbo
            GL.BindVertexArray(0); //Unbind the vao

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
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            GL.DeleteVertexArray(vao);
            GL.DeleteProgram(shaderProgram);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(0.6f, 0.3f, 1f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            //Draw triangle
            GL.UseProgram(shaderProgram);
            GL.BindVertexArray(vao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

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
            catch(Exception ex)
            {
                Console.WriteLine("Failed to load shader source file: " + ex.Message);
            }

            return shaderSource;
        }
    }
}
