using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model.EntitySettings.Texture.ConstructorHelpers.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture
{
    internal class FontTexture : IMappedTileFont
    {
        internal FontTexture(ITextureContructor textureContructor, ITileTextureContructor tileTextureContructor, ICollection<Tuple<char, int>> mappings)
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

            foreach (var entry in mappings)
            {
                this.MappedPositions[entry.Item1] = entry.Item2;
            }
        }

        public Dictionary<char, int> MappedPositions { get; }

        public int XRows { get; private set; }

        public int YRows { get; private set; }

        public Tuple<int, int> Pointer { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public string FilePath { get; private set; }

        public Tuple<Vector2, Vector2, Vector2, Vector2> TextureCoordinates => this.GetCurrentTextureCoordinates();

        public Tuple<int, int> GetTileOfKey(char key)
        {
            if (!this.MappedPositions.ContainsKey(key))
            {
                return null;
            }

            return this.GetTileIndexFromIndex(this.MappedPositions[key]);
        }

        public void Update(float dTime)
        {
        }

        public void UpdatePointer(char key)
        {
            this.Pointer = this.GetTileOfKey(key);
        }

        private Tuple<int, int> GetTileIndexFromIndex(int i)
        {
            int x = i % this.XRows;
            int y = i / this.XRows;
            return new Tuple<int, int>(x, y);
        }

        private Tuple<Vector2, Vector2, Vector2, Vector2> GetCurrentTextureCoordinates()
        {
            var tile = this.Pointer;

            // Tuple: TopLeft, TopRight, BottomRight, BottomLeft coordinated range from 0 to 1.
            float left = tile.Item1 / this.XRows;
            float right = left + (1 / (float)this.XRows);

            float bottom = (this.YRows - tile.Item2 - 1) / (float)this.YRows;
            float top = bottom + (1 / (float)this.YRows);

            Vector2 tl = new Vector2(left, top);
            Vector2 tr = new Vector2(right, top);
            Vector2 bl = new Vector2(left, bottom);
            Vector2 br = new Vector2(right, bottom);

            return new Tuple<Vector2, Vector2, Vector2, Vector2>(tl, tr, br, bl);
        }
    }
}
