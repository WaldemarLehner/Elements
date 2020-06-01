using ComputergrafikSpiel.Model.EntitySettings.Interfaces;

namespace ComputergrafikSpiel.Model.Collider.Interfaces
{
    public interface ICollider : IPositionable
    {
        ICollidable CollidableParent { get; }

        float MaximumDistanceFromPosition { get; }

        bool DidCollideWith(ICollider otherCollider);

        float MinimalDistanceTo(ICollider otherCollider);
    }
}