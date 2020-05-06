using System;
using System.Collections.Generic;
using OpenTK;

namespace ComputergrafikSpiel.Model
{
    internal class Player : IPlayerControl
    {
        public Player()
        {
            this.currentHealth = this.maxHealth;
        }

        public int movementSpeed = 10;
        public int defense = 1;
        public int attackDamage = 1;
        public int maxHealth = 5;
        private int currentHealth;

        public void PlayerControl(IReadOnlyList<PlayerActionEnum.PlayerActions> actions)
        {
            foreach (PlayerActionEnum.PlayerActions playerAction in actions)
            {
                Console.WriteLine(playerAction);
                if (playerAction == PlayerActionEnum.PlayerActions.MoveUp || playerAction == PlayerActionEnum.PlayerActions.MoveDown || playerAction == PlayerActionEnum.PlayerActions.MoveLeft || playerAction == PlayerActionEnum.PlayerActions.MoveRight)
                {
                    this.PlayerMovement(playerAction);
                }
                else if (playerAction == PlayerActionEnum.PlayerActions.Attack)
                {
                    this.PlayerAttack();
                }
                else if (playerAction == PlayerActionEnum.PlayerActions.Interaction)
                {
                    this.PlayerInteraction();
                }
            }
        }

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

        private void PlayerMovement(PlayerActionEnum.PlayerActions direction)
        {
            // TODO: Movement -> Check if Collider hits anything
        }

        private void PlayerAttack()
        {
            // TODO: Attacking -> Check Collider hits anything (Event)
        }
    }
}
