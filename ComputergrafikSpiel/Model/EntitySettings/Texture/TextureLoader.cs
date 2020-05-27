using System.IO;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using Image = SixLabors.ImageSharp.Image;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture
{
    internal class TextureLoader : ITextureLoader
    {
        public ITexture LoadTexture(string pathOrIdentifier)
        {
            // Fehler abfangen (leerer Pfad bzw. falscher)
            if (string.IsNullOrEmpty(pathOrIdentifier))
            {
                throw new EmptyIdentifierException(nameof(pathOrIdentifier), "The file path cannot be empty!");
            }

            if (pathOrIdentifier.StartsWith(" ") || pathOrIdentifier.EndsWith(" "))
            {
                throw new SpaceBarIdentifierException(nameof(pathOrIdentifier), "Don`t use spacebars at the beginning or at the end!");
            }

            // Pfad schon richtig angegeben
            if (pathOrIdentifier.StartsWith(@"C:\All Users\Source\Repos\ComputergrafikSpiel"))
            {
                if (File.Exists(pathOrIdentifier) == false)
                {
                    throw new PathDoesNotExistException(nameof(pathOrIdentifier), "The File does not exist!");
                }

                Image currentTexture = Image.Load(pathOrIdentifier);

                return ITexture;
            }

            // Nur Texturename angegeben BSP: character.png
            else
            {
                throw new InvalidPathException(nameof(pathOrIdentifier), "Wrong path to file.");
            }
        }
    }
}
