using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexoidra
{
    internal class Camera
    {
        private const float SPEED = 8f;
        private const float FOV = 60;

        private float screenWidth;
        private float screenHeight;

        private float sensitivity = 180f;

        public Vector3 position;

        private Vector3 up = Vector3.UnitY;
        private Vector3 front = -Vector3.UnitZ;
        private Vector3 right = Vector3.UnitX;
        
        public Camera(float screenWidth, float screenHeight, Vector3 position)
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;

            this.position = position;
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(
                position, //Eye position
                position + front, //Target (front)
                up //Up Vector
            );
        }

        public Matrix4 GetProjectionMatrix()
        {
            return Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(FOV), //FOV
                screenWidth / screenHeight, //Aspect
                0.1f, //Near clip
                100f //Far clip
            );
        }

        private void UpdateVectors()
        {

        }

        public void InputController(KeyboardState keyboardInput, MouseState mouseInput, FrameEventArgs e)
        {
            if(keyboardInput.IsKeyDown(Keys.W))
            {
                position += front * SPEED * (float)e.Time;
            }
            
            if (keyboardInput.IsKeyDown(Keys.A))
            {
                position -= right * SPEED * (float)e.Time;
            }

            if (keyboardInput.IsKeyDown(Keys.S))
            {
                position -= front * SPEED* (float)e.Time;
            }

            if (keyboardInput.IsKeyDown(Keys.D))
            {
                position += right * SPEED * (float)e.Time;
            }
        }   

        public void Update(KeyboardState keyboardState, MouseState mouseInput, FrameEventArgs e)
        {
            InputController(keyboardState, mouseInput, e);
        }
    }
}
