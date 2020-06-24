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

        public Vector2 EnemyAIMovement(INonPlayerCharacter myself)
        {
            var direction = Scene.Scene.Player.Collider.Position - myself.Collider.Position;
            direction.Normalize();

            var ray = new Ray(myself.Collider.Position, direction, 500, ColliderLayer.Layer.Player | ColliderLayer.Layer.Wall | ColliderLayer.Layer.Water);
            var collidables = Scene.Scene.Current.ColliderManager.GetRayCollisionsSorted(ray, ray.Position);

            var first = (from col in collidables orderby Vector2.DistanceSquared(col.Collider.Position, myself.Position) - col.Collider.MaximumDistanceFromPosition ascending select col).FirstOrDefault();
            if (first == null || !(first is IPlayer))
            {
                if(first == null)
                {
                    if (true) ;
                }
                // TODO: Walk around.
                return Vector2.Zero;
            }

            return direction;
        }
    }
}
