using OpenTK.Graphics.OpenGL4;

namespace Hexoidra.Graphics
{
    internal class Shader
    {
        internal int ID;

        internal Shader(string vertexShaderFilepath, string fragmentShaderFilepath)
        {
            //Create the shader program
            ID = GL.CreateProgram();

            //Create the vertex shader, add code, and compile
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, LoadShaderSource(vertexShaderFilepath));
            GL.CompileShader(vertexShader);

            //Create the fragment shader, add code, and compile
            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, LoadShaderSource(fragmentShaderFilepath));
            GL.CompileShader(fragmentShader);

            //Attatch and link shader program to OpenGL
            GL.AttachShader(ID, vertexShader);
            GL.AttachShader(ID, fragmentShader);
            GL.LinkProgram(ID);

            //Delete the shaders
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        /// <summary>
        /// Method to load a shader file and return it's contents as a string
        /// </summary>
        /// <param name="filePath">The path of the file</param>
        /// <returns>Contents of the file as a string</returns>
        internal static string LoadShaderSource(string filePath)
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

        /// <summary>
        /// Binds this Shader
        /// </summary>
        internal void Bind() { GL.UseProgram(ID); }

        /// <summary>
        /// Unbinds this Shader
        /// </summary>
        internal void Unbind() { GL.UseProgram(0); }

        /// <summary>
        /// Disposes of this Shader
        /// </summary>
        internal void Dispose() { GL.DeleteShader(ID); }
    }
}
