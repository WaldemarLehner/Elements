using System;
using ComputergrafikSpiel.Model.EntitySettings.Texture.ConstructorHelpers.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture
{
    internal class TileTexture : ITileTexture
    {
        internal TileTexture(ITextureContructor tex, ITileTextureContructor tileTex)
        {
            _ = tex ?? throw new ArgumentNullException(nameof(tex));
            _ = tileTex ?? throw new ArgumentNullException(nameof(tileTex));
            this.XRows = tileTex.XRows;
            this.YRows = tileTex.YRows;
            this.Width = tex.Width;
            this.Height = tex.Height;
            this.FilePath = tex.FilePath;
        }

        public int XRows { get; }

        public int YRows { get; }

        public (int x, int y) Pointer { get; }

        public int Width { get; }

        public int Height { get; }

        public string FilePath { get; }

        public TextureCoordinates TextureCoordinates { get; }

        public TextureCoordinates GetTexCoordsOfIndex(int index) => TexturePointerCalculationHelper.GetCurrentTextureCoordinates(this, index) ?? TextureCoordinates.Error;

        public void Update(float dtime)
        {
        }
    }
}