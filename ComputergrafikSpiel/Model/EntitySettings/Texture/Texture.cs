using System;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture
{
    internal class Texture : ITexture
    {
        private readonly string path;

        public Texture(string path) //Noch auf Fehler Prüfen, wurde bei path eine Exception geworfen?
        {
            this.path = path;
        }

        public int Width => throw new NotImplementedException();

        public int Height => throw new NotImplementedException();

        public string FilePath => throw new NotImplementedException();

        public Tuple<Vector2, Vector2, Vector2, Vector2> TextureCoordinates => throw new NotImplementedException();

        public void Update(float dtime)
        {
            throw new NotImplementedException();
        }
    }
}
