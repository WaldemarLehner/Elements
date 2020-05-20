using OpenTK;

namespace ComputergrafikSpiel.Model.EntitySettings.Interfaces
{
    internal interface IRotatable
    {
        float Rotation { get; }

        Vector2 RotationAnker { get; }
    }
}