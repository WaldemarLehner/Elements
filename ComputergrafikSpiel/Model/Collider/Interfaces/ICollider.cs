using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using OpenTK;
using OpenTK.Graphics;

namespace ComputergrafikSpiel.Model.Collider.Interfaces
{
    public interface ICollider : IPositionable
    {
        ICollidable CollidableParent { get; }

        float MaximumDistanceFromPosition { get; }

        ColliderLayer.Layer Layer { get; }

        (Color4 color, Vector2[] verts) DebugData { get; }

        bool DidCollideWith(ICollider otherCollider);

        float MinimalDistanceTo(ICollider otherCollider);
    }
}