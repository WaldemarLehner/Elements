using System;
using System.IO;
using ComputergrafikSpiel.Model.EntitySettings.Texture.ConstructorHelpers.Interfaces;
using ComputergrafikSpiel.View.Exceptions;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture.ConstructorHelpers
{
    /// <summary>
    /// Parameters required to create an object of <see cref="EntitySettings.Texture.Interfaces.ITexture"/>.
    /// </summary>
    internal class TextureContructor : ITextureContructor
    {
        internal TextureContructor(int width, int height, string filepath)
        {
            if (width <= 0)
            {
                throw new ArgumentNotPositiveIntegerGreaterZeroException(nameof(width));
            }

            if (height <= 0)
            {
                throw new ArgumentNotPositiveIntegerGreaterZeroException(nameof(height));
            }

            _ = filepath ?? throw new ArgumentNullException(nameof(filepath));

            if (!File.Exists(filepath))
            {
                throw new FileNotFoundException("No file can be found at " + filepath);
            }

            this.Width = width;
            this.Height = height;
            this.FilePath = filepath;
        }

        public int Width { get; }

        public int Height { get; }

        public string FilePath { get; }
    }
}
