using System;
using System.IO;
using System.Reflection;
using ComputergrafikSpiel.Model.EntitySettings.Texture.ConstructorHelpers;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using SixLabors.ImageSharp;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture
{
    internal class TextureLoader : ITextureLoader
    {
        public ITexture LoadTexture(string name)
        {
            // Leerer Texture name
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name), "Error: Name can not be empty.");
            }

            // Endung .png wird hinzugefügt
            name += ".png";

            var pathToTexture = Path.Combine("./Content/Images/", name);
            Image currentTexture = Image.Load(pathToTexture);
            TextureContructor constructor;

            try
            {
                constructor = new TextureContructor(currentTexture.Width, currentTexture.Height, pathToTexture);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: im Kontruktor {0}", e);
                return null;
            }

            Texture texture = new Texture(constructor);
            return texture;
        }
    }
}
