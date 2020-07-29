using ComputergrafikSpiel.Model.Character.NPC.NPCAI;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.World;
using OpenTK;
using System;

namespace ComputergrafikSpiel.Model.Character.NPC.Interfaces
{
    public class DashEnemy : Enemy
    {
        private readonly Vector2 scale;

        public DashEnemy(Vector2 startposition, string texture, WorldEnum.Type type)
        {
            Console.WriteLine("Create Dash");
            this.Position = startposition;
            var collisionMask = ColliderLayer.Layer.Bullet | ColliderLayer.Layer.Player | ColliderLayer.Layer.Wall | ColliderLayer.Layer.Water;
            this.Collider = new CircleOffsetCollider(this, Vector2.Zero, 17, ColliderLayer.Layer.Enemy, collisionMask);

            switch (type)
            {
                case WorldEnum.Type.Water:
                    this.SetEnemyStats(20, 5, 1, 1);
                    this.Texture = new TextureLoader().LoadTexture("NPC/Enemy/Water/" + texture);
                    break;
                case WorldEnum.Type.Earth:
                    this.SetEnemyStats(40, 10, 2, 2);
                    break;
                case WorldEnum.Type.Fire:
                    this.SetEnemyStats(60, 15, 3, 3);
                    break;
                case WorldEnum.Type.Air:
                    this.SetEnemyStats(80, 20, 4, 4);
                    break;
                default: break;
            }

            this.scale = new Vector2(20, 20);
            this.Scale = this.scale;
            this.NPCController = new AIEnemy();
        }
    }
}
