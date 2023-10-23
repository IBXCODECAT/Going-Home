using Hexoidra.Data;
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
        private const float FOV = 45;

        private float screenWidth;
        private float screenHeight;

        private float sensitivity = 180f;

        private Vector3 position;

        private Vector3 up = Vector3.UnitY;
        private Vector3 relativeForward = -Vector3.UnitZ;
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
            PlayerInfo.playerPosition = position;
            PlayerInfo.playerCoordinates = (Vector3i)position;

            return Matrix4.LookAt(
                position, //Eye position
                position + relativeForward, //Target (front)
                up //Up Vector
            );
        }

        public Matrix4 GetProjectionMatrix()
        {
            return Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(FOV), //FOV
                screenWidth / screenHeight, //Aspect
                0.1f, //Near clip
                10000f //Far clip
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

            relativeForward.X = MathF.Cos(MathHelper.DegreesToRadians(pitch)) * MathF.Cos(MathHelper.DegreesToRadians(yaw));
            relativeForward.Y = MathF.Sin(MathHelper.DegreesToRadians(pitch));
            relativeForward.Z = MathF.Cos(MathHelper.DegreesToRadians(pitch)) * MathF.Sin(MathHelper.DegreesToRadians(yaw));

            relativeForward = Vector3.Normalize(relativeForward);

            //Cross multiply vectors
            right = Vector3.Normalize(Vector3.Cross(relativeForward, Vector3.UnitY));
            up = Vector3.Normalize(Vector3.Cross(right, relativeForward));
        }

        public void InputController(KeyboardState keyboardInput, MouseState mouseInput, FrameEventArgs e)
        {
            //Move forward (relative forward)
            if(keyboardInput.IsKeyDown(Keys.W))
            {
                position += new Vector3(relativeForward.X, 0f, relativeForward.Z) * SPEED * (float)e.Time;
            }
            
            //Move left (relative left)
            if (keyboardInput.IsKeyDown(Keys.A))
            {
                position -= right * SPEED * (float)e.Time;
            }

            //Move back (relative back)
            if (keyboardInput.IsKeyDown(Keys.S))
            {
                position -= new Vector3(relativeForward.X, 0f, relativeForward.Z) * SPEED * (float)e.Time;
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
