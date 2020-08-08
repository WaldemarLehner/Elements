using System.Collections.Generic;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using OpenTK;
using OpenTK.Graphics;

namespace ComputergrafikSpiel.Model.World
{
    /// <summary>
    /// A World Tile without a Collider. For Tiles with Colliders, use a Tile that inherits from <see cref="World.Interfaces.IWorldTileCollidable"/>.
    /// </summary>
    public class WorldTile : IWorldTile
    {
        internal WorldTile(float size, (int x, int y) gridPos, TileDefinitions.Type type, TileDefinitions.SurroundingTiles neighbors, bool allowObstacle = true)
        {
            this.Size = size;
            this.GridPosition = gridPos;
            this.Position = (new Vector2(this.GridPosition.x, this.GridPosition.y) * this.Size) + (Vector2.One * this.Size / 2f);
            this.TileType = type;
            this.Spawnmask = SpawnMask.Mask.AllowNPC | SpawnMask.Mask.AllowInteractable | (allowObstacle ? SpawnMask.Mask.AllowObstacle : SpawnMask.Mask.Disallow);
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

        public IEnumerable<(Color4 color, Vector2[] vertices)> DebugData => null;

        public Vector2 Position { get; }

        public float Rotation => 0f;

        public SpawnMask.Mask Spawnmask { get; set; }

        public Vector2 RotationAnker => this.Position;

        public Vector2 Scale { get => Vector2.One * this.Size / 2f; set => _ = value; }

        public TileDefinitions.Type TileType { get; }

        public (IEnumerable<TextureCoordinates>, ITileTexture) Texture => (this.Coordinates, this.TileTexture);

        ITexture IRenderable.Texture => this.TileTexture;

        private float Size { get; }

        private List<TextureCoordinates> Coordinates { get; } = new List<TextureCoordinates>();

        private ITileTexture TileTexture { get; }
    }
}
