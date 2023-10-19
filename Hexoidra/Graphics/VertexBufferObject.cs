using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;

namespace Hexoidra.Graphics
{
    /// <summary>
    /// Vertex Buffer Object
    /// </summary>
    internal class VertexBufferObject
    {
        internal int ID;

        public VertexBufferObject(List<Vector3> data)
        {
            ID = GL.GenBuffer();
            Bind();
            GL.BufferData(BufferTarget.ArrayBuffer, data.Count * Vector3.SizeInBytes, data.ToArray(), BufferUsageHint.StaticDraw);

        }

        public VertexBufferObject(List<Vector2> data)
        {
            ID = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, ID);
            GL.BufferData(BufferTarget.ArrayBuffer, data.Count * Vector2.SizeInBytes, data.ToArray(), BufferUsageHint.StaticDraw);
        }

        /// <summary>
        /// Binds this VertexBufferObject
        /// </summary>
        internal void Bind() { GL.BindBuffer(BufferTarget.ArrayBuffer, ID); }

        /// <summary>
        /// Unbinds this VertexBufferObject
        /// </summary>
        internal void UnBind() { GL.BindBuffer(BufferTarget.ArrayBuffer, 0); }

        /// <summary>
        /// Disposes of this VertexBufferObject
        /// </summary>
        internal void Dispose() { GL.DeleteBuffer(ID); }
    }
}
