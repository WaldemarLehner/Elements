using System.Collections.Generic;

namespace ComputergrafikSpiel.Model
{
    public interface ICollider
    {
        ICollidable CollidableParent { get; }

        bool DidCollideWith(ICollider otherCollider);

        float MinimalDistanceTo(ICollider otherCollider);

        IReadOnlyCollection<ICollider> GetCollisions();
    }
}