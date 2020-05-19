using OpenTK;

namespace ComputergrafikSpiel.Model
{
    internal interface IRotatable
    {
        float Rotation { get; }

        Vector2 RotationAnker { get; }
    }
}