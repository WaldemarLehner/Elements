using System.Collections.Generic;
using System.Linq;
using ComputergrafikSpiel.Model.Character.NPC.Interfaces;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
using OpenTK;
using OpenTK.Graphics;

namespace ComputergrafikSpiel.Model.Character.Weapon
{
    internal class Projectile : IEntity
    {
        internal Projectile(int attackDamage, Vector2 position, Vector2 direction, float ttl, float bulletSize, IColliderManager colliderManager)
        {
            this.AttackDamage = attackDamage;
            this.Position = position;
            this.Direction = direction;
            this.TTL = ttl;
            this.Texture = new TextureLoader().LoadTexture("Projectile/Bullet");

            this.Collider = new CircleOffsetCollider(this, Vector2.Zero, bulletSize, ColliderLayer.Layer.Bullet, ColliderLayer.Layer.Wall | ColliderLayer.Layer.Enemy);
            this.ColliderManager = colliderManager;
            this.ColliderManager.AddEntityCollidable(this);
            this.Scale = Vector2.One * bulletSize;

            // rotation calculation
            Vector2 positionForRotation = new Vector2(1, 0);
            Vector2 directionNormalized = Vector2.Normalize(direction);
            this.Rotation = RotationHelper.GetRotationBetweenTwoVectorsRadians(positionForRotation, directionNormalized);
            this.RotationAnker = position;
            Scene.Scene.Current.SpawnEntity(this);
        }

        public int AttackDamage { get; }

        public IColliderManager ColliderManager { get; }

        public float TTL { get; set; }

        public Vector2 Position { get; set; }

        public Vector2 Direction { get; set; }

        public ICollider Collider { get; set; }

        public ITexture Texture { get; }

        public float Rotation { get; }

        public Vector2 RotationAnker { get; set; }

        public Vector2 Scale { get; }

        public IEnumerable<(Color4 color, Vector2[] vertices)> DebugData => null;

        public void Update(float dtime)
        {
            this.Position += this.Direction * dtime;
            this.RotationAnker = this.Position;
            this.TTL -= dtime;
            this.ProjectileCollisionManager();
            if (this.TTL <= 0)
            {
                Scene.Scene.Current.RemoveEntity(this);
            }
        }

        public void ProjectileCollisionManager()
        {
            IReadOnlyCollection<ICollidable> bulletCollisions = new List<ICollidable>();
            bulletCollisions = this.ColliderManager.GetCollisions(this);

            foreach (var collidableToCheck in bulletCollisions)
            {
                foreach (var tileCollidable in this.ColliderManager.CollidableTileDictionary)
                {
                    if (collidableToCheck == tileCollidable.Value)
                    {
                        Scene.Scene.Current.RemoveEntity(this);
                    }
                }

                foreach (var enemyCollidable in Scene.Scene.Current.NPCs.ToList())
                {
                    if (collidableToCheck == enemyCollidable)
                    {
                        enemyCollidable.TakingDamage(this.AttackDamage);
                        Scene.Scene.Current.RemoveEntity(this);
                    }
                }
            }
        }
    }
}