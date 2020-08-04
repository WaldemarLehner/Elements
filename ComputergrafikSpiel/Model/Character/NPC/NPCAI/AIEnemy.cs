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
        private Ray ray;

        private float DashCooldown { get; set; } = 0;

        private float MovementCooldown { get; set; } = 0;

        private float AfterMovementCooldown { get; set; } = 0;

        private Vector2 RandomDirectionSave { get; set; }

        public Vector2 EnemyAIMovement(INonPlayerCharacter myself, float dtime)
        {
            this.DashCooldown -= dtime;
            var direction = Scene.Scene.Player.Collider.Position - myself.Collider.Position;

            if (this.LookForPlayer(myself, direction))
            {
                if (myself.Variant == EnemyEnum.Variant.Range)
                {
                    myself.ShootBullet(dtime);
                    return this.MoveTowardsPlayer(direction, new Vector2(10, 10));
                }
                else if (myself.Variant == EnemyEnum.Variant.Dash)
                {
                    if (this.DashCooldown <= 0)
                    {
                        myself.Dash();
                        this.DashCooldown = 1;
                    }
                }
                else if (myself.Variant == EnemyEnum.Variant.Boss)
                {
                    myself.ShootBullet(dtime);
                    if (this.DashCooldown <= 0)
                    {
                        myself.Dash();
                        this.DashCooldown = 1;
                    }
                }

                myself.GiveDamageToPlayer();

                return this.MoveTowardsPlayer(direction, new Vector2(0, 0));
            }
            else
            {
                return this.MoveRandom(dtime);
            }
        }

        private Vector2 MoveRandom(float dtime)
        {
            Random random = new Random();

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

        private bool LookForPlayer(INonPlayerCharacter myself, Vector2 direction)
        {
            if (myself.Air)
            {
                this.ray = new Ray(myself.Collider.Position, direction, 500, ColliderLayer.Layer.Player);
            }
            else
            {
                this.ray = new Ray(myself.Collider.Position, direction, 500, ColliderLayer.Layer.Player | ColliderLayer.Layer.Wall | ColliderLayer.Layer.Water);
            }

            var collidables = Scene.Scene.Current.ColliderManager.GetRayCollisionsSorted(this.ray, this.ray.Position);

            var first = (from col in collidables where col != myself orderby Vector2.DistanceSquared(col.Collider.Position, myself.Position) - col.Collider.MaximumDistanceFromPosition ascending select col).FirstOrDefault();
            if (first == null || !(first is IPlayer))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private Vector2 MoveTowardsPlayer(Vector2 direction, Vector2 offset)
        {
            var truedirection = direction - offset;
            truedirection.Normalize();
            return truedirection;
        }
    }
}
