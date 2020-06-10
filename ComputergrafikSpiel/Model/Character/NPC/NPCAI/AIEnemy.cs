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
        private IPlayer player;
        private ICollection<INonPlayerCharacter> otherEnemys;

        public AIEnemy(IColliderManager colliderManager, ICollection<INonPlayerCharacter> otherEnemys, IPlayer player)
        {
            this.colliderManager = colliderManager;
            this.collidables = new List<ICollidable>();
            this.otherEnemys = otherEnemys;
            this.player = player;
        }

        public Vector2 EnemyAIMovement(INonPlayerCharacter myself)
        {
            this.direction = this.player.Position - myself.Position;
            this.direction.Normalize();

            this.ray = new Ray(myself.Position, this.direction, 500);
            this.collidables = (List<ICollidable>)this.colliderManager.GetRayCollisions(this.ray, myself.Position);
            this.collidables.Remove(myself.Collider.CollidableParent);
            foreach (INonPlayerCharacter otherEnemys in this.otherEnemys)
            {
                this.collidables.Remove(otherEnemys.Collider.CollidableParent);
            }

            if (this.collidables.FirstOrDefault<ICollidable>() == this.player.Collider.CollidableParent)
            {
                return this.direction;
            }

            return Vector2.Zero;
        }
    }
}
