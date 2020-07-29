﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ComputergrafikSpiel.Model.Character.NPC.Interfaces;
using ComputergrafikSpiel.Model.Character.NPC.NPCAI;
using ComputergrafikSpiel.Model.Character.Player;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.Entity.Particles;
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
            this.Texture = new TextureLoader().LoadTexture("NPC/" + texture);
            this.Position = startPosition;

            this.Scale = this.scale;
            var collisionMask = ColliderLayer.Layer.Bullet | ColliderLayer.Layer.Player | ColliderLayer.Layer.Wall | ColliderLayer.Layer.Water;
            if (texture == "Enemy/Water/Fungus")
            {
                this.Collider = new CircleOffsetCollider(this, Vector2.Zero, 17, ColliderLayer.Layer.Enemy, collisionMask);
                this.scale = new Vector2(18, 18);
            }
            else if (texture == "Enemy/Water/Lizard")
            {
                this.Collider = new CircleOffsetCollider(this, Vector2.Zero, 20, ColliderLayer.Layer.Enemy, collisionMask);
                this.scale = new Vector2(24, 24);
            }

            // Bosse
            else if (texture == "Boss/Tree")
            {
                this.Collider = new CircleOffsetCollider(this, Vector2.Zero, 20, ColliderLayer.Layer.Enemy, collisionMask);
                this.scale = new Vector2(45, 45);
            }

            // Nichtdefinierte Gegner
            else
            {
                this.Collider = new CircleOffsetCollider(this, Vector2.Zero, 10, ColliderLayer.Layer.Enemy, collisionMask);
                this.scale = new Vector2(16, 16);
            }

            this.Scale = this.scale;
            this.NPCController = new AIEnemy();
        }

        public event EventHandler CharacterDeath;

        public event EventHandler CharacterHit;

        public event EventHandler CharacterMove;

        public int CurrentHealth { get; set; }

        public float BloodColorHue => 215f;

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

            this.Position += this.Direction * this.MovementSpeed * dtime;

            this.AttackCooldown -= dtime;

            if (this.AttackCooldown <= 0)
            {
                this.GiveDamageToPlayer();
            }

            this.CollisionPrevention();
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

        private void GiveDamageToPlayer()
        {
            var collidables = Scene.Scene.Current.ColliderManager.GetCollisions(this);

            foreach (var player in from i in collidables where i is Player.Player select i as Player.Player)
            {
                this.AttackCooldown = 2;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Spieler wurde getroffen!\n");
                Scene.Scene.Player.TakingDamage();
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
                    (Scene.Scene.Current.Model as Model).SpawnInteractable(PlayerEnum.Stats.Heal, this.Position.X, this.Position.Y);
                }
                else
                {
                    (Scene.Scene.Current.Model as Model).SpawnInteractable(PlayerEnum.Stats.Money, this.Position.X, this.Position.Y);
                }
            }
        }
    }
}
