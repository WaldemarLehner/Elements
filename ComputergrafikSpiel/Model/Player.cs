using System;
using System.Collections.Generic;
using OpenTK;

namespace ComputergrafikSpiel.Model
{
    internal class Player : IPlayerControl
    {
        private List<PlayerActionEnum.PlayerActions> playerActionList;

        public Player()
        {
            this.currentHealth = this.maxHealth;
            this.playerActionList = new List<PlayerActionEnum.PlayerActions>();
        }

        public int movementSpeed = 10;
        public int defense = 1;
        public int attackDamage = 1;
        public int maxHealth = 5;
        private int currentHealth;

        // Look wich action was handed over and call corresponding method
        public void PlayerControl(IReadOnlyList<PlayerActionEnum.PlayerActions> actions)
        {
            foreach (PlayerActionEnum.PlayerActions playerAction in actions)
            {
                Console.WriteLine(playerAction);
                if (playerAction == PlayerActionEnum.PlayerActions.MoveUp || playerAction == PlayerActionEnum.PlayerActions.MoveDown || playerAction == PlayerActionEnum.PlayerActions.MoveLeft || playerAction == PlayerActionEnum.PlayerActions.MoveRight)
                {
                    this.playerActionList.Add(playerAction);
                    this.PlayerMovement(this.playerActionList);
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

        // Shall be called in Enemie OnHit Method => calculate the amount of damage the player gets minus his defense
        public void PlayerTakingDamage(int damage)
        {
            if (damage > this.defense)
            {
                this.currentHealth -= damage - this.defense;
                if (this.currentHealth <= 0)
                {
                    // TODO: Die function (Event)
                }
            }
        }

        private void PlayerInteraction()
        {
            // TODO: Interaction System (Event)
        }

        // Determines in which direction the player moves
        private void PlayerMovement(IReadOnlyList<PlayerActionEnum.PlayerActions> movement)
        {
            foreach (PlayerActionEnum.PlayerActions direction in movement)
            {
                if (direction == PlayerActionEnum.PlayerActions.MoveUp)
                {
                    // TODO: Move +Y
                }
                else if (direction == PlayerActionEnum.PlayerActions.MoveDown)
                {
                    // TODO: Move -Y
                }
                else if (direction == PlayerActionEnum.PlayerActions.MoveRight)
                {
                    // TODO: Move +X
                }
                else if (direction == PlayerActionEnum.PlayerActions.MoveLeft)
                {
                    // TODO: Move -X
                }
            }
        }

        private void PlayerAttack()
        {
            // TODO: Attacking -> Check Collider hits anything (Event)
        }

        // Just a test => can be deleted
        public class PlayerRenderTest : IRenderable
        {
            public Vector2 Position { get; set; } = Vector2.Zero;

            public Vector2 Scale { get; set; } = Vector2.One * 20;

            public float Rotation { get; set; } = 0f;

            public Vector2 RotationAnker { get; set; } = Vector2.Zero;

            public ITexture Texture { get; set; } = null;
        }
    }

}
