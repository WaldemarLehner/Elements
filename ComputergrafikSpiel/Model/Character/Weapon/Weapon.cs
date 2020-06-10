using ComputergrafikSpiel.Model.Character.Weapon.Interfaces;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.Character.Weapon
{
    internal class Weapon : IWeapon
    {
        internal Weapon(float firerate, int projectileCCount, int bulletTTL, float bulletSize, IColliderManager colliderManager)
        {
            this.Firerate = firerate;
            this.ProjectileCreationCount = projectileCCount;
            this.BulletTTL = bulletTTL;
            this.BulletSize = bulletSize;
            this.ColliderManager = colliderManager;
            this.Collider = new CircleOffsetCollider(this, Vector2.Zero, 10);
            this.Texture = new TextureLoader().LoadTexture("statincrease/");
        }

        public int ProjectileCreationCount { get; }

        public float Firerate { get; }

        public int BulletTTL { get; }

        public float BulletSize { get; }

        public IColliderManager ColliderManager { get; }

        public ICollider Collider { get; }

        public ITexture Texture { get; }

        public Vector2 Position => throw new System.NotImplementedException();

        public float Rotation { get; } = 0f;

        public Vector2 RotationAnker { get; } = Vector2.Zero;

        public Vector2 Scale => throw new System.NotImplementedException();

        public float AttackDamage => throw new System.NotImplementedException();

        // has to be changed for shotgun/semi-automatic. For this it will always be as if the projectile count is 1
        public void CreateProjectile(Vector2 position, Vector2 direction)
        {
            for (int i = 0; i <= this.ProjectileCreationCount; i++)
            {
                new Projectile(position, direction, this.BulletTTL, this.BulletSize, this.ColliderManager);
            }
        }

        public void Update(float dtime)
        {
            throw new System.NotImplementedException();
        }
    }
}