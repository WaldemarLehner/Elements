using OpenTK;

namespace ComputergrafikSpiel.Model.EntitySettings
{
    internal interface IRotatable
    {
        float Rotation { get; }

        Vector2 RotationAnker { get; }
    }
}