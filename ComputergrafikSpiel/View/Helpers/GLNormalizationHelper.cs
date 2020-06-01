using ComputergrafikSpiel.View.Exceptions;
using OpenTK;
using System;

namespace ComputergrafikSpiel.View.Helpers
{
    [Obsolete]
    internal static class GLNormalizationHelper
    {
        internal static float NormalizeGL(this float value, int boundary)
        {
            if (boundary <= 0)
            {
                throw new ArgumentNotPositiveIntegerGreaterZeroException(nameof(boundary));
            }

            return ((value / boundary) * 2) - 1;
        }

        internal static Vector2 NormalizeGL(this Vector2 value, int boundaryX, int boundaryY)
        {
            if (boundaryX <= 0)
            {
                throw new ArgumentNotPositiveIntegerGreaterZeroException(nameof(boundaryX));
            }

            if (boundaryY <= 0)
            {
                throw new ArgumentNotPositiveIntegerGreaterZeroException(nameof(boundaryY));
            }

            float x = value.X.NormalizeGL(boundaryX);
            float y = value.Y.NormalizeGL(boundaryY);
            return new Vector2(x, y);
        }
    }
}
