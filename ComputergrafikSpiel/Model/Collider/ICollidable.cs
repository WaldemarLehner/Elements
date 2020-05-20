using ComputergrafikSpiel.Model.EntitySettings;

namespace ComputergrafikSpiel.Model.Collider
{
    internal interface ICollidable : ITransformable
    {
        ICollider Collider { get; }
    }
}