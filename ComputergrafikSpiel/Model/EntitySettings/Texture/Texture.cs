using System;
using ComputergrafikSpiel.Model.EntitySettings.Texture.ConstructorHelpers;
using ComputergrafikSpiel.Model.EntitySettings.Texture.ConstructorHelpers.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture
{
    internal class Texture : ITexture
    {
        public Texture(TextureContructor contructor)
        {
            this.Width = contructor.Width;
            this.Height = contructor.Height;
            this.FilePath = contructor.FilePath;
        }

        public int Width { get; set; }

        public int Height { get; set; }

        public string FilePath { get; set; }

        public Tuple<Vector2, Vector2, Vector2, Vector2> TextureCoordinates => throw new NotImplementedException();

        int ITexture.Width { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        int ITexture.Height { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        string ITexture.FilePath { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Update(float dtime)
        {
            throw new NotImplementedException();
        }
    }
}
