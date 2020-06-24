using System.Collections.Generic;
using ComputergrafikSpiel.Model.Character.NPC.Interfaces;
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
    public class Weapon : IWeapon
    {
        internal Weapon(float firerate, int projectileCCount, float bulletTTL, float bulletSize, int attackDamage)
        {
            this.Firerate = firerate;
            this.ProjectileCreationCount = projectileCCount;
            this.BulletTTL = bulletTTL;
            this.BulletSize = bulletSize;
            this.Collider = new CircleOffsetCollider(this, Vector2.Zero, 10, ColliderLayer.Layer.Interactable, ColliderLayer.Layer.Player | ColliderLayer.Layer.Wall | ColliderLayer.Layer.Bullet);
            this.AttackDamage = attackDamage;
        }

        // most likely temporary
        public IModel Model { get; }

        public int ProjectileCreationCount { get; }

        public float Firerate { get; }

        public float BulletTTL { get; }

        public float BulletSize { get; }

        public IColliderManager ColliderManager { get; }

        public ICollider Collider { get; set; }

        // temp position
        public Vector2 Position => new Vector2(300, 300);

        public float Rotation { get; } = 0f;

        public Vector2 RotationAnker { get; } = Vector2.Zero;

        public Vector2 Scale => new Vector2(32, 32);

        public int AttackDamage { get; }

        public IEnumerable<(Color4 color, Vector2[] vertices)> DebugData { get; } = new List<(Color4, Vector2[])>();

        public ITexture Texture { get; }

        public void Shoot(Vector2 position, Vector2 direction, ICollection<INonPlayerCharacter> enemyList)
        {
           // @Gerald this.Model.CreateProjectile(this.AttackDamage, this.ProjectileCreationCount, position, direction, this.BulletTTL, this.BulletSize, this.ColliderManager, enemyList);
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