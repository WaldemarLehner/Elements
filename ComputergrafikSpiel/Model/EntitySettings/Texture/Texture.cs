using System;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture
{
    internal class Texture : ITexture
    {
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
