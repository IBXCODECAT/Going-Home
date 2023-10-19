using OpenTK.Graphics.GL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;

namespace Hexoidra.Graphics
{
    internal class VertexArrayObject
    {
        internal int ID;

        internal VertexArrayObject()
        {
            ID = GL.GenVertexArray();
            Bind();
        }

        internal void LinkVertexBufferObject(int location, int size, VertexBufferObject vbo)
        {
            Bind();
            vbo.Bind();
            GL.VertexAttribPointer(location, size, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(location);
            Unbind();
        }

        /// <summary>
        /// Binds this VertexArrayObject
        /// </summary>
        internal void Bind() { GL.BindVertexArray(ID); }

        /// <summary>
        /// Unbinds this VertexArrayObject
        /// </summary>
        internal void Unbind() { GL.BindVertexArray(0); }

        /// <summary>
        /// Disposes of this VertexArrayObject
        /// </summary>
        internal void Dispose() { GL.DeleteVertexArray(ID); }
        
    }
}
