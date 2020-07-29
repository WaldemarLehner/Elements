using System;
using ComputergrafikSpiel.Model.Character.NPC.NPCAI;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.World;
using OpenTK;

namespace ComputergrafikSpiel.Model.Character.NPC
{
    public class EnemyBoss : Enemy
    {
        public EnemyBoss(Vector2 startposition, string texture, WorldEnum.Type type)
        {
            Console.WriteLine("Create Boss");
            this.Position = startposition;
            var collisionMask = ColliderLayer.Layer.Bullet | ColliderLayer.Layer.Player | ColliderLayer.Layer.Wall | ColliderLayer.Layer.Water;
            this.Collider = new CircleOffsetCollider(this, Vector2.Zero, 17, ColliderLayer.Layer.Enemy, collisionMask);

            switch (type)
            {
                case WorldEnum.Type.Water:
                    this.SetEnemyStats(20, 70, 1, 1);
                    this.Texture = new TextureLoader().LoadTexture("NPC/Boss/" + texture);
                    this.BloodColorHue = 13f;
                    break;
                case WorldEnum.Type.Earth:
                    this.SetEnemyStats(40, 75, 2, 2);
                    this.Texture = new TextureLoader().LoadTexture("NPC/Boss/" + texture);
                    this.BloodColorHue = 51f;
                    break;
                case WorldEnum.Type.Fire:
                    this.SetEnemyStats(60, 80, 3, 3);
                    this.Texture = new TextureLoader().LoadTexture("NPC/Boss/" + texture);
                    this.BloodColorHue = 25f;
                    break;
                case WorldEnum.Type.Air:
                    this.SetEnemyStats(80, 85, 4, 4);
                    this.Texture = new TextureLoader().LoadTexture("NPC/Boss/" + texture);
                    this.BloodColorHue = 0f;
                    break;
                default: break;
            }

            this.CurrentHealth = this.MaxHealth;

            this.Scale = new Vector2(64, 64);
            this.SetScale();

            this.NPCController = new AIEnemy();
        }
    }
}
