using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.View.Renderer.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.View.Interfaces
{
    public interface ICamera
    {
        IRenderer Parent { get; }

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

        bool AttachRenderer(IRenderer renderer);

        bool CanPointBeSeenByCamera(Vector2 point);

        void Update(float top, float bottom, float left, float right);

        Vector2 WorldToNDC(Vector2 pointWorldSpace);

        Vector2 NDCToWorld(Vector2 pointNDC);

        void DrawRectangle(Rectangle vertices, TextureCoordinates texCoords, (int width, int height) screen);
    }
}