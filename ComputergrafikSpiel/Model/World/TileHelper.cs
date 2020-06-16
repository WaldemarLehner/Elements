using System.Collections.Generic;
using ComputergrafikSpiel.Model.Collider;

namespace ComputergrafikSpiel.Model.World
{
    internal static class TileHelper
    {
        internal static bool IsWalkable(TileDefinitions.Type type)
        {
            if (type == TileDefinitions.Type.Dirt || type == TileDefinitions.Type.Grass)
            {
                return true;
            }

            return false;
        }

        internal static TileDefinitions.TextureSubType[] GetTexturesTransitionable(TileDefinitions.SurroundingTiles n)
        {
            n.RemoveUnneccesary();
            if (n.Count == 0)
            {
                return new TileDefinitions.TextureSubType[] { TileDefinitions.TextureSubType.Filled, TileDefinitions.TextureSubType.NarrowSingle };
            }

            if (n.Count == 8)
            {
                return new TileDefinitions.TextureSubType[] { TileDefinitions.TextureSubType.Filled };
            }

            List<TileDefinitions.TextureSubType> textures = new List<TileDefinitions.TextureSubType>() { TileDefinitions.TextureSubType.Filled };

            // Check TopRight
            if (!n.Top && !n.Right)
            {
                textures.Add(TileDefinitions.TextureSubType.EdgeCornerTopRight);
            }
            else if (n.Top && !n.TopRight && n.Right)
            {
                textures.Add(TileDefinitions.TextureSubType.InvertCornerTopRight);
            }

            // Check TopLeft
            if (!n.Top && !n.Left)
            {
                textures.Add(TileDefinitions.TextureSubType.EdgeCornerTopLeft);
            }
            else if (n.Top && n.Left && !n.TopLeft)
            {
                textures.Add(TileDefinitions.TextureSubType.InvertCornerTopLeft);
            }

            // Check BottomRight
            if (!n.Bottom && !n.Right)
            {
                textures.Add(TileDefinitions.TextureSubType.EdgeCornerBottomRight);
            }
            else if (n.Bottom && !n.BottomRight && n.Right)
            {
                textures.Add(TileDefinitions.TextureSubType.InvertCornerBottomRight);
            }

            // Check BottomLeft
            if (!n.Bottom && !n.Left)
            {
                textures.Add(TileDefinitions.TextureSubType.EdgeCornerBottomLeft);
            }
            else if (n.Bottom && n.Left && !n.BottomLeft)
            {
                textures.Add(TileDefinitions.TextureSubType.InvertCornerBottomLeft);
            }

            // Check Edges
            if (!n.Top && n.Bottom)
            {
                textures.Add(TileDefinitions.TextureSubType.EdgeTop);
            }

            if (!n.Bottom && n.Top)
            {
                textures.Add(TileDefinitions.TextureSubType.EdgeBottom);
            }

            if (!n.Left && n.Right)
            {
                textures.Add(TileDefinitions.TextureSubType.EdgeLeft);
            }

            if (!n.Right && n.Left)
            {
                textures.Add(TileDefinitions.TextureSubType.EdgeRight);
            }

            // Check Narrow Variants
            if (n.Count == 1)
            {
                if (n.Right)
                {
                    textures.Add(TileDefinitions.TextureSubType.NarrowCapEndLeft);
                }
                else if (n.Left)
                {
                    textures.Add(TileDefinitions.TextureSubType.NarrowCapEndRight);
                }
                else if (n.Top)
                {
                    textures.Add(TileDefinitions.TextureSubType.NarrowCapEndBottom);
                }
                else if (n.Bottom)
                {
                    textures.Add(TileDefinitions.TextureSubType.NarrowCapEndTop);
                }
            }
            else if (n.Count == 2)
            {
                if (n.Right && n.Left)
                {
                    textures.Add(TileDefinitions.TextureSubType.NarrowHorizontal);
                }
                else if (n.Top && n.Bottom)
                {
                    textures.Add(TileDefinitions.TextureSubType.NarrowVertical);
                }
            }

            return textures.ToArray();
        }

        internal static ColliderLayer.Layer GetCollisionLayers(TileDefinitions.Type type)
        {
            ColliderLayer.Layer layers = ColliderLayer.Layer.Empty;
            if (!IsWalkable(type))
            {
                // Players and Enemies will collide with this Tile
                layers |= ColliderLayer.Layer.Player | ColliderLayer.Layer.Enemy;
            }

            if (!CanBulletsPassThrough(type))
            {
                layers |= ColliderLayer.Layer.Bullet;
            }

            return layers;
        }

        internal static bool HasTileTypeTrims(TileDefinitions.Type type)
        {
            switch (type)
            {
                case TileDefinitions.Type.Dirt:
                case TileDefinitions.Type.WallTrim:
                case TileDefinitions.Type.Water:
                    return true;

                case TileDefinitions.Type.Wall:
                case TileDefinitions.Type.Grass:
                default:
                    return false;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:Code should not contain multiple whitespace in a row", Justification = "better visibility")]
        internal static TileDefinitions.SurroundingTiles GetSurroundingTile(TileDefinitions.Type[,] tileResult, (int x, int y) index, (int x, int y) maxLen)
        {
            int x = index.x;
            int y = index.y;
            maxLen.x--;
            maxLen.y--;
            var current = tileResult[x, y];

            var top =           (y == 0)                            ? true : current == tileResult[x,      y - 1];
            var bottom =        (y == maxLen.y)                     ? true : current == tileResult[x,      y + 1];
            var left =          (x == 0)                            ? true : current == tileResult[x - 1,  y];
            var right =         (x == maxLen.x)                     ? true : current == tileResult[x + 1,  y];
            var topright =      (x == maxLen.x || y == 0)           ? true : current == tileResult[x + 1,  y - 1];
            var topleft =       (x == 0 || y == 0)                  ? true : current == tileResult[x - 1,  y - 1];
            var bottomright =   (x == maxLen.x || y == maxLen.y)    ? true : current == tileResult[x + 1,  y + 1];
            var bottomleft =    (x == 0 || y == maxLen.y)           ? true : current == tileResult[x - 1,  y + 1];

            return new TileDefinitions.SurroundingTiles(top, bottom, left, right, topleft, topright, bottomright, bottomleft);
        }

        private static bool CanBulletsPassThrough(TileDefinitions.Type type)
        {
            switch (type)
            {
                case TileDefinitions.Type.Dirt:
                case TileDefinitions.Type.Error:
                case TileDefinitions.Type.Water:
                    return true;
                case TileDefinitions.Type.Wall:
                case TileDefinitions.Type.WallTrim:
                default:
                    return false;
            }
        }
    }
}
