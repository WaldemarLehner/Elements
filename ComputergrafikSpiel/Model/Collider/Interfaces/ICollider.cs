using ComputergrafikSpiel.Model.EntitySettings.Interfaces;

namespace ComputergrafikSpiel.Model.Collider.Interfaces
{
    internal interface ICollider : ITransformable
    {
        ICollidable CollidableParent { get; }

        float MaximumDistanceFromPosition { get; }

        bool DidCollideWith(ICollider otherCollider);

        float MinimalDistanceTo(ICollider otherCollider);
    }
}