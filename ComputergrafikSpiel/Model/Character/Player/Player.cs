using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.Character.Player
{
    internal class Player : IPlayer
    {
        private List<PlayerEnum.PlayerActions> playerActionList;
        private Vector2 directionXY = Vector2.Zero;
        private bool run = false;

        public Player()
        {
            this.CurrentHealth = this.MaxHealth;
            this.playerActionList = new List<PlayerEnum.PlayerActions>();
            this.Position = new Vector2(50, 50);
            this.Collider = new CircleOffsetCollider(this, Vector2.Zero, 10);
        }

        // Define Player
        public event EventHandler CharacterDeath;

        public event EventHandler CharacterHit;

        public event EventHandler CharacterMove;

        public int CurrentHealth { get; set; }

        public int MaxHealth { get; set; } = 5;

        public float MovementSpeed { get; set; } = 50;

        public int Defense { get; set; } = 1;

        public Vector2 Position { get; set; } = Vector2.Zero;

        public Vector2 Scale { get; } = Vector2.One * 20;

        public float Rotation { get; } = 0f;

        public Vector2 RotationAnker { get; } = Vector2.Zero;

        public ITexture Texture { get; } = null;

        public ICollider Collider { get; set; }

        // Look wich action was handed over and call corresponding method
        public void PlayerControl(IReadOnlyList<PlayerEnum.PlayerActions> actions)
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
                    this.PlayerAttack();
                }
                else if (playerAction == PlayerEnum.PlayerActions.Interaction)
                {
                    this.PlayerInteraction();
                }
                else if (playerAction == PlayerEnum.PlayerActions.Run)
                {
                    this.run = true;
                }
                else if (playerAction == PlayerEnum.PlayerActions.Dash)
                {
                    this.PlayerDash();
                }
            }

            this.SetPlayerDirection(this.playerActionList);
            this.playerActionList.Clear();
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
            }

            this.OnHit(EventArgs.Empty);
            if (this.CurrentHealth <= 0)
            {
                this.OnDeath(EventArgs.Empty);
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

        private void PlayerDash()
        {
            throw new NotImplementedException();
        }

        private void PlayerInteraction()
        {
            // TODO: Interaction System => Need Collider and NPC
        }

        // Determines in which direction the player moves
        private void SetPlayerDirection(IReadOnlyList<PlayerEnum.PlayerActions> movement)
        {
            Vector2 dir = Vector2.Zero;

            if (movement.Count == 0)
            {
                return;
            }

            foreach (PlayerEnum.PlayerActions direction in movement)
            {
                if (direction == PlayerEnum.PlayerActions.MoveUp)
                {
                    dir.Y = 1;
                }
                else if (direction == PlayerEnum.PlayerActions.MoveDown)
                {
                    dir.Y = -1;
                }
                else if (direction == PlayerEnum.PlayerActions.MoveRight)
                {
                    dir.X = 1;
                }
                else if (direction == PlayerEnum.PlayerActions.MoveLeft)
                {
                    dir.X = -1;
                }
            }

            this.directionXY = dir.Normalized();

            // Console.WriteLine(this.directionXY);
        }

        private void PlayerAttack()
        {
            // TODO: Attacking => need Collider, NPC and Weapon. This Method should call a Attack Method in Weapon class
        }
    }
}