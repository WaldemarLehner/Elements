using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.Collider;
using OpenTK;

namespace ComputergrafikSpiel.Test.Model.TestHelper
{
    internal class MockCircleCollidable : ICollidable
    {
        internal static MockCircleCollidable CreateCollidableWithCollider(Vector2 position, float radius)
        {
            var collidable = new MockCircleCollidable(position);
            var collider = new CircleOffsetCollider(collidable, Vector2.Zero, radius, ColliderLayer.Layer.Player,  (ColliderLayer.Layer)~0);
            collidable.CircleCollider = collider;

            return collidable;
        }

        internal MockCircleCollidable(Vector2 position)
        {
            this.Position = position;
        }
        public CircleOffsetCollider CircleCollider { get; set; }

        public ICollider Collider => CircleCollider;

        public Vector2 Position { get; private set; }

        public Vector2 Scale => throw new System.NotImplementedException();

        public float Rotation => 0f;

        public Vector2 RotationAnker => Position;
    }
}
