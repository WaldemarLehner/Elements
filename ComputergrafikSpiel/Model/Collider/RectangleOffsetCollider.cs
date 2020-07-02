using System;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using OpenTK;
using OpenTK.Graphics;

namespace ComputergrafikSpiel.Model.Collider
{
    internal class RectangleOffsetCollider : ICollider
    {
        private readonly (float width, float height) size;

        private Vector2 offset;

        internal RectangleOffsetCollider(ICollidable parent, Vector2 offset, float radius, ColliderLayer.Layer self, ColliderLayer.Layer collidesWith)
        {
            this.CollidableParent = parent ?? throw new ArgumentNullException(nameof(parent));
            this.offset = offset;
            this.size = (radius * 2, radius * 2);
            this.CollidesWith = collidesWith;
            this.OwnLayer = self;
        }

        public ColliderLayer.Layer CollidesWith { get; }

        public ICollidable CollidableParent { get; }

        public float MaximumDistanceFromPosition => Vector2.Distance(Vector2.Zero, new Vector2(this.size.width, this.size.height) / 2f);

        public Vector2 Position => this.CollidableParent.Position + this.offset;

        public Vector2[] Corners => new[]
        {
            new Vector2(this.Bounds.left, this.Bounds.top),
            new Vector2(this.Bounds.right, this.Bounds.top),
            new Vector2(this.Bounds.left, this.Bounds.bottom),
            new Vector2(this.Bounds.right, this.Bounds.bottom),
        };

        public (float top, float bottom, float left, float right) Bounds => (this.Position.Y + this.size.height, this.Position.Y - this.size.height, this.Position.X - this.size.width, this.Position.X + this.size.width);

        public (Color4 color, Vector2[] verts) DebugData => (new Color4(0, 255, 0, 255), this.GetDebugData());

        public ColliderLayer.Layer OwnLayer { get; }

        private Vector2[] GetDebugData()
        {
            return new Vector2[]
            {
                new Vector2(this.Bounds.left, this.Bounds.top),
                new Vector2(this.Bounds.right, this.Bounds.top),
                new Vector2(this.Bounds.right, this.Bounds.bottom),
                new Vector2(this.Bounds.left, this.Bounds.bottom),
                new Vector2(this.Bounds.left, this.Bounds.top),
            };
        }

        public bool DidCollideWith(ICollider otherCollider) => CollisionDetectionHelper.DidCollideWith(this, otherCollider);

        public float MinimalDistanceTo(ICollider otherCollider) => CollisionDetectionHelper.MinDistanceBetween(this, otherCollider);
    }
}
