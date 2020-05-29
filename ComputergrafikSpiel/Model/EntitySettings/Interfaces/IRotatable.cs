using OpenTK;

namespace ComputergrafikSpiel.Model.EntitySettings.Interfaces
{
    public interface IRotatable
    {
        float Rotation { get; }

        Vector2 RotationAnker { get; }
    }
}