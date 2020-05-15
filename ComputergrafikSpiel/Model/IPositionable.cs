using OpenTK;

namespace ComputergrafikSpiel.Model
{
    internal interface IPositionable
    {
        Vector2 Position { get; }

        Vector2 Scale { get; }

        float Rotation { get; }

        Vector2 RotationAnker { get; }
    }
}
