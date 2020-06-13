using System;
using System.Collections.Generic;
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
        internal WorldTile(float size, (int x, int y) gridPos, Type type, TileHelper.SurroundingTiles neighbors)
        {
            this.Size = size;
            this.GridPosition = gridPos;
            this.Position = (new Vector2(this.GridPosition.x, this.GridPosition.y) * this.Size) + (Vector2.One * this.Size / 2f);
            this.TileType = type;
            if (type == Type.Dirt)
            {
                var textureTypes = TileHelper.GetTexturesTransitionable(neighbors);


            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "Names self-explanatory")]
        public enum Type
        {
            Water,
            Grass,
            Dirt,
            Wall,
            WallTrim,
            Error = 255,
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "Names self-explanatory")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1028:Code should not contain trailing whitespace", Justification = "easier seperation.")]
        public enum TextureSubType : int
        {
            Filled = (5 * 1) + 1,
            NarrowSingle = (5 * 3) + 3,
            NarrowCapEndTop = (5 * 0) + 3,
            NarrowCapEndLeft = (5 * 3) + 0,
            NarrowCapEndRight = (5 * 3) + 2,
            NarrowCapEndBottom = (5 * 2) + 3,
            NarrowVertical = (5 * 1) + 3,
            NarrowHorizontal = (5 * 3) + 1,
            EdgeTop = (5 * 0) + 1,
            EdgeBottom = (5 * 2) + 1,
            EdgeLeft = (5 * 1) + 0,
            EdgeRight = (5 * 1) + 2,
            EdgeCornerTopLeft = (5 * 0) + 0,
            EdgeCornerTopRight = (5 * 0) + 2,
            EdgeCornerBottomLeft = (5 * 2) + 0, 
            EdgeCornerBottomRight = (5 * 2) + 2,
            InvertCornerTopRight = (5 * 1) + 4,
            InvertCornerTopLeft = (5 * 2) + 4,
            InvertCornerBottomRight = (5 * 0) + 4,
            InvertCornerBottomLeft = (5 * 3) + 4,
        }

        public (int x, int y) GridPosition { get; }


        public IEnumerable<(Color4 color, Vector2[] vertices)> DebugData => new List<(Color4 color, Vector2[] vertices)>();

        public Vector2 Position { get; }

        public float Rotation => 0f;

        public Vector2 RotationAnker => this.Position;

        public Vector2 Scale => Vector2.One * this.Size / 2f;

        public Type TileType { get; }

        public (IEnumerable<(Vector2 TL, Vector2 TR, Vector2 BR, Vector2 BL)>, ITileTexture) Texture => (this.Coordinates, this.TileTexture);

        private float Size { get; }

        private List<(Vector2 TL, Vector2 TR, Vector2 BR, Vector2 BL)> Coordinates { get; } = new List<(Vector2 TL, Vector2 TR, Vector2 BR, Vector2 BL)>();

        private ITileTexture TileTexture { get; }
    }
}
