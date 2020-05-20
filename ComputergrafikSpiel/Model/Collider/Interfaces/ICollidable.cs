using ComputergrafikSpiel.Model.EntitySettings.Interfaces;

namespace ComputergrafikSpiel.Model.Collider.Interfaces
{
    internal interface ICollidable : ITransformable
    {
        ICollider Collider { get; }
    }
}