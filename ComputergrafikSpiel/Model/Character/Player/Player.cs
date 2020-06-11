using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model.Character.NPC.Interfaces;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.Character.Player.PlayerSystems;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.Entity;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
using OpenTK;
using OpenTK.Graphics;

namespace ComputergrafikSpiel.Model.Character.Player
{
    internal class Player : IPlayer
    {
        private readonly List<PlayerEnum.PlayerActions> playerActionList;
        private readonly PlayerAttackSystem playerAttackSystem;
        private readonly PlayerMovementSystem playerMovementSystem;
        private readonly PlayerInteractionSystem playerInteractionSystem;
        private bool run = false;
        private Vector2 directionXY = Vector2.Zero;
        private ICollection<INonPlayerCharacter> enemyList;
        private IModel model;

        public Player(IReadOnlyDictionary<PlayerEnum.Stats, IEntity> interactable, IColliderManager colliderManager, ICollection<INonPlayerCharacter> enemys, IModel model)
        {
            this.model = model;
            this.enemyList = enemys;
            this.CurrentHealth = this.MaxHealth;
            this.playerActionList = new List<PlayerEnum.PlayerActions>();
            this.Position = new Vector2(50, 50);
            this.Scale = new Vector2(32, 32);
            this.Collider = new CircleOffsetCollider(this, Vector2.Zero, 10);
            this.playerAttackSystem = new PlayerAttackSystem();
            this.playerMovementSystem = new PlayerMovementSystem();
            this.playerInteractionSystem = new PlayerInteractionSystem(interactable);
            this.Texture = new TextureLoader().LoadTexture("PlayerWeapon");
            colliderManager.AddEntityCollidable(this.Collider.CollidableParent);
        }

        // Define Player
        public event EventHandler CharacterDeath;

        public event EventHandler CharacterHit;

        public event EventHandler CharacterMove;

        public event EventHandler PlayerInc;

        public int CurrentHealth { get; set; }

        public int MaxHealth { get; set; } = 5;

        public float MovementSpeed { get; set; } = 50;

        public int Defense { get; set; } = 0;

        public Vector2 Position { get; set; } = Vector2.Zero;

        public Vector2 Scale { get; } = Vector2.One * 10;

        public float Rotation { get; } = 0f;

        public Vector2 RotationAnker { get; } = Vector2.Zero;

        public ITexture Texture { get; }

        public ICollider Collider { get; set; }

        public IEnumerable<(Color4 color, Vector2[] vertices)> DebugData { get; } = new List<(Color4, Vector2[])>();

        // Look wich action was handed over and call corresponding method
        public void PlayerControl(List<PlayerEnum.PlayerActions> actions, Controller.Input.MouseCursor mouseCursor)
        {
            foreach (PlayerEnum.PlayerActions playerAction in actions)
            {
                if (playerAction == PlayerEnum.PlayerActions.MoveUp || playerAction == PlayerEnum.PlayerActions.MoveDown || playerAction == PlayerEnum.PlayerActions.MoveLeft || playerAction == PlayerEnum.PlayerActions.MoveRight)
                {
                    this.playerActionList.Add(playerAction);
                    this.OnMove(EventArgs.Empty);
                }
                else if (playerAction == PlayerEnum.PlayerActions.Attack)
                {
                    this.playerAttackSystem.PlayerAttack();
                }
                else if (playerAction == PlayerEnum.PlayerActions.Interaction)
                {
                    this.playerInteractionSystem.PlayerInteraction(this);
                }
                else if (playerAction == PlayerEnum.PlayerActions.Run)
                {
                    this.run = true;
                }
                else if (playerAction == PlayerEnum.PlayerActions.Dash)
                {
                    this.playerMovementSystem.PlayerDash();
                }
            }

            this.directionXY = this.playerMovementSystem.SetPlayerDirection(this.playerActionList);
            this.playerActionList.Clear();
        }

        // Needs EventHandler from Npc who hits player
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
                Console.WriteLine("git gud, u died");
                this.OnDeath(EventArgs.Empty);
                //this.model.DestroyObject(this, null, null);
            }
        }

        public void IncreasePlayerStats(int incNumber, IReadOnlyList<PlayerEnum.Stats> incstats)
        {
            if (incNumber <= 0)
            {
                throw new View.Exceptions.ArgumentNotPositiveIntegerGreaterZeroException(nameof(incNumber));
            }

            foreach (PlayerEnum.Stats stats in incstats)
            {
                if (stats == PlayerEnum.Stats.Defense)
                {
                    this.Defense += incNumber;
                }
                else if (stats == PlayerEnum.Stats.MaxHealth)
                {
                    this.MaxHealth += incNumber;
                }
                else if (stats == PlayerEnum.Stats.MovementSpeed)
                {
                    this.MovementSpeed += incNumber;
                }

                this.OnInc(EventArgs.Empty);
            }
        }

        public void Update(float dtime)
        {
            if (this.run)
            {
                this.Position += this.directionXY * this.MovementSpeed * dtime * 2;
                this.run = false;
            }

            this.Position += this.directionXY * this.MovementSpeed * dtime;

            this.directionXY = Vector2.Zero;
        }

        public void OnInc(EventArgs e)
        {
            this.PlayerInc?.Invoke(this, e);
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
    }
}