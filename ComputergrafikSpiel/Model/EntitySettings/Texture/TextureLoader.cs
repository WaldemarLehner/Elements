using System;
using System.IO;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using Image = SixLabors.ImageSharp.Image;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture
{
    internal class TextureLoader : ITextureLoader
    {
        private readonly Texture texture = new Texture();

        public ITexture LoadTexture(string pathOrIdentifier)
        {
            // Leerer Pfad bzw. (String)
            if (string.IsNullOrEmpty(pathOrIdentifier))
            {
                throw new ArgumentNullException();
            }

            // Pfad schon richtig angegeben
            if (pathOrIdentifier.StartsWith(@"C:\All Users\Source\Repos\ComputergrafikSpiel"))
            {
                if (File.Exists(pathOrIdentifier) == false)
                {
                    throw new FileNotFoundException();
                }

                pathOrIdentifier = pathOrIdentifier.Trim();
                Image currentTexture = Image.Load(pathOrIdentifier);

                return this.texture;
            }

            // Wenn falscher Pfad eingegeben
            else
            {
                throw new FileNotFoundException();
            }
        }
    }
}
