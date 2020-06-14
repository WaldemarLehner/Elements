using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using ComputergrafikSpiel.Model.Character.NPC.Interfaces;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.Character.NPC.NPCAI
{
    public class AIEnemy : INPCController
    {
        public AIEnemy()
        {
        }

        public Vector2 EnemyAIMovement(INonPlayerCharacter myself)
        {
            var direction = Scene.Scene.Player.Position - myself.Position;
            direction.Normalize();

            var ray = new Ray(myself.Position, direction, 500, ColliderLayer.Layer.Player | ColliderLayer.Layer.Wall);
            var collidables = Scene.Scene.Current.ColliderManager.GetRayCollisions(ray, myself.Position);

            var first = (from col in collidables orderby Vector2.DistanceSquared(col.Collider.Position, myself.Position) - col.Collider.MaximumDistanceFromPosition ascending select col).FirstOrDefault();
            if (first == null || !(first is IPlayer))
            {
                // TODO: Walk around.
                return Vector2.Zero;
            }

            return direction;
        }
    }
}
