using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.Character.Player
{
    internal class PlayerHandler : IPlayerControl
    {
        private List<PlayerEnum.PlayerActions> playerActionList;
        private Vector2 directionXY;

        public PlayerHandler()
        {
            this.CurrentHealth = this.MaxHealth;
            this.playerActionList = new List<PlayerEnum.PlayerActions>();
            this.Position = new Vector2(50, 50);
        }

        // Define Player
        public event EventHandler CharacterDeath;

        public event EventHandler CharacterHit;

        public event EventHandler CharacterMove;

        public int CurrentHealth { get; set; }

        public int MaxHealth { get; set; } = 5;

        public int MovementSpeed { get; set; } = 1;

        public int Defense { get; set; } = 1;

        public Vector2 Position { get; set; } = Vector2.Zero;

        public Vector2 Scale { get; } = Vector2.One * 20;

        public float Rotation { get; } = 0f;

        public Vector2 RotationAnker { get; } = Vector2.Zero;

        public ITexture Texture { get; } = null;

        // Look wich action was handed over and call corresponding method
        public void PlayerControl(IReadOnlyList<PlayerEnum.PlayerActions> actions)
        {
            foreach (PlayerEnum.PlayerActions playerAction in actions)
            {
                if (playerAction == PlayerEnum.PlayerActions.MoveUp || playerAction == PlayerEnum.PlayerActions.MoveDown || playerAction == PlayerEnum.PlayerActions.MoveLeft || playerAction == PlayerEnum.PlayerActions.MoveRight)
                {
                    this.playerActionList.Add(playerAction);
                    this.PlayerMovement(this.playerActionList);
                    Console.WriteLine("Added Event");
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

                this.playerActionList.Clear();
            }
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

        private void PlayerInteraction()
        {
            // TODO: Interaction System => Need Collider and NPC
        }

        // Determines in which direction the player moves
        private void PlayerMovement(IReadOnlyList<PlayerEnum.PlayerActions> movement)
        {
            foreach (PlayerEnum.PlayerActions direction in movement)
            {
                if (direction == PlayerEnum.PlayerActions.MoveUp)
                {
                    this.directionXY.X = 0;
                    this.directionXY.Y = 1;
                }
                else if (direction == PlayerEnum.PlayerActions.MoveDown)
                {
                    this.directionXY.X = 0;
                    this.directionXY.Y = -1;
                }
                else if (direction == PlayerEnum.PlayerActions.MoveRight)
                {
                    this.directionXY.X = 1;
                    this.directionXY.Y = 0;
                }
                else if (direction == PlayerEnum.PlayerActions.MoveLeft)
                {
                    this.directionXY.X = -1;
                    this.directionXY.Y = 0;
                }
            }

            this.Position = this.Position + (this.directionXY * this.MovementSpeed);
            Console.WriteLine("Player Position: " + this.Position);
        }

        private void PlayerAttack()
        {
            // TODO: Attacking => need Collider, NPC and Weapon. This Method should call a Attack Method in Weapon class
        }
    }
}
