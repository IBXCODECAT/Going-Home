using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Hexoidra.Graphics;
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

        List<uint> indices = new List<uint>()
        {
            // first face
            // top triangle
            0, 1, 2,
            // bottom triangle
            2, 3, 0,

            4, 5, 6,
            6, 7, 4,

            8, 9, 10,
            10, 11, 8,

            12, 13, 14,
            14, 15, 12,

            16, 17, 18,
            18, 19, 16,

            20, 21, 22,
            22, 23, 20
        };

        //Render pipeline vars
        VertexArrayObject vao;
        IndexBufferObject ibo;
        Shader shader;
        Texture texture;


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

            vao = new VertexArrayObject();

            VertexBufferObject vbo = new VertexBufferObject(vertices);
            vao.LinkVertexBufferObject(0, 3, vbo);

            VertexBufferObject uvVBO = new VertexBufferObject(texCoords);
            vao.LinkVertexBufferObject(1, 2, uvVBO);

            ibo = new IndexBufferObject(indices);

            shader = new Shader("default.vert", "default.frag");
            texture = new Texture("tex.png");

            //Enable Depth Tests (Render closer objects on top of others)
            GL.Enable(EnableCap.DepthTest);


            camera = new Camera(width, height, Vector3.Zero);
            CursorState = CursorState.Grabbed;
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            vao.Dispose();
            ibo.Dispose();
            texture.Dispose();
            shader.Dispose();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(0.6f, 0.3f, 1f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            shader.Bind();
            vao.Bind();
            ibo.Bind();
            texture.Bind();

            // transformation matricies
            Matrix4 model = Matrix4.Identity;
            Matrix4 view = camera.GetViewMatrix();
            Matrix4 projection = camera.GetProjectionMatrix();

            model = Matrix4.CreateRotationY(yRot);
            yRot += 0.001f;

            Matrix4 translation = Matrix4.CreateTranslation(0f, 0f, -3f);

            model *= translation;

            int modelLocation = GL.GetUniformLocation(shader.ID, "model");
            int viewLocation = GL.GetUniformLocation(shader.ID, "view");
            int projectionLocation = GL.GetUniformLocation(shader.ID, "projection");

            GL.UniformMatrix4(modelLocation, true, ref model);
            GL.UniformMatrix4(viewLocation, true, ref view);
            GL.UniformMatrix4(projectionLocation, true, ref projection);

            GL.DrawElements(PrimitiveType.Triangles, indices.Count, DrawElementsType.UnsignedInt, 0);

            

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
    }
}
