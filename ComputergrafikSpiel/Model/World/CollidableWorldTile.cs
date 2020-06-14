﻿using System.Collections.Generic;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using ComputergrafikSpiel.Model.World.Interfaces;
using OpenTK;
using OpenTK.Graphics;

namespace ComputergrafikSpiel.Model.World
{
    internal class CollidableWorldTile : IWorldTileCollidable
    {
        internal CollidableWorldTile(float size, (int x, int y) gridPos, TileDefinitions.Type type, TileDefinitions.SurroundingTiles neighbors)
        {
            this.Size = size;
            this.GridPosition = gridPos;
            this.Position = (new Vector2(this.GridPosition.x, this.GridPosition.y) * this.Size) + (Vector2.One * this.Size / 2f);
            this.TileType = type;
            ColliderLayer.Layer collisionLayer = TileHelper.GetCollisionLayers(type);
            this.Collider = new RectangleOffsetCollider(this, Vector2.Zero, size / 2f, collisionLayer);

            this.TileTexture = new WorldTileTextureLoader().LoadTexture(type);
            TileDefinitions.TextureSubType[] textureLayers = TileHelper.HasTileTypeTrims(type) ? TileHelper.GetTexturesTransitionable(neighbors) : new TileDefinitions.TextureSubType[] { TileDefinitions.TextureSubType.Filled };

            foreach (var tex in textureLayers)
            {
                var coord = TexturePointerCalculationHelper.GetCurrentTextureCoordinates(this.TileTexture, (int)tex);
                if (coord == null)
                {
                    continue;
                }

                this.Coordinates.Add((TextureCoordinates)coord);
            }
        }

        public (int x, int y) GridPosition { get; }

        public TileDefinitions.Type TileType { get; }

        public (IEnumerable<TextureCoordinates>, ITileTexture) Texture => (this.Coordinates, this.TileTexture);

        public IEnumerable<(Color4 color, Vector2[] vertices)> DebugData { get; } = new List<(Color4 color, Vector2[] vertices)>();

        public Vector2 Position { get; }

        public float Rotation { get; } = 0f;

        public Vector2 RotationAnker => this.Position;

        public Vector2 Scale => this.Size / 2 * Vector2.One;

        public ICollider Collider { get; }

        public float Size { get; }

        public ITileTexture TileTexture { get; }

        ITexture IRenderable.Texture => this.TileTexture;

        private List<TextureCoordinates> Coordinates { get; } = new List<TextureCoordinates>();
    }
}