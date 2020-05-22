﻿using System;
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

            var tileWidth = this.Width / this.XRows;
            var tileHeight = this.Height / this.YRows;

            // Tuple: TopLeft, TopRight, BottomRight, BottomLeft coordinated range from 0 to 1.
            float bottom = ((this.YRows - tile.Item2 - 1) / this.YRows) * tileHeight; // Flipping Y axis
            float top = bottom + tileHeight;
            float left = tile.Item1 / this.XRows;
            float right = left + tileWidth;

            Vector2 tl = new Vector2(top, left);
            Vector2 tr = new Vector2(top, right);
            Vector2 bl = new Vector2(bottom, left);
            Vector2 br = new Vector2(bottom, right);

            return new Tuple<Vector2, Vector2, Vector2, Vector2>(tl, tr, br, bl);
        }
    }
}