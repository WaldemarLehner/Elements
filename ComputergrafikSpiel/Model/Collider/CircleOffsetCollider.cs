using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using OpenTK;
using OpenTK.Graphics;

namespace ComputergrafikSpiel.Model.Collider
{
    internal class CircleOffsetCollider : ICollider
    {
        private Vector2 offset;

        internal CircleOffsetCollider(ICollidable parent, Vector2 offset, float radius, ColliderLayer.Layer self, ColliderLayer.Layer collidesWith)
        {
            this.CollidableParent = parent ?? throw new ArgumentNullException(nameof(parent));
            if (radius <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(radius), "Expect positive radius greater Zero");
            }

            this.Radius = radius;
            this.offset = offset;
            this.OwnLayer = self;
            this.CollidesWith = collidesWith;
        }

        public ColliderLayer.Layer CollidesWith { get; }

        public float Radius { get; }

        public ICollidable CollidableParent { get; }

        public float MaximumDistanceFromPosition => this.Radius;

        public Vector2 Position => this.CollidableParent.Position + this.offset;

        public float Rotation => 0;

        public Vector2 RotationAnker => this.Position;

        public (Color4 color, Vector2[] verts) DebugData => (new Color4(0, 255, 0, 255), this.GenerateDebugVerts());

        public ColliderLayer.Layer OwnLayer { get; }

        public bool DidCollideWith(ICollider otherCollider) => CollisionDetectionHelper.DidCollideWith(this, otherCollider);

        public float MinimalDistanceTo(ICollider otherCollider) => CollisionDetectionHelper.MinDistanceBetween(this, otherCollider);

        private Vector2[] GenerateDebugVerts()
        {
            const int resolution = 15;
            var degree = 0f;
            Vector2[] verts = new Vector2[resolution + 1];
            for (int i = 0; i < resolution; i++)
            {
                float x = (float)Math.Cos(degree);
                float y = (float)Math.Sin(degree);
                degree += (float)(2f * Math.PI) / resolution;
                Vector2 direction = new Vector2(x, y);
                direction *= this.Radius;
                verts[i] = direction + this.Position;
            }

            verts[resolution] = verts[0];
            return verts;
        }
    }
}
