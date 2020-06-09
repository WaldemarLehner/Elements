using System;
using System.Collections.Generic;
using System.Linq;
using ComputergrafikSpiel.Model.Character.NPC.Interfaces;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.Character.NPC.NPCAI
{
    public class AIEnemy : INPCController
    {
        private IRay ray;
        private Vector2 direction;
        private IColliderManager colliderManager;
        private List<ICollidable> collidables;

        public AIEnemy(IColliderManager colliderManager)
        {
            this.colliderManager = colliderManager;
            this.collidables = new List<ICollidable>();
        }

        public Vector2 EnemyAI(INonPlayerCharacter enemy, IPlayer player)
        {
            this.direction = player.Position - enemy.Position;
            this.direction.Normalize();

            this.ray = new Ray(enemy.Position, this.direction, 500);
            this.collidables = (List<ICollidable>)this.colliderManager.GetRayCollisions(this.ray, enemy.Position);
            this.collidables.Remove(enemy.Collider.CollidableParent);

            if (this.collidables.FirstOrDefault<ICollidable>() == player.Collider.CollidableParent)
            {
                return this.direction;
            }

            return Vector2.Zero;
        }
    }
}
