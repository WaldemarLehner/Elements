using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1501:Statement should not be on a single line", Justification = "single line helps visibility")]
        internal static WorldTile.SubType GetSubType(WorldTile.Type tiletype, SurroundingTiles surrounding)
        {
            var count = surrounding.Count;

            if (count == 8)
            {
                return WorldTile.SubType.Center;
            }

            if (count == 0)
            {
                return WorldTile.SubType.NarrowSingle;
            }

            // Only on Diagonals => No Connections => NarrowSingle.
            if (!surrounding.OnlyCross)
            {
                return WorldTile.SubType.NarrowSingle;
            }

            // Has to be Narrow End Cap or Narrow Single if on diagonal
            if (count == 1)
            {
                // Is on main cross => End Cap
                if (surrounding.T) { return WorldTile.SubType.NarrowEndTop; }
                if (surrounding.B) { return WorldTile.SubType.NarrowEndBottom; }
                if (surrounding.L) { return WorldTile.SubType.NarrowEndLeft; }
                if (surrounding.R) { return WorldTile.SubType.NarrowEndRight; }
            }

            if (count == 2)
            {
                // No Diagonal Tiles => Naror Pipe or L - Bend.
                if (count == surrounding.CrossCount)
                {
                    if (surrounding.NarrowVertical) { return WorldTile.SubType.Top_Bottom; }
                    if (surrounding.NarrowHorizontal) { return WorldTile.SubType.Left_Right; }

                    if (surrounding.NarrowBendBottomLeft) { return WorldTile.SubType.Bottom_Left; }
                    if (surrounding.NarrowBendBottomRight) { return WorldTile.SubType.Bottom_Right; }
                    if (surrounding.NarrowBendTopLeft) { return WorldTile.SubType.Top_Left; }
                    if (surrounding.NarrowBendTopRight) { return WorldTile.SubType.Top_Right; }

                    if (surrounding.InvertBendTopLeft) { return WorldTile.SubType.Top_TopLeft; }
                    if (surrounding.InvertBendTopRight) { return WorldTile.SubType.Top_TopRight; }
                    if (surrounding.InvertBendLeftBottom) { return WorldTile.SubType.Left_BottomLeft; }
                    if (surrounding.InvertBendLeftTop) { return WorldTile.SubType.Left_TopLeft; }
                    if (surrounding.InvertBendRightBottom) { return WorldTile.SubType.Right_BottomRight; }
                    if (surrounding.InvertBendRightTop) { return WorldTile.SubType.Right_RightTop; }
                    if (surrounding.InvertBendBottomLeft) { return WorldTile.SubType.Bottom_BottomLeft; }
                    if (surrounding.InvertBendBottomRight) { return WorldTile.SubType.Bottom_BottomRight; }
                }
            }

            if (count == 3)
            {
                if (surrounding.CrossCount == 2)
                {
                    // if true => Chance for a corner
                    if (surrounding.CornerBL) { return WorldTile.SubType.CornerBottomLeft; }
                    if (surrounding.CornerBR) { return WorldTile.SubType.CornerBottomRight; }
                    if (surrounding.CornerTL) { return WorldTile.SubType.CornerTopLeft; }
                    if (surrounding.CornerTR) { return WorldTile.SubType.CornerTopRight; }
                }

                if (surrounding.CrossCount == 1)
                {
                    if (surrounding.NarrowHorizontalTop) { return WorldTile.SubType.TopRow; }
                    if (surrounding.NarrowHorizontalBottom) { return WorldTile.SubType.BottomRow; }
                    if (surrounding.NarrowVerticalLeft) { return WorldTile.SubType.LeftColumn; }
                    if (surrounding.NarrowVerticalRight) { return WorldTile.SubType.RightColumn; }
                }

                if (surrounding.CrossCount == 3)
                {
                    // T-Shape.
                    if (!surrounding.T) { return WorldTile.SubType.Left_Bottom_Right; }
                    if (!surrounding.B) { return WorldTile.SubType.Left_Top_Right; }
                    if (!surrounding.L) { return WorldTile.SubType.Top_Right_Bottom; }
                    if (!surrounding.R) { return WorldTile.SubType.Top_Left_Bottom; }
                }
            }

            if (count == 4)
            {
                if (surrounding.Cross) { return WorldTile.SubType.NarrowCenter; }



            throw new Exception("Should not get here");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1101:Prefix local calls with this", Justification = "Too much clutter")]
        public class SurroundingTiles
        {
            public bool T = false;
            public bool B = false;
            public bool L = false;
            public bool R = false;
            public bool TL = false;
            public bool TR = false;
            public bool BR = false;
            public bool BL = false;

            public bool Any => T || B || L || R || TL || TR || BR || BL;

            public bool All => T && B && L && R && TL && TR && BR && BL;

            public int Count => GetSum(T, B, L, R, TL, TR, BL, BR);

            public int CrossCount => GetSum(T, B, L, R);

            public int HorizontalTopCount => GetSum(T, TL, TR);

            public int HorizontalBottomCount => GetSum(B, BL, BR);

            public int VerticalLeftCount => GetSum(L, TL, BL);

            public int VerticalRightCount => GetSum(R, TR, BR);





            // Only...
            public bool OnlyCross => Cross && !AnyDiagonal;

            public bool OnlyNarrowHorizontal => NarrowHorizontal && !T && !B && !TL && !TR && !BL && !BR;

            public bool OnlyNarrowVertical => NarrowVertical && !L && !R && !TL && !TR && !BL && !BR;


            


            // Default...
            public bool NarrowVertical => T && B;

            public bool NarrowHorizontal => L && R;

            public bool Cross => NarrowHorizontal && NarrowVertical;

            public bool NarrowVerticalLeft => L && TL && BL;

            public bool NarrowVerticalRight => R && TR && BR;

            public bool NarrowHorizontalTop => T && TL && TR;

            public bool NarrowHorizontalBottom => B && BL && BR;

            public bool NarrowBendTopLeft => T && L;

            public bool NarrowBendTopRight => T && R;

            public bool NarrowBendBottomRight => B && R;

            public bool NarrowBendBottomLeft => B && L;

            public bool InvertBendTopLeft => T && TL;

            public bool InvertBendBottomLeft => B && BL;

            public bool InvertBendTopRight => T && TR;

            public bool InvertBendBottomRight => B && BR;

            public bool InvertBendLeftBottom => L && BL;

            public bool InvertBendLeftTop => L && TL;

            public bool InvertBendRightBottom => R && BR;

            public bool InvertBendRightTop => R && TR;

            public bool CornerTR => T && TR && R;

            public bool CornerBR => B && BR && R;

            public bool CornerBL => B && BL && L;

            public bool CornerTL => T && TL && L;

            // Any...
            public bool AnyCross => CrossCount > 0;

            public bool AnyDiagonal => CrossCount != Count;



            public void RemoveIrrelevant()
            {
                if (!AnyDiagonal)
                {
                    return;
                }

                if (TL && !T && !L)
                {
                    TL = false;
                }

                if (TR && !T && !R)
                {
                    TR = false;
                }

                if (BL && !B && !L)
                {
                    BL = false;
                }

                if (BR && !B && !R)
                {
                    BR = false;
                }

            }

            private int GetSum(params bool[] values)
            {
                return values.Where(e => e).Count();
            }

        }
    }
}
