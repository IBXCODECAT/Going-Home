using Hexoidra.Graphics;
using Hexoidra.World;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Hexoidra
{
    internal class Game : GameWindow
    {
        Camera camera;

        Shader shader;

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

            shader = new Shader("default.vert", "default.frag");

            //Enable Depth Tests (Render closer objects on top of others)
            GL.Enable(EnableCap.DepthTest);

            //Enable backface mesh culling
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            camera = new Camera(width, height, Vector3.Zero);
            CursorState = CursorState.Grabbed;
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            shader.Dispose();

            ChunkManager.DisposeAll();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(0f, 0.7f, 1f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // transformation matricies
            Matrix4 model = Matrix4.Identity;
            Matrix4 view = camera.GetViewMatrix();
            Matrix4 projection = camera.GetProjectionMatrix();

            int modelLocation = GL.GetUniformLocation(shader.ID, "model");
            int viewLocation = GL.GetUniformLocation(shader.ID, "view");
            int projectionLocation = GL.GetUniformLocation(shader.ID, "projection");

            GL.UniformMatrix4(modelLocation, true, ref model);
            GL.UniformMatrix4(viewLocation, true, ref view);
            GL.UniformMatrix4(projectionLocation, true, ref projection);

            ChunkManager.RenderAll(shader);

            Context.SwapBuffers();

            base.OnRenderFrame(args);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {

            MouseState mouseInput = MouseState;
            KeyboardState keyboardInput = KeyboardState;

            ChunkManager.BuildAll();

            base.OnUpdateFrame(args);

            camera.Update(keyboardInput, mouseInput, args);
        }
    }
}
