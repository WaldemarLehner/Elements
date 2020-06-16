using System;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.Collider
{
    internal class RectangleOffsetCollider : ICollider
    {
        private readonly (float width, float height) size;

        private Vector2 offset;

        internal RectangleOffsetCollider(ICollidable parent, Vector2 offset, float radius, ColliderLayer.Layer layer)
        {
            this.CollidableParent = parent ?? throw new ArgumentNullException(nameof(parent));
            this.offset = offset;
            this.size = (radius * 2, radius * 2);
            this.Layer = layer;
        }

        public ColliderLayer.Layer Layer { get; }

        public ICollidable CollidableParent { get; }

        public float MaximumDistanceFromPosition => (float)Math.Sqrt(.5f * (this.size.height + this.size.width));

        public Vector2 Position => this.CollidableParent.Position + this.offset;

        public Vector2[] Corners => new[]
        {
            new Vector2(this.Bounds.left, this.Bounds.top),
            new Vector2(this.Bounds.right, this.Bounds.top),
            new Vector2(this.Bounds.left, this.Bounds.bottom),
            new Vector2(this.Bounds.right, this.Bounds.bottom),
        };

        public (float top, float bottom, float left, float right) Bounds => (this.Position.Y + this.size.height, this.Position.Y - this.size.height, this.Position.X - this.size.width, this.Position.X + this.size.width);

        public bool DidCollideWith(ICollider otherCollider) => CollisionDetectionHelper.DidCollideWith(this, otherCollider);

        public float MinimalDistanceTo(ICollider otherCollider) => CollisionDetectionHelper.MinDistanceBetween(this, otherCollider);
    }
}
