using System;
using OpenTK;

namespace ComputergrafikSpiel.Model.Entity.Particles
{
    internal static class ParticleCalculationHelper
    {
        internal static Vector2 Deviate(this Vector2 vectorToRotate, float deviationInDegrees, Random rand)
        {
            var dev = deviationInDegrees * rand.Next(int.MinValue, int.MaxValue) / (float)int.MaxValue;
            var rotationInRadians = dev * Math.PI / 180;
            float newX = ((float)Math.Cos(rotationInRadians) * vectorToRotate.X) - ((float)Math.Sin(rotationInRadians) * vectorToRotate.Y);
            float newY = ((float)Math.Sin(rotationInRadians) * vectorToRotate.X) + ((float)Math.Cos(rotationInRadians) * vectorToRotate.Y);
            return new Vector2(newX, newY);
        }

        internal static float Deviate(this float value, float deviation, Random rand)
        {
            var dev = deviation * rand.Next(int.MinValue, int.MaxValue) / (float)int.MaxValue;
            return value + dev;
        }

        internal static float Clamp(this float value, float? min, float? max)
        {
            if (min != null)
            {
                if (value < min)
                {
                    return min ?? 0;
                }
            }

            if (max != null)
            {
                if (value > max)
                {
                    return max ?? 0;
                }
            }

            return value;
        }
    }
}
