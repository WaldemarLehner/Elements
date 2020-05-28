using System;
using System.IO;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using Image = SixLabors.ImageSharp.Image;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture
{
    internal class TextureLoader : ITextureLoader
    {

        // In Welchem Pfad befinde ich mich gerade (von User zu User verschieden -> user/bin)
        private string currentDirectory;

        public ITexture LoadTexture(string thisIsTheName)
        {
            //Endung .png wird hinzugefügt
            thisIsTheName = thisIsTheName + ".png";

            //Pfad wird erstellt (an currentDirectory wird der Pfad zur Textur hinzugefügt)
            this.currentDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            currentDirectory = currentDirectory.Replace("bin\" , "source\Repos\ComputergrafikSpiel\ComputergrafikSpiel\model\EntitySettings\texture\Images\" + thisIsTheName);
            // Leerer Pfad bzw. (String)
            if (string.IsNullOrEmpty(currentDirectory))
            {
                throw new ArgumentNullException(nameof(currentDirectory), "Error: Path is empty.");
            }

            // Pfad schon richtig angegeben
            if (File.Exists(currentDirectory) == false)
            {
                throw new FileNotFoundException(nameof(currentDirectory), "Error: File does not exist or wrong Path");
            }

            Image currentTexture = Image.Load(currentDirectory);
            //public readonly Texture texture = new Texture(currentDirectory); Wie genau funktioniert das nun mit der Rückgabe

            return this.texture;
        }
    }
}
