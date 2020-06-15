using System.Collections.Generic;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using ComputergrafikSpiel.Model.Trigger.Interfaces;
using OpenTK;
using OpenTK.Graphics;

namespace ComputergrafikSpiel.Model.Trigger
{
    internal class Trigger : ITrigger
    {
        public Trigger(IColliderManager colliderManager, Vector2 position)
        {
            // radius may have to be changed
            this.Collider = new CircleOffsetCollider(this, Vector2.Zero, 32);
            this.ColliderManager = colliderManager;
            this.ColliderManager.AddTriggerCollidable((int)position.X, (int)position.Y, this);
            this.Position = position;
            this.Texture = new TextureLoader().LoadTexture("Door.TreeBranchesDoor");
        }

        public IColliderManager ColliderManager { get; }

        public ICollider Collider { get; }

        public Vector2 Position { get; }

        public float Rotation { get; } = 0f;

        public Vector2 RotationAnker { get; } = Vector2.Zero;

        // may have to be changed with the collider radius
        public Vector2 Scale { get; } = Vector2.One * 32;

        public ITexture Texture { get; }

        public IEnumerable<(Color4 color, Vector2[] vertices)> DebugData => throw new System.NotImplementedException();

        public void Update(float dtime)
        {
            throw new System.NotImplementedException();
        }
    }
}
