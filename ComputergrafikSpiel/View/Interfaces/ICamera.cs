using OpenTK;

namespace ComputergrafikSpiel.View.Interfaces
{
    internal interface ICamera
    {
        float Top { get; }

        float Bottom { get; }

        float Left { get; }

        float Right { get; }

        Vector2 TopLeft { get; }

        Vector2 TopRight { get; }

        Vector2 BottomRight { get; }

        Vector2 BottomLeft { get; }

        float AspectRatio { get; }

        (Vector2 TL, Vector2 TR, Vector2 BL, Vector2 BR) CameraBounds { get; }

        bool CanPointBeSeenByCamera(Vector2 point);

        void Update(float top, float bottom, float left, float right);

        void DrawRectangle(Rectangle vertices, (Vector2 TL, Vector2 TR, Vector2 BR, Vector2 BL) texCoords, (int width, int height) screen);
    }
}