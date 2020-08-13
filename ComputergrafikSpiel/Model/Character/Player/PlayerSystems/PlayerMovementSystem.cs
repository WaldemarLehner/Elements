using System.Collections.Generic;
using System.Threading.Tasks;
using OpenTK;

namespace ComputergrafikSpiel.Model.Character.Player.PlayerSystems
{
    internal class PlayerMovementSystem
    {
        private const float Multiplier = 3f;

        public float DashMultiplier { get; private set; } = 1f;

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

        public void PlayerDash()
        {
            this.DashMultiplier = Multiplier;
            Scene.Scene.Player.Invulnerable = true;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            this.ReduceSpeedAfterDash();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            return;
        }

        private async Task ReduceSpeedAfterDash()
        {
            await Task.Delay(150);
            this.DashMultiplier = 1f;
            Scene.Scene.Player.Invulnerable = false;
            return;
        }
    }
}
