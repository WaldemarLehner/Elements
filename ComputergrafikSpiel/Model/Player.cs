using System;
using System.Collections.Generic;
using OpenTK;

namespace ComputergrafikSpiel.Model
{
    internal class Player : IPlayerControl
    {
        private List<PlayerActionEnum.PlayerActions> playerActionList;
        private Vector2 directionXY;

        public Player()
        {
            this.CurrentHealth = this.MaxHealth;
            this.playerActionList = new List<PlayerActionEnum.PlayerActions>();
            this.Position = new Vector2(50, 50);
        }

        // Define Player
        public event EventHandler CharacterDeath;

        public event EventHandler CharacterHit;

        public event EventHandler CharacterMove;

        public int CurrentHealth { get; set; }

        public int MaxHealth { get; } = 5;

        public int MovementSpeed { get; } = 1;

        public int AttackSpeed { get; } = 1;

        public ITexture Texture { get; } = null;

        public Vector2 Position { get; set; } = Vector2.Zero;

        public Vector2 Scale { get; } = Vector2.One * 20;

        public float Rotation { get; } = 0f;

        public Vector2 RotationAnker { get; } = Vector2.Zero;

        // Look wich action was handed over and call corresponding method
        public void PlayerControl(IReadOnlyList<PlayerActionEnum.PlayerActions> actions)
        {
            foreach (PlayerActionEnum.PlayerActions playerAction in actions)
            {
                if (playerAction == PlayerActionEnum.PlayerActions.MoveUp || playerAction == PlayerActionEnum.PlayerActions.MoveDown || playerAction == PlayerActionEnum.PlayerActions.MoveLeft || playerAction == PlayerActionEnum.PlayerActions.MoveRight)
                {
                    this.playerActionList.Add(playerAction);
                    this.PlayerMovement(this.playerActionList);
                    this.OnMove(EventArgs.Empty);
                }
                else if (playerAction == PlayerActionEnum.PlayerActions.Attack)
                {
                    this.PlayerAttack();
                }
                else if (playerAction == PlayerActionEnum.PlayerActions.Interaction)
                {
                    this.PlayerInteraction();
                }

                this.playerActionList.Clear();
            }
        }

        public void TakingDamage(int damage)
        {
            this.CurrentHealth -= damage;
            this.OnHit(EventArgs.Empty);
            if (this.CurrentHealth >= 0)
            {
                this.OnDeath(EventArgs.Empty);
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
        private void PlayerMovement(IReadOnlyList<PlayerActionEnum.PlayerActions> movement)
        {
            foreach (PlayerActionEnum.PlayerActions direction in movement)
            {
                if (direction == PlayerActionEnum.PlayerActions.MoveUp)
                {
                    this.directionXY.X = 0;
                    this.directionXY.Y = 1;
                }
                else if (direction == PlayerActionEnum.PlayerActions.MoveDown)
                {
                    this.directionXY.X = 0;
                    this.directionXY.Y = -1;
                }
                else if (direction == PlayerActionEnum.PlayerActions.MoveRight)
                {
                    this.directionXY.X = 1;
                    this.directionXY.Y = 0;
                }
                else if (direction == PlayerActionEnum.PlayerActions.MoveLeft)
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
            // TODO: Attacking => need Collider and NPC
        }
    }
}
