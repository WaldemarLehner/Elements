using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ComputergrafikSpiel.Model.Character.NPC.Interfaces;
using ComputergrafikSpiel.Model.Character.NPC.NPCAI;
using ComputergrafikSpiel.Model.Character.Player;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using OpenTK;
using OpenTK.Graphics;

namespace ComputergrafikSpiel.Model.Character.NPC
{
    public class Enemy : INonPlayerCharacter
    {
        private readonly Vector2 scale;

        public Enemy(int maxHealth, string texture, float movementSpeed, int defense, int attackDamage, Vector2 startPosition)
        {
            this.AttackDamage = attackDamage;
            this.MaxHealth = maxHealth;
            this.CurrentHealth = maxHealth;
            this.MovementSpeed = movementSpeed;
            this.Defense = defense;
            this.Texture = new TextureLoader().LoadTexture("Enemy/" + texture);
            this.Position = startPosition;
            this.scale = new Vector2(16, 16);
            this.Scale = this.scale;
            var collisionMask = ColliderLayer.Layer.Bullet | ColliderLayer.Layer.Player | ColliderLayer.Layer.Wall | ColliderLayer.Layer.Water;
            if (texture == "Fungus")
            {
                this.Collider = new CircleOffsetCollider(this, Vector2.Zero, 17, ColliderLayer.Layer.Enemy, collisionMask);
            }
            else
            {
                this.Collider = new CircleOffsetCollider(this, Vector2.Zero, 10, ColliderLayer.Layer.Enemy, collisionMask);
            }

            this.NPCController = new AIEnemy();
        }

        public event EventHandler CharacterDeath;

        public event EventHandler CharacterHit;

        public event EventHandler CharacterMove;

        public int CurrentHealth { get; set; }

        public INPCController NPCController { get; }

        public int MaxHealth { get; set; }

        public float MovementSpeed { get; set; } = 35;

        public int Defense { get; set; }

        public ICollider Collider { get; }

        public ITexture Texture { get; } = null;

        public IEnumerable<(Color4 color, Vector2[] vertices)> DebugData => new (Color4 color, Vector2[] vertices)[] { this.Collider.DebugData };

        public Vector2 Position { get; set; } = Vector2.Zero;

        public float Rotation { get; } = 0f;

        public Vector2 RotationAnker { get; } = Vector2.Zero;

        public Vector2 Scale { get; set; } = Vector2.One * 20;

        public int AttackDamage { get; set; }

        public bool TextureWasMirrored { get; set; } = false;

        public Vector2 LastPosition { get; set; }

        private Vector2 Direction { get; set; }

        private float AttackCooldown { get; set; } = 0;

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

        public void TakingDamage(int damage)
        {
            if (damage <= 0)
            {
                throw new View.Exceptions.ArgumentNotPositiveIntegerGreaterZeroException(nameof(damage));
            }

            if (this.Defense < damage)
            {
                damage -= this.Defense;
                this.CurrentHealth -= damage;
                this.OnHit(EventArgs.Empty);
            }

            if (this.CurrentHealth <= 0)
            {
                this.DropLootOrHeal(50);
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

            this.Position += this.Direction * this.MovementSpeed * dtime;

            this.CollisionPrevention();

            this.AttackCooldown -= dtime;

            if (this.AttackCooldown <= 0)
            {
                this.GiveDamageToPlayer();
            }
        }

        public void LookAt(Vector2 vec) => this.Scale = (this.Position.X < vec.X) ? this.Scale = this.scale * new Vector2(-1, 1) : this.scale;

        public void CollisionPrevention()
        {
            /*foreach (var collision in Scene.Scene.Current.ColliderManager.GetCollisions(this))
            {
                if (collision.GetType == )
            }
            */
            if (Scene.Scene.Current.ColliderManager.GetCollisions(this).Count > 0)
            {
                this.Position = this.LastPosition;
                this.Direction = Vector2.Multiply(this.Direction, -1);
            }

            return;
        }

        private void GiveDamageToPlayer()
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

        private void DropLootOrHeal(int chance)
        {
            Random random = new Random();
            if (random.Next(0, 100) <= chance)
            {
                var whichOne = random.Next(0, 5);
                if (whichOne <= 2)
                {
                    (Scene.Scene.Current.Model as Model).SpawnInteractable(PlayerEnum.Stats.Heal, this.Position.X, this.Position.Y, 1);
                }
                else
                {
                    (Scene.Scene.Current.Model as Model).SpawnInteractable(PlayerEnum.Stats.Währung, this.Position.X, this.Position.Y, 1);
                }
            }
        }
    }
}
