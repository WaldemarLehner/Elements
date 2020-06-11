using System.Collections.Generic;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using OpenTK;
using OpenTK.Graphics;

namespace ComputergrafikSpiel.Model.Character.Weapon
{
    internal class Projectile : IEntity
    {
        private readonly int ttl;
        private readonly float bulletSize;

        internal Projectile(Vector2 position, Vector2 direction, int ttl, float bulletSize, IColliderManager colliderManager)
        {
            this.Position = position;
            this.Direction = direction;
            this.ttl = ttl;
            this.bulletSize = bulletSize;

            // name of texture to be determined
            this.Texture = new TextureLoader().LoadTexture("Projectile/Bullet");

            // added to the ColliderManager?
            this.Collider = new CircleOffsetCollider(this, Vector2.Zero, bulletSize);
            colliderManager.AddEntityCollidable(this);
        }

        public Vector2 Position { get; set; }

        public Vector2 Direction { get; set; }

        public ICollider Collider { get; set; }

        public ITexture Texture { get; }

        public float Rotation { get; } = 0f;

        public Vector2 RotationAnker { get; } = Vector2.Zero;

        // should this be multiplied by bullet size instead?
        public Vector2 Scale { get; } = Vector2.One * 20;

        public IEnumerable<(Color4 color, Vector2[] vertices)> DebugData => throw new System.NotImplementedException();

        // does this work?
        public void Update(float dtime)
        {
            this.Position += this.Direction * dtime;
        }
    }
}