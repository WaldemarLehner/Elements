using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model.Character.NPC.Interfaces;
using ComputergrafikSpiel.Model.Character.NPC.NPCAI;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
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
        public Enemy(int maxHealth, string texture, float movementSpeed, int defense, int attackDamage, IPlayer player, IColliderManager colliderManager, ICollection<INonPlayerCharacter> allEnemys, Vector2 startPosition)
        {
            this.AttackDamage = attackDamage;
            this.MaxHealth = maxHealth;
            this.MovementSpeed = movementSpeed;
            this.Defense = defense;
            this.Texture = new TextureLoader().LoadTexture("Enemy/" + texture);
            this.Position = startPosition;
            this.Scale = new Vector2(16, 16);
            this.Player = player;
            this.Collider = new CircleOffsetCollider(this, Vector2.Zero, 10);
            colliderManager.AddEntityCollidable(this.Collider.CollidableParent);
            this.NPCController = new AIEnemy(colliderManager, allEnemys, this.Player);
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

        public IEnumerable<(Color4 color, Vector2[] vertices)> DebugData { get; } = new List<(Color4, Vector2[])>();

        public Vector2 Position { get; set; } = Vector2.Zero;

        public float Rotation { get; } = 0f;

        public Vector2 RotationAnker { get; } = Vector2.Zero;

        public Vector2 Scale { get; } = Vector2.One * 20;

        public int AttackDamage { get; set; }

        private Vector2 Direction { get; set; }

        private IPlayer Player { get; }

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
            this.Direction = this.NPCController.EnemyAIMovement(this);

            this.Position += this.Direction * this.MovementSpeed * dtime;

            this.GiveDamageToPlayer();
        }

        private void GiveDamageToPlayer()
        {
            if (this.Collider.DidCollideWith(this.Player.Collider))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Spieler wurde getroffen!\n");
                this.Player.TakingDamage(this.AttackDamage);
            }
        }
    }
}
