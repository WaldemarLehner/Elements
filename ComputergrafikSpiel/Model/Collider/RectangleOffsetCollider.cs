using System;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using OpenTK;
using OpenTK.Graphics;

namespace ComputergrafikSpiel.Model.Collider
{
    internal class RectangleOffsetCollider : ICollider
    {
        private readonly (float width, float height) halfsize;

        private Vector2 offset;

        internal RectangleOffsetCollider(ICollidable parent, Vector2 offset, float diameter, ColliderLayer.Layer self, ColliderLayer.Layer collidesWith)
        {
            this.CollidableParent = parent ?? throw new ArgumentNullException(nameof(parent));
            this.offset = offset;
            this.halfsize = (diameter, diameter);
            this.CollidesWith = collidesWith;
            this.OwnLayer = self;
        }

        public ColliderLayer.Layer CollidesWith { get; }

        public ICollidable CollidableParent { get; }

        public float MaximumDistanceFromPosition => Vector2.Distance(Vector2.Zero, new Vector2(this.halfsize.width, this.halfsize.height));

        public Vector2 Position => this.CollidableParent.Position + this.offset;

        public Vector2[] Corners => new[]
        {
            new Vector2(this.Bounds.left, this.Bounds.top),
            new Vector2(this.Bounds.right, this.Bounds.top),
            new Vector2(this.Bounds.left, this.Bounds.bottom),
            new Vector2(this.Bounds.right, this.Bounds.bottom),
        };

        public (float top, float bottom, float left, float right) Bounds => (this.Position.Y + this.halfsize.height, this.Position.Y - this.halfsize.height, this.Position.X - this.halfsize.width, this.Position.X + this.halfsize.width);

        public (Color4 color, Vector2[] verts) DebugData => (new Color4(0, 255, 0, 255), this.GetDebugData());

        public ColliderLayer.Layer OwnLayer { get; }

        public bool DidCollideWith(ICollider otherCollider) => CollisionDetectionHelper.DidCollideWith(this, otherCollider);

        public float MinimalDistanceTo(ICollider otherCollider) => CollisionDetectionHelper.MinDistanceBetween(this, otherCollider);

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
    }
}
