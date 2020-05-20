using System;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.Collider
{
    internal class CircleOffsetCollider : ICollider
    {
        private Vector2 offset;

        internal CircleOffsetCollider(ICollidable parent, Vector2 offset, float radius)
        {
            _ = parent ?? throw new ArgumentNullException(nameof(parent));
            if (radius <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(radius), "Expect positive radius greater Zero");
            }

            this.Radius = radius;
            this.CollidableParent = parent;
            this.offset = offset;
        }

        public float Radius { get; }

        public ICollidable CollidableParent { get; }

        public float MaximumDistanceFromPosition => this.Radius;

        public Vector2 Position => this.CollidableParent.Position + this.offset;

        public Vector2 Scale => throw new NotImplementedException();

        public float Rotation => 0;

        public Vector2 RotationAnker => this.Position;

        public bool DidCollideWith(ICollider otherCollider)
        {
            // General Check using maximum Distance from Position
            float minimalDistance = this.MinimalDistanceTo(otherCollider);
            if (minimalDistance > 0)
            {
                // No Collision possible
                return false;
            }

            if (otherCollider is CircleOffsetCollider)
            {
                return true; // Radius = MaximumDistanceFromPosition -> Previus check stated that there might be a collision, but for Circles there is a collision for sure
            }

            throw new NotImplementedException();
        }

        public float MinimalDistanceTo(ICollider otherCollider)
        {
            var distancePosition = Vector2.Distance(this.Position, otherCollider.Position);
            if (otherCollider is CircleOffsetCollider)
            {
                var col = otherCollider as CircleOffsetCollider;

                float radiusSum = this.Radius + col.Radius;
                return distancePosition - radiusSum;
            }

            // If not Circle, Approximate using MaxDistanceToPosition
            return distancePosition - (this.MaximumDistanceFromPosition + otherCollider.MaximumDistanceFromPosition);
        }
    }
}
