using System.Linq;

namespace ComputergrafikSpiel.Model.World
{
    public static class TileDefinitions
    {
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1101:Prefix local calls with this", Justification = "ommitting this helps visiblilty")]
        internal class SurroundingTiles
        {
            internal SurroundingTiles()
            {

            }

            internal SurroundingTiles(bool top, bool bottom, bool left, bool right, bool topleft, bool topright, bool bottomright, bool bottomleft)
            {
                Top = top;
                Bottom = bottom;
                Left = left;
                Right = right;
                TopLeft = topleft;
                TopRight = topright;
                BottomLeft = bottomleft;
                BottomRight = bottomright;
            }

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
