using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture
{
    internal static class TexturePointerCalculationHelper
    {
        internal static TextureCoordinates? GetCurrentTextureCoordinates(ITileTexture tex, int index)
        {
            var tileNull = GetPointer(tex, index);

            if (tileNull == null)
            {
                return null;
            }

            var (x, y) = tileNull ?? (-1, -1); // Null Operation will never be reached

            // Tuple: TopLeft, TopRight, BottomRight, BottomLeft coordinated range from 0 to 1.
            float left = x / tex.XRows;
            float right = left + (1 / (float)tex.XRows);

            float bottom = (tex.YRows - y - 1) / (float)tex.YRows;
            float top = bottom + (1 / (float)tex.YRows);

            Vector2 tl = new Vector2(left, top);
            Vector2 tr = new Vector2(right, top);
            Vector2 bl = new Vector2(left, bottom);
            Vector2 br = new Vector2(right, bottom);

            return new TextureCoordinates(tl, tr, br, bl);
        }

        internal static (int x, int y)? GetPointer(ITileTexture tex, int index)
        {
            var currentIndex = index;
            if (currentIndex == -1)
            {
                return null;
            }

            int x = currentIndex % tex.YRows;
            int y = currentIndex / tex.YRows;
            return (x, y);
        }
    }
}
