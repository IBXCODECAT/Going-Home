using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hexoidra.Graphics
{
    internal class IndexBufferObject
    {
        internal int ID;

        internal IndexBufferObject(List<uint> data)
        {
            ID = GL.GenBuffer();
            Bind();
            GL.BufferData(BufferTarget.ElementArrayBuffer, data.Count * sizeof(uint), data.ToArray(), BufferUsageHint.StaticDraw);
        }

        /// <summary>
        /// Binds this IndexBufferObject
        /// </summary>
        internal void Bind() { GL.BindBuffer(BufferTarget.ElementArrayBuffer, ID); }

        /// <summary>
        /// Unbinds this IndexBufferObject
        /// </summary>
        internal void Unbind() { GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0); }

        /// <summary>
        /// Disposes of this IndexBufferObject
        /// </summary>
        internal void Dispose() { GL.DeleteBuffer(ID); }
    }
}
