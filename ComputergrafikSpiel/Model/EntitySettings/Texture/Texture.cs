using System;
using ComputergrafikSpiel.Model.EntitySettings.Texture.ConstructorHelpers.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture
{
    internal class Texture : ITexture
    {

        public Texture(ITextureContructor texture) // Noch auf Fehler Prüfen, wurde bei path eine Exception geworfen?
        {
            this.Width = texture.Width;
            this.Height = texture.Height;
            this.FilePath = texture.FilePath;
        }

        public int Width;

        public int Height;

        public string FilePath;

        public Tuple<Vector2, Vector2, Vector2, Vector2> TextureCoordinates => throw new NotImplementedException();

        public void Update(float dtime)
        {
            throw new NotImplementedException();
        }
    }
}
