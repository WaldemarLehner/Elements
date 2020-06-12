using System.Collections.Generic;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using OpenTK;
using OpenTK.Graphics;

namespace ComputergrafikSpiel.Model.Entity
{
    internal class TestInteractable : IEntity
    {
        public TestInteractable(IColliderManager colliderManager)
        {
            this.Position = new Vector2(200, 200);
            this.Scale = new Vector2(20, 20);
            this.Collider = new CircleOffsetCollider(this, Vector2.Zero, 10);
            colliderManager.AddEntityCollidable(this.Collider.CollidableParent);
            this.Texture = new ComputergrafikSpiel.Model.EntitySettings.Texture.TextureLoader().LoadTexture("StatIncrease/MovementSpeedIncrease");
        }

        public ICollider Collider { get; set; }

        public ITexture Texture { get; } = null;

        public Vector2 Position { get; set; } = Vector2.Zero;

        public float Rotation { get; } = 0f;

        public Vector2 RotationAnker { get; } = Vector2.Zero;

        public Vector2 Scale { get; } = Vector2.One * 20;

        public IEnumerable<(Color4 color, Vector2[] vertices)> DebugData { get; } = new List<(Color4, Vector2[])>();

        public void Update(float dtime)
        {
        }
    }
}
