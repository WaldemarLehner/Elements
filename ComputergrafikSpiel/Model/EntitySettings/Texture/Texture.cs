using System;
using System.Numerics;
using ComputergrafikSpiel.Model.EntitySettings.Texture.ConstructorHelpers;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using OpenTK;
using Vector2 = OpenTK.Vector2;

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

        public int Width { get; }

        public int Height { get; }

        public string FilePath { get; }

        private Vector2 tl = new Vector2(0, 1);
        private Vector2 tr = new Vector2(1, 1);
        private Vector2 br = new Vector2(1, 0);
        private Vector2 bl = new Vector2(0, 0);

        public Tuple<Vector2, Vector2, Vector2, Vector2> TextureCoordinates => Tuple.Create(this.tl, this.tr, this.br, this.bl);

        public void Update(float dtime)
        {
        }
    }
}
