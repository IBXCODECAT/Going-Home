using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace Hexoidra.Graphics
{
    internal class Texture
    {
        internal int ID;

        internal Texture(string filepath)
        {
            ID = GL.GenTexture();

            //Activate the fist texture in the unit
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, ID);

            //Texture params
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            //mipmap params - does not use bluring due to pixelated vibes
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);


            //load image
            StbImage.stbi_set_flip_vertically_on_load(1); //StbImage reads in opisisate direction to opengl by default
            ImageResult texture = ImageResult.FromStream(File.OpenRead($"./Textures/{filepath}"), ColorComponents.RedGreenBlueAlpha);

            //Give openGL texture data
            GL.TexImage2D(
                TextureTarget.Texture2D,
                0, PixelInternalFormat.Rgba,
                texture.Width,
                texture.Height,
                0,
                PixelFormat.Rgba,
                PixelType.UnsignedByte,
                texture.Data
            );

            Unbind();            
        }

        /// <summary>
        /// Binds this Texture
        /// </summary>
        internal void Bind() { GL.BindTexture(TextureTarget.Texture2D, ID); }

        /// <summary>
        /// Unbinds this Texture
        /// </summary>
        internal void Unbind() { GL.BindTexture(TextureTarget.Texture2D, 0); }

        /// <summary>
        /// Disposes of this Texture
        /// </summary>
        internal void Dispose() { GL.DeleteTexture(ID); }
    }
}
