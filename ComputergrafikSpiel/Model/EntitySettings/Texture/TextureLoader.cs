using System;
using System.IO;
using ComputergrafikSpiel.Model.EntitySettings.Texture.ConstructorHelpers;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using Image = SixLabors.ImageSharp.Image;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture
{
    internal class TextureLoader : ITextureLoader
    {
        // In Welchem Pfad befinde ich mich gerade (von User zu User verschieden -> user/bin)
        private string currentDirectory;
        private string pathWithName;

        public ITexture LoadTexture(string name)
        {
            // Leerer Texture name
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name), "Error: Name can not be empty.");
            }

            // Endung .png wird hinzugefügt
            name = name + ".png";

            // Pfad wird erstellt (an currentDirectory wird der Pfad zur Textur hinzugefügt)
            currentDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            pathWithName = this.currentDirectory.Replace("bin/", "source/Repos/ComputergrafikSpiel/ComputergrafikSpiel/model/EntitySettings/texture/Images/" + name);

            // Pfad schon richtig angegeben
            if (!File.Exists(currentDirectory))
            {
                throw new FileNotFoundException(nameof(currentDirectory), "Error: File does not exist or wrong Path");
            }

            Image currentTexture = Image.Load(pathWithName);
            TextureContructor constructor;

            try
            {
                constructor = new TextureContructor(currentTexture.Width, currentTexture.Height, pathWithName);
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
