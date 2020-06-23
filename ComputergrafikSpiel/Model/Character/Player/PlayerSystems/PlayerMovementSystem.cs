using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenTK;

namespace ComputergrafikSpiel.Model.Character.Player.PlayerSystems
{
    internal class PlayerMovementSystem
    {
        float setMovementspeedToValueBefore;

        // Determines in which direction the player moves
        public Vector2 SetPlayerDirection(IReadOnlyList<PlayerEnum.PlayerActions> movement)
        {
            Vector2 dir = Vector2.Zero;

            if (movement.Count == 0)
            {
                return Vector2.Zero;
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

            return dir.Normalized();
        }

        public void PlayerDash(Player player)
        {
            this.setMovementspeedToValueBefore = player.MovementSpeed;
            Console.WriteLine("DAshh");
            player.MovementSpeed = 700;
            this.ReduceSpeedAfterDash(player);
            return;
        }

        public async Task ReduceSpeedAfterDash(Player player)
        {
            await Task.Delay(20);
            player.MovementSpeed = this.setMovementspeedToValueBefore;
            return;
        }
    }
}
