using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model.EntitySettings.Texture.ConstructorHelpers.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture
{
    internal class FontTexture : IMappedTileFont
    {
        private char currentKey = '\0';

        internal FontTexture(ITextureContructor textureContructor, ITileTextureContructor tileTextureContructor, ICollection<(char key, int index)> mappings)
        {
            _ = textureContructor ?? throw new ArgumentNullException(nameof(textureContructor));
            _ = tileTextureContructor ?? throw new ArgumentNullException(nameof(tileTextureContructor));
            _ = mappings ?? throw new ArgumentNullException(nameof(mappings));
            this.Width = textureContructor.Width;
            this.Height = textureContructor.Height;
            this.FilePath = textureContructor.FilePath;
            this.XRows = tileTextureContructor.XRows;
            this.YRows = tileTextureContructor.YRows;
            this.MappedPositions = new Dictionary<char, int>(mappings.Count);

            foreach (var (key, index) in mappings)
            {
                this.MappedPositions[key] = index;
            }
        }

        public Dictionary<char, int> MappedPositions { get; }

        public int XRows { get; private set; }

        public int YRows { get; private set; }

        public (int x, int y) Pointer { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public string FilePath { get; private set; }

        public TextureCoordinates TextureCoordinates => TexturePointerCalculationHelper.GetCurrentTextureCoordinates(this, this.currentKey) ?? TextureCoordinates.Error;

        public (int x, int y) GetTileOfKey(char key)
        {
            if (!this.MappedPositions.ContainsKey(key))
            {
                return (-1, -1);
            }

            return this.GetTileIndexFromIndex(this.MappedPositions[key]);
        }

        public void Update(float dTime)
        {
        }

        public void UpdatePointer(char key)
        {
            if (!this.MappedPositions.ContainsKey(key))
            {
                throw new ArgumentOutOfRangeException(nameof(key), "Given key is not part of the Texture's Set");
            }

            this.currentKey = key;
        }

        public TextureCoordinates GetTexCoordsOfIndex(int index) => TexturePointerCalculationHelper.GetCurrentTextureCoordinates(this, index) ?? TextureCoordinates.Error;

        private (int x, int y) GetTileIndexFromIndex(int i)
        {
            int x = i % this.XRows;
            int y = i / this.XRows;
            return (x, y);
        }
    }
}
