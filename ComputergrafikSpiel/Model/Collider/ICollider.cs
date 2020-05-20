using ComputergrafikSpiel.Model.EntitySettings;

namespace ComputergrafikSpiel.Model.Collider
{
    internal interface ICollider : ITransformable
    {
        ICollidable CollidableParent { get; }

        float MaximumDistanceFromPosition { get; }

        bool DidCollideWith(ICollider otherCollider);

        float MinimalDistanceTo(ICollider otherCollider);
    }
}