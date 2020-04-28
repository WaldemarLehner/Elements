using OpenTK;

namespace ComputergrafikSpiel.Model
{
    public interface IRenderable
    {
        Vector2 Position { get; }

        Vector2 Scale { get; }

        float Rotation { get; }

        Vector2 RotationAnker { get; }

        ITexture Texture { get; }
    }
}