namespace ComputergrafikSpiel.Model
{
    internal interface ICollider : IPositionable
    {
        ICollidable CollidableParent { get; }

        float MaximumDistanceFromPosition { get; }

        bool DidCollideWith(ICollider otherCollider);

        float MinimalDistanceTo(ICollider otherCollider);
    }
}