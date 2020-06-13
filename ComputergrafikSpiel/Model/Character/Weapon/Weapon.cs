using System.Collections.Generic;
using ComputergrafikSpiel.Model.Character.Weapon.Interfaces;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
using OpenTK;
using OpenTK.Graphics;

namespace ComputergrafikSpiel.Model.Character.Weapon
{
    internal class Weapon : IWeapon
    {
        internal Weapon(float firerate, int projectileCCount, int bulletTTL, float bulletSize, IColliderManager colliderManager, float attackDamage, IModel model)
        {
            this.Firerate = firerate;
            this.ProjectileCreationCount = projectileCCount;
            this.BulletTTL = bulletTTL;
            this.BulletSize = bulletSize;
            this.ColliderManager = colliderManager;
            this.Collider = new CircleOffsetCollider(this, Vector2.Zero, 10);
            this.Texture = new TextureLoader().LoadTexture("StatIncrease/AttackDamageIncrease");
            this.AttackDamage = attackDamage;
            this.Model = model;
        }

        // most likely temporary
        public IModel Model { get; }

        public int ProjectileCreationCount { get; }

        public float Firerate { get; }

        public int BulletTTL { get; }

        public float BulletSize { get; }

        public IColliderManager ColliderManager { get; }

        public ICollider Collider { get; set; }

        public ITexture Texture { get; set; }

        // temp position
        public Vector2 Position => new Vector2(300, 300);

        public float Rotation { get; } = 0f;

        public Vector2 RotationAnker { get; } = Vector2.Zero;

        public Vector2 Scale => new Vector2(32, 32);

        public float AttackDamage { get; }

        public IEnumerable<(Color4 color, Vector2[] vertices)> DebugData { get; } = new List<(Color4, Vector2[])>();

        public void Shoot(Vector2 position, Vector2 direction)
        {
            this.Model.CreateProjectile(this.ProjectileCreationCount, position, direction, this.BulletTTL, this.BulletSize, this.ColliderManager);
        }

        // has to be changed for shotgun/semi-automatic. For this it will always be as if the projectile count is 1
        /* public void CreateProjectile(Vector2 position, Vector2 direction)
        {
            for (int i = 0; i <= this.ProjectileCreationCount; i++)
            {
                new Projectile(position, direction, this.BulletTTL, this.BulletSize, this.ColliderManager);
            }
        } */

        public void Update(float dtime)
        {
        }
    }
}