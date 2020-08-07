using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model.Character.NPC.Interfaces;
using ComputergrafikSpiel.Model.Character.Weapon.Interfaces;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.Entity.Particles;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
using OpenTK;
using OpenTK.Graphics;
using Spectrum;

namespace ComputergrafikSpiel.Model.Character.Weapon
{
    public class Weapon : IWeapon
    {
        internal Weapon(float firerate, int projectileCCount, float bulletSize)
        {
            this.Firerate = firerate;
            this.ProjectileCreationCount = projectileCCount;
            this.BulletSize = bulletSize;
            this.Collider = new CircleOffsetCollider(this, Vector2.Zero, 10, ColliderLayer.Layer.Interactable, ColliderLayer.Layer.Player | ColliderLayer.Layer.Wall | ColliderLayer.Layer.Bullet);
        }

        // most likely temporary
        public IModel Model { get; }

        public int ProjectileCreationCount { get; }

        public float Firerate { get; }

        public float BulletSize { get; }

        public ICollider Collider { get; set; }

        public Vector2 Position => new Vector2(Scene.Scene.Player.Position.X, Scene.Scene.Player.Position.Y - 10);

        public float Rotation { get; } = 0f;

        public Vector2 RotationAnker { get; } = Vector2.Zero;

        public Vector2 Scale { get => new Vector2(32, 32); set => _ = value; }

        public IEnumerable<(Color4 color, Vector2[] vertices)> DebugData { get; } = new List<(Color4, Vector2[])>();

        public ITexture Texture { get; }

        public void CreateProjectile(Vector2 direction)
        {
            for (int i = 0; i < this.ProjectileCreationCount; i++)
            {
                new Projectile(Scene.Scene.Player.PlayerData.bulletDamage, direction, Scene.Scene.Player.PlayerData.bulletTTL, this.BulletSize, true, Scene.Scene.Player.Position, "Bullet", (37f, 20f));
            }

            EmitParticleOnceOptions opt = EmitParticleOnceOptions.PlayerWeaponMuzzle;
            opt.PointOfEmmision = this.Position;
            opt.Direction = direction.Normalized();
            StaticParticleEmmiter.EmitOnce(opt);
        }

        public void Update(float dtime)
        {
        }
    }
}