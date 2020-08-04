using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComputergrafikSpiel.Model.Character.NPC.Interfaces;
using ComputergrafikSpiel.Model.Character.Player;
using ComputergrafikSpiel.Model.Character.Player.PlayerSystems;
using ComputergrafikSpiel.Model.Character.Weapon;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.Entity.Particles;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using OpenTK;
using OpenTK.Graphics;

namespace ComputergrafikSpiel.Model.Character.NPC
{
    public abstract class Enemy : INonPlayerCharacter
    {
        private Vector2 scale;

        public event EventHandler CharacterDeath;

        public event EventHandler CharacterHit;

        public event EventHandler CharacterMove;

        public int CurrentHealth { get; set; }

        public float BloodColorHue { get; set; } = 255f;

        public INPCController NPCController { get; set; }

        public int MaxHealth { get; set; }

        public float MovementSpeedDash => 1f * this.MovementSpeed * this.DashMultiplier;

        public float MovementSpeed { get; set; } = 1f;

        public int Defense { get; set; }

        public ICollider Collider { get; set; }

        public ITexture Texture { get; set; } = null;

        public IEnumerable<(Color4 color, Vector2[] vertices)> DebugData => new (Color4 color, Vector2[] vertices)[] { this.Collider.DebugData };

        public Vector2 Position { get; set; } = Vector2.Zero;

        public float Rotation { get; } = 0f;

        public Vector2 RotationAnker { get; } = Vector2.Zero;

        public Vector2 Scale { get; set; } = Vector2.One * 20;

        public int AttackDamage { get; set; }

        public bool TextureWasMirrored { get; set; } = false;

        public Vector2 LastPosition { get; set; }

        public EnemyEnum.Variant Variant { get; set; }

        public Vector2 Direction { get; set; }

        private float AttackCooldown { get; set; } = 0;

        private float DashMultiplier { get; set; } = 1f;

        private float BossBulletCycle { get; set; } = 1f;

        public void SetScale()
        {
            this.scale = this.Scale;
        }

        public void OnDeath(EventArgs e)
        {
            this.CharacterDeath?.Invoke(this, e);
        }

        public void OnHit(EventArgs e)
        {
            this.CharacterHit?.Invoke(this, e);
        }

        public void OnMove(EventArgs e)
        {
            this.CharacterMove?.Invoke(this, e);
        }

        public void SetEnemyStats(int maxHealth, float movementSpeed, int attackDamage)
        {
            this.MaxHealth = maxHealth;
            this.MovementSpeed = movementSpeed;
            this.AttackDamage = attackDamage;
        }

        public void TakingDamage(int damage)
        {
            if (damage <= 0)
            {
                throw new View.Exceptions.ArgumentNotPositiveIntegerGreaterZeroException(nameof(damage));
            }

            this.CurrentHealth -= damage;

            if (this.CurrentHealth <= 0)
            {
                this.DropLootOrHeal(50);
                EmitParticleOnceOptions opt = EmitParticleOnceOptions.ProjectileHit;
                opt.Count = 50;
                opt.PointOfEmmision = this.Position;
                opt.Direction = Vector2.One;
                opt.DirectionDeviation = 180;
                opt.Hue = (this.BloodColorHue, this.BloodColorHue);
                StaticParticleEmmiter.EmitOnce(opt);
                Scene.Scene.Current.RemoveObject(this);
                this.OnDeath(EventArgs.Empty);
            }
        }

        public void IncreaseDifficulty(int multiplier)
        {
            this.AttackDamage += multiplier;
            this.Defense += multiplier;
            this.MaxHealth += multiplier;
            this.MovementSpeed += multiplier;
        }

        public void Update(float dtime)
        {
            this.LastPosition = this.Position;

            this.LookAt(Scene.Scene.Player.Position);

            this.Direction = this.NPCController.EnemyAIMovement(this, dtime);

            this.OnMove(EventArgs.Empty);

            this.Position += this.Direction * this.MovementSpeedDash * dtime;

            this.AttackCooldown -= dtime;

            this.CollisionPrevention();
        }

        public void ShootBullet(float dtime)
        {
            if (this.AttackCooldown <= 0)
            {
                if (this.Variant == EnemyEnum.Variant.Boss)
                {
                    if (this.BossBulletCycle <= 0)
                    {
                        // TODO: geht nicht
                        this.ShootBulletBoss();
                        this.BossBulletCycle -= dtime;
                    }

                    this.BossBulletCycle = 1;
                    this.AttackCooldown = 2;
                    return;
                }

                this.AttackCooldown = 2;
                new Projectile(this.AttackDamage, Scene.Scene.Player.Position - this.Position, .6f, 12, false, this.Position, "Bullet");
            }
        }

        private void ShootBulletBoss()
        {
            new Projectile(this.AttackDamage, Scene.Scene.Player.Position - this.Position, 4f, 24, false, this.Position, "Bullet");
        }

        public void LookAt(Vector2 vec) => this.Scale = (this.Position.X < vec.X) ? this.Scale = this.scale * new Vector2(-1, 1) : this.scale;

        public void CollisionPrevention()
        {
            IReadOnlyCollection<ICollidable> collisions = Scene.Scene.Current.ColliderManager.GetCollisions(this);

            foreach (ICollidable collision in collisions)
            {
                if (collision.Collider.OwnLayer != ColliderLayer.Layer.Bullet)
                {
                    this.Position = this.LastPosition;
                    this.Direction = this.NPCController.EnemyAIMovement(this, 4);
                    return;
                }
            }
        }

        public void GiveDamageToPlayer()
        {
            if (this.AttackCooldown <= 0)
            {
                var collidables = Scene.Scene.Current.ColliderManager.GetCollisions(this);

                foreach (var player in from i in collidables where i is Player.Player select i as Player.Player)
                {
                    this.AttackCooldown = 2;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Spieler wurde getroffen!\n");
                    Scene.Scene.Player.TakingDamage(this.AttackDamage);
                }
            }

        }

        public void Dash()
        {
#pragma warning disable CS4014 // Da auf diesen Aufruf nicht gewartet wird, wird die Ausführung der aktuellen Methode vor Abschluss des Aufrufs fortgesetzt.
            this.DashTask();
#pragma warning restore CS4014 // Da auf diesen Aufruf nicht gewartet wird, wird die Ausführung der aktuellen Methode vor Abschluss des Aufrufs fortgesetzt.
            return;
        }

        private void DropLootOrHeal(int chance)
        {
            Random random = new Random();
            if (random.Next(0, 100) <= chance)
            {
                var whichOne = random.Next(0, 5);
                if (whichOne <= 2)
                {
                    (Scene.Scene.Current.Model as Model).SpawnInteractable(PlayerEnum.Stats.Heal, this.Position.X, this.Position.Y);
                }
                else
                {
                    (Scene.Scene.Current.Model as Model).SpawnInteractable(PlayerEnum.Stats.Money, this.Position.X, this.Position.Y);
                }
            }
        }

        private async Task DashTask()
        {
            this.DashMultiplier = 4f;
            await Task.Delay(100);
            this.DashMultiplier = 1f;
        }
    }
}
