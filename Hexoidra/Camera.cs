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
        private const float SPEED = 5f;
        private const float FOV = 45;

        private float screenWidth;
        private float screenHeight;

        private float sensitivity = 120f;

        public Vector3 position;

        private Vector3 up = Vector3.UnitY;
        private Vector3 front = -Vector3.UnitZ;
        private Vector3 right = Vector3.UnitX;

        private float pitch;
        private float yaw = -90f; //Set to -90 to point in -Z direction by default (forward)

        private bool firstmove = true;

        public Vector2 lastPos;
        
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
            if(pitch > 89)
            {
                pitch = 89;
            }

            if(pitch < -89)
            {
                pitch = -89;
            }

            front.X = MathF.Cos(MathHelper.DegreesToRadians(pitch)) * MathF.Cos(MathHelper.DegreesToRadians(yaw));
            front.Y = MathF.Sin(MathHelper.DegreesToRadians(pitch));
            front.Z = MathF.Cos(MathHelper.DegreesToRadians(pitch)) * MathF.Sin(MathHelper.DegreesToRadians(yaw));

            front = Vector3.Normalize(front);

            //Cross multiply vectors
            right = Vector3.Normalize(Vector3.Cross(front, Vector3.UnitY));
            up = Vector3.Normalize(Vector3.Cross(right, front));
        }

        public void InputController(KeyboardState keyboardInput, MouseState mouseInput, FrameEventArgs e)
        {
            //Move forward (relative forward)
            if(keyboardInput.IsKeyDown(Keys.W))
            {
                position += front * SPEED * (float)e.Time;
            }
            
            //Move left (relative left)
            if (keyboardInput.IsKeyDown(Keys.A))
            {
                position -= right * SPEED * (float)e.Time;
            }

            //Move back (relative back)
            if (keyboardInput.IsKeyDown(Keys.S))
            {
                position -= front * SPEED* (float)e.Time;
            }

            //Move right (relative right)
            if (keyboardInput.IsKeyDown(Keys.D))
            {
                position += right * SPEED * (float)e.Time;
            }

            //Move up (global Y)
            if(keyboardInput.IsKeyDown(Keys.Space))
            {
                position.Y += SPEED * (float)e.Time;
            }

            //Move down (gloabl Y)
            if(keyboardInput.IsKeyDown(Keys.LeftShift))
            {
                position.Y -= SPEED * (float)e.Time;
            }

            //Mouse is always one step behind in order to calculate movement distance :)
            if(firstmove)
            {
                lastPos = new Vector2(mouseInput.X, mouseInput.Y);
                firstmove = false;
            }
            else
            {
                //Calculate mouse deltas
                var deltaX = mouseInput.X - lastPos.X;
                var deltaY = mouseInput.Y - lastPos.Y;

                //Assign new position to last position
                lastPos = new Vector2(mouseInput.X, mouseInput.Y);

                //Set pitch and yaw
                pitch -= deltaY * sensitivity * (float)e.Time;
                yaw += deltaX * sensitivity * (float)e.Time;
            }

            UpdateVectors();
        }   

        public void Update(KeyboardState keyboardState, MouseState mouseInput, FrameEventArgs e)
        {
            InputController(keyboardState, mouseInput, e);
        }
    }
}
