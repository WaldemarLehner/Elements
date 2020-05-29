using System;
using System.IO;
using System.Linq;
using ComputergrafikSpiel.Model.EntitySettings.Texture.ConstructorHelpers;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using Image = SixLabors.ImageSharp.Image;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture
{
    internal class TextureLoader : ITextureLoader
    {
        // In Welchem Pfad befinde ich mich gerade (von User zu User verschieden -> user/bin)
        private string individualPathLocationToProject;
        private string pathToTexture;

        public ITexture LoadTexture(string name)
        {
            // Leerer Texture name
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name), "Error: Name can not be empty.");
            }

            // Endung .png wird hinzugefügt
            name = name + ".png";

            // Pfad wird erstellt (an individualPathLocationToProject wird der Pfad zur Textur hinzugefügt)
            this.individualPathLocationToProject = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            this.pathToTexture = Path.Combine(this.individualPathLocationToProject, "/Content/Images/", name);

            // Prüfung ob Datei existiert
            if (!File.Exists(this.pathToTexture))
            {
                throw new FileNotFoundException(nameof(this.individualPathLocationToProject), "Error: File does not exist or wrong Path");
            }

            Image currentTexture = Image.Load(this.pathToTexture);
            TextureContructor constructor;

            try
            {
                constructor = new TextureContructor(currentTexture.Width, currentTexture.Height, this.pathToTexture);
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
