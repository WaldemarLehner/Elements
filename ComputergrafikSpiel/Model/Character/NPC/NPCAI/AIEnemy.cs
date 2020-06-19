using System;
using System.Linq;
using ComputergrafikSpiel.Model.Character.NPC.Interfaces;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.Collider;
using OpenTK;

namespace ComputergrafikSpiel.Model.Character.NPC.NPCAI
{
    public class AIEnemy : INPCController
    {
        public AIEnemy()
        {
        }

        private float MovementCooldown { get; set; } = 0;

        private float AfterMovementCooldown { get; set; } = 0;

        private Vector2 RandomDirectionSave { get; set; }

        public Vector2 EnemyAIMovement(INonPlayerCharacter myself, float dtime)
        {
            var direction = Scene.Scene.Player.Position - myself.Position;
            direction.Normalize();
            Random random = new Random();

            var ray = new Ray(myself.Position, direction, 500, ColliderLayer.Layer.Player | ColliderLayer.Layer.Wall);
            var collidables = Scene.Scene.Current.ColliderManager.GetRayCollisions(ray, myself.Position);

            var first = (from col in collidables orderby Vector2.DistanceSquared(col.Collider.Position, myself.Position) - col.Collider.MaximumDistanceFromPosition ascending select col).FirstOrDefault();
            if (first == null || !(first is IPlayer))
            {
                this.MovementCooldown -= dtime;

                if (this.MovementCooldown <= 0)
                {
                    this.AfterMovementCooldown -= dtime;

                    if (this.AfterMovementCooldown <= 0)
                    {
                        this.MovementCooldown = random.Next(1, 4);
                        this.AfterMovementCooldown = random.Next(0, 2);
                        Vector2 randomDirection = new Vector2((float)random.Next(-500, 500), (float)random.Next(-500, 500));
                        randomDirection.Normalize();
                        this.RandomDirectionSave = randomDirection;
                        if (double.IsNaN(randomDirection.Y) || double.IsNaN(randomDirection.X))
                        {
                            return Vector2.Zero;
                        }

                        return randomDirection;
                    }

                    return Vector2.Zero;
                }

                return this.RandomDirectionSave;
            }

            return direction;
        }
    }
}
