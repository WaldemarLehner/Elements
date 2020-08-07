using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.Entity.Particles;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using OpenTK;
using OpenTK.Graphics;

namespace ComputergrafikSpiel.Model.Character.Weapon
{
    internal class Projectile : IEntity
    {
        private readonly GenericParticleEmitter backsmokeEmitter;

        internal Projectile(int attackDamage, Vector2 direction, float ttl, float bulletSize, bool player, Vector2 position, string texture, (float, float) hue)
        {
            this.AttackDamage = attackDamage;
            this.Position = position;
            this.Direction = direction;
            this.TTL = ttl;

            this.Texture = new TextureLoader().LoadTexture("Projectile/" + texture);

            if (player)
            {
                this.Collider = new CircleOffsetCollider(this, Vector2.Zero, bulletSize / 2, ColliderLayer.Layer.Bullet, ColliderLayer.Layer.Wall | ColliderLayer.Layer.Enemy);
            }
            else
            {
                this.Collider = new CircleOffsetCollider(this, Vector2.Zero, bulletSize / 2, ColliderLayer.Layer.Bullet, ColliderLayer.Layer.Wall | ColliderLayer.Layer.Player);
            }

            this.Scale = Vector2.One * bulletSize;

            var emitOpt = EmitParticleOnceOptions.BulletSmoke;
            emitOpt.Direction = this.Direction * -1;
            emitOpt.PointOfEmmision = this.Position;
            emitOpt.Hue = hue;
            this.backsmokeEmitter = new GenericParticleEmitter(emitOpt, .02f);

            // rotation calculation
            Vector2 positionForRotation = new Vector2(1, 0);
            Vector2 directionNormalized = Vector2.Normalize(direction);
            this.Rotation = RotationHelper.GetRotationBetweenTwoVectorsRadians(positionForRotation, directionNormalized);
            this.RotationAnker = Scene.Scene.Player.Position;
            Scene.Scene.Current.SpawnObject(this);
        }

        public int AttackDamage { get; }

        public float TTL { get; set; }

        public Vector2 Position { get; set; }

        public Vector2 Direction { get; set; }

        public ICollider Collider { get; set; }

        public ITexture Texture { get; }

        public float Rotation { get; }

        public Vector2 RotationAnker { get; set; }

        public Vector2 Scale { get; set; }

        public IEnumerable<(Color4 color, Vector2[] vertices)> DebugData => null;

        public void Update(float dtime)
        {
            this.Position += this.Direction * dtime;
            this.RotationAnker = this.Position;
            this.TTL -= dtime;
            this.ProjectileCollisionManager();

            var opt = this.backsmokeEmitter.Options;
            opt.PointOfEmmision = this.Position;
            this.backsmokeEmitter.Options = opt;
            this.backsmokeEmitter.Update(dtime);
            if (this.TTL <= 0)
            {
                EmitParticleOnceOptions onDeathOpt = EmitParticleOnceOptions.ProjectileHit;
                onDeathOpt.Count = 50;
                onDeathOpt.PointOfEmmision = this.Position;
                onDeathOpt.TTL = .3f;
                onDeathOpt.TTLDeviation = .2f;
                onDeathOpt.Direction = Vector2.One;
                onDeathOpt.DirectionDeviation = 180;
                onDeathOpt.Hue = (0, 0);
                onDeathOpt.Saturation = (0, 0);
                onDeathOpt.SaturationDeviation = 0f;
                onDeathOpt.Value = (.2f, 1f);
                StaticParticleEmmiter.EmitOnce(onDeathOpt);
                Scene.Scene.Current.RemoveObject(this);
            }
        }

        public void ProjectileCollisionManager()
        {
            IReadOnlyCollection<ICollidable> bulletCollisions = Scene.Scene.Current.ColliderManager.GetCollisions(this);

            foreach (var collidableToCheck in bulletCollisions)
            {
                if (collidableToCheck == Scene.Scene.Player)
                {
                    Scene.Scene.Player.TakingDamage(this.AttackDamage);
                    Scene.Scene.Current.RemoveObject(this);

                    EmitParticleOnceOptions opt = EmitParticleOnceOptions.ProjectileHit;
                    opt.PointOfEmmision = Scene.Scene.Player.Position;
                    opt.Direction = this.Direction.Normalized();
                    opt.Hue = (Scene.Scene.Player.BloodColorHue, Scene.Scene.Player.BloodColorHue);
                    StaticParticleEmmiter.EmitOnce(opt);
                }

                foreach (var tileCollidable in Scene.Scene.Current.ColliderManager.CollidableTileDictionary)
                {
                    if (collidableToCheck == tileCollidable.Value)
                    {
                        Scene.Scene.Current.RemoveObject(this);
                    }
                }

                foreach (var enemyCollidable in Scene.Scene.Current.NPCs.ToList())
                {
                    if (collidableToCheck == enemyCollidable)
                    {
                        enemyCollidable.TakingDamage(this.AttackDamage);
                        Scene.Scene.Current.RemoveObject(this);

                        // Spawn particles
                        EmitParticleOnceOptions opt = EmitParticleOnceOptions.ProjectileHit;
                        opt.PointOfEmmision = enemyCollidable.Position;
                        opt.Direction = this.Direction.Normalized();
                        opt.Hue = (enemyCollidable.BloodColorHue, enemyCollidable.BloodColorHue);
                        StaticParticleEmmiter.EmitOnce(opt);
                    }
                }
            }
        }
    }
}