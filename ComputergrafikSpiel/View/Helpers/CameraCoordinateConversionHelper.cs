using System;
using ComputergrafikSpiel.View.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.View.Helpers
{
    internal static class CameraCoordinateConversionHelper
    {
        public static Vector2 WorldToScreen(ICamera camera, Vector2 point)
        {
            float x = Normalize(point.X, camera.Left, camera.Right);
            float y = Normalize(point.Y, camera.Bottom, camera.Top);
            return new Vector2(x, y);
        }

        public static (float x, float y) CalculateAspectRatioMultiplier(float cameraAR, float screenAR)
        {
            var aspect = screenAR - cameraAR;

            // If the result is Positive, screen Ratio is greater than camera Ration.
            // This results in black borders left and right
            if (aspect > 0)
            {
                // y is guaranteed 1
                float x = 1 - ((screenAR - cameraAR) / screenAR);
                return (x, 1);
            }
            else if (aspect == 0)
            {
                // No black borders, as the ratios match
                return (1, 1);
            }
            else
            {
                // x is gauranteed 1
                float y = 1 - ((cameraAR - screenAR) / cameraAR);
                return (1, y);
            }
        }

        public static Vector2 WorldToNDC(Vector2 point, (float width, float height) multipliers, ICamera camera)
        {
            // All vertices are now mapped to x{0;1} , y{0;1} , representing what the camera can "see"
            Vector2 vec = WorldToScreen(camera, point);

            // Mapped x{-1;1} , y{-1;1}
            vec = (vec - (Vector2.One * .5f)) * 2f;
            vec.Y *= multipliers.height;
            vec.X *= multipliers.width;
            return vec;
        }

        public static Vector2 NDCToWorld(Vector2 point, (float width, float height) multipliers, ICamera camera)
        {
            var cameraSpace = new Vector2(point.X / multipliers.width, point.Y / multipliers.height);
            return ScreenToWorld(cameraSpace, camera);
        }

        private static Vector2 ScreenToWorld(Vector2 point, ICamera camera)
        {
            float x = camera.Left + (point.X * (camera.Right - camera.Left));
            float y = camera.Bottom + (point.Y * (camera.Top - camera.Bottom));
            return new Vector2(x, y);
        }

        private static float Normalize(float value, float lower, float upper)
        {
            if (lower == upper)
            {
                throw new ArgumentOutOfRangeException(nameof(lower) + " " + nameof(upper), "Lower and Upper may not be equal, as this will create a division by 0");
            }

            return (value - lower) / (upper - lower);
        }
    }
}
