using System.Collections.Generic;
using System.Linq;

namespace ComputergrafikSpiel.Model.World
{
    internal static class TileHelper
    {
        internal static bool IsWalkable(WorldTile.Type type)
        {
            if (type == WorldTile.Type.Dirt || type == WorldTile.Type.Grass)
            {
                return true;
            }

            return false;
        }

        internal static WorldTile.TextureSubType[] GetTexturesTransitionable(SurroundingTiles n)
        {
            n.RemoveUnneccesary();
            if (n.Count == 0)
            {
                return new WorldTile.TextureSubType[] { WorldTile.TextureSubType.Filled, WorldTile.TextureSubType.NarrowSingle };
            }

            if (n.Count == 8)
            {
                return new WorldTile.TextureSubType[] { WorldTile.TextureSubType.Filled };
            }

            List<WorldTile.TextureSubType> textures = new List<WorldTile.TextureSubType>() { WorldTile.TextureSubType.Filled };

            // Check TopRight
            if (!n.Top && !n.Right)
            {
                textures.Add(WorldTile.TextureSubType.EdgeCornerTopRight);
            }
            else if (n.Top && !n.TopRight && n.Right)
            {
                textures.Add(WorldTile.TextureSubType.InvertCornerTopRight);
            }

            // Check TopLeft
            if (!n.Top && !n.Left)
            {
                textures.Add(WorldTile.TextureSubType.EdgeCornerTopLeft);
            }
            else if (n.Top && n.Left && !n.TopLeft)
            {
                textures.Add(WorldTile.TextureSubType.InvertCornerTopLeft);
            }

            // Check BottomRight
            if (!n.Bottom && !n.Right)
            {
                textures.Add(WorldTile.TextureSubType.EdgeCornerBottomRight);
            }
            else if (n.Bottom && !n.BottomRight && n.Right)
            {
                textures.Add(WorldTile.TextureSubType.InvertCornerBottomRight);
            }

            // Check BottomLeft
            if (!n.Bottom && !n.Left)
            {
                textures.Add(WorldTile.TextureSubType.EdgeCornerBottomLeft);
            }
            else if (n.Bottom && n.Left && !n.BottomLeft)
            {
                textures.Add(WorldTile.TextureSubType.InvertCornerBottomLeft);
            }

            // Check Edges
            if (!n.Top && n.Bottom)
            {
                textures.Add(WorldTile.TextureSubType.EdgeTop);
            }

            if (!n.Bottom && n.Top)
            {
                textures.Add(WorldTile.TextureSubType.EdgeBottom);
            }

            if (!n.Left && n.Right)
            {
                textures.Add(WorldTile.TextureSubType.EdgeLeft);
            }

            if (!n.Right && n.Left)
            {
                textures.Add(WorldTile.TextureSubType.EdgeRight);
            }

            // Check Narrow Variants
            if (n.Count == 1)
            {
                if (n.Right)
                {
                    textures.Add(WorldTile.TextureSubType.NarrowCapEndLeft);
                }
                else if (n.Left)
                {
                    textures.Add(WorldTile.TextureSubType.NarrowCapEndRight);
                }
                else if (n.Top)
                {
                    textures.Add(WorldTile.TextureSubType.NarrowCapEndBottom);
                }
                else if (n.Bottom)
                {
                    textures.Add(WorldTile.TextureSubType.NarrowCapEndTop);
                }
            }
            else if (n.Count == 2)
            {
                if (n.Right && n.Left)
                {
                    textures.Add(WorldTile.TextureSubType.NarrowHorizontal);
                }
                else if (n.Top && n.Bottom)
                {
                    textures.Add(WorldTile.TextureSubType.NarrowVertical);
                }
            }

            return textures.ToArray();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1101:Prefix local calls with this", Justification = "ommitting this helps visiblilty")]
        internal class SurroundingTiles
        {
            internal bool Top { get; set; } = false;

            internal bool Left { get; set; } = false;

            internal bool Bottom { get; set; } = false;

            internal bool Right { get; set; } = false;

            internal bool TopLeft { get; set; } = false;

            internal bool TopRight { get; set; } = false;

            internal bool BottomLeft { get; set; } = false;

            internal bool BottomRight { get; set; } = false;

            internal int Count
            {
                get
                {
                    bool[] enumerable = { Top, Bottom, Left, Right, TopLeft, TopRight, BottomLeft, BottomRight };
                    return (from x in enumerable where x select x).Count();
                }
            }

            internal bool TopRow => Top && TopLeft && TopRight;

            internal bool BottomRow => Bottom && BottomRight && BottomLeft;

            internal bool LeftColumn => Left && TopLeft && BottomLeft;

            internal bool RightColumn => Right && TopRight && BottomRight;

            internal void RemoveUnneccesary()
            {
                if (this.TopLeft && !this.Left && !this.Top)
                {
                    this.TopLeft = false;
                }

                if (this.TopRight && !this.Right && !this.Top)
                {
                    this.TopRight = false;
                }

                if (this.BottomRight && !this.Bottom && !this.Right)
                {
                    this.BottomRight = false;
                }

                if (this.BottomLeft && !this.Bottom && !this.Left)
                {
                    this.BottomLeft = false;
                }
            }
        }
    }
}
