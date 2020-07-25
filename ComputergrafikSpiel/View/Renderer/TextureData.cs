using System;
using System.IO;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using ComputergrafikSpiel.View.Helpers;
using OpenTK.Graphics.OpenGL;

namespace ComputergrafikSpiel.View.Renderer
{
    /// <summary>
    /// TextureData is a class that stores OpenTK-specific aspects of the Texture, such as handle and so on.
    /// </summary>
    public class TextureData
    {
        private readonly byte[] data;
        private readonly int handle;

        internal TextureData(ITexture texture, TextureWrapMode wrapMode = TextureWrapMode.MirroredRepeat)
        {
            this.ConstructorInputCheck(texture);
            this.data = ImageToByteHelper.ImageToByteArray(texture.FilePath, (texture.Width, texture.Height));
            this.handle = GL.GenTexture();
            this.Enable();
            this.CreateGLTexture(wrapMode, texture.Width, texture.Height);
            this.Disable();
        }

        internal void Enable()
        {
            GL.BindTexture(TextureTarget.Texture2D, this.handle);
        }

        internal void Disable()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        private void ConstructorInputCheck(ITexture texture)
        {
            _ = texture ?? throw new ArgumentNullException(nameof(texture), "Passed Texture is null");

            if (!File.Exists(texture.FilePath))
            {
                throw new FileNotFoundException(nameof(texture.FilePath), texture.FilePath);
            }
        }

        private void CreateGLTexture(TextureWrapMode wrapMode, int width, int height)
        {
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, this.data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear); // Wenn ein Pixel zw. 2 Texturen liegt wird mit Linearisierung daran angenähert
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest); // Erstellt den Pixel Look.

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)wrapMode);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)wrapMode);
        }
    }
}