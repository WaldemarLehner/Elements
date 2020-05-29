using ComputergrafikSpiel.Model.EntitySettings.Interfaces;

namespace ComputergrafikSpiel.Model.Collider.Interfaces
{
    public interface ICollidable : ITransformable
    {
        ICollider Collider { get; }
    }
}