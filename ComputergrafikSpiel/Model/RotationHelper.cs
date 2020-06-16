using System;
using OpenTK;

namespace ComputergrafikSpiel.Model
{
    public static class RotationHelper
    {
        public static float GetRotationBetweenTwoVectorsRadians(Vector2 vec1, Vector2 vec2) => (float)(Math.Atan2(vec2.Y, vec2.X) - Math.Atan2(vec1.Y, vec1.X));
    }
}
