using System;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.Collider
{
    internal class CircleOffsetCollider : ICollider
    {
        private Vector2 offset;

        internal CircleOffsetCollider(ICollidable parent, Vector2 offset, float radius, ColliderLayer.Layer layer)
        {
            this.CollidableParent = parent ?? throw new ArgumentNullException(nameof(parent));
            if (radius <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(radius), "Expect positive radius greater Zero");
            }

            this.Radius = radius;
            this.offset = offset;
            this.Layer = layer;
        }

        public ColliderLayer.Layer Layer { get; }

        public float Radius { get; }

        public ICollidable CollidableParent { get; }

        public float MaximumDistanceFromPosition => this.Radius;

        public Vector2 Position => this.CollidableParent.Position + this.offset;

        public float Rotation => 0;

        public Vector2 RotationAnker => this.Position;

        public bool DidCollideWith(ICollider otherCollider) => CollisionDetectionHelper.DidCollideWith(this, otherCollider);

        public float MinimalDistanceTo(ICollider otherCollider) => CollisionDetectionHelper.MinDistanceBetween(this, otherCollider);
    }
}
