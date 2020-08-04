using ComputergrafikSpiel.Model.Character.NPC.NPCAI;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.World;
using OpenTK;

namespace ComputergrafikSpiel.Model.Character.NPC
{
    public class TankEnemy : Enemy
    {
        public TankEnemy(Vector2 startposition, string texture, WorldEnum.Type type)
        {
            this.Position = startposition;
            var collisionMask = ColliderLayer.Layer.Bullet | ColliderLayer.Layer.Player | ColliderLayer.Layer.Wall | ColliderLayer.Layer.Water;
            this.Collider = new CircleOffsetCollider(this, Vector2.Zero, 17, ColliderLayer.Layer.Enemy, collisionMask);

            switch (type)
            {
                case WorldEnum.Type.Water:
                    this.SetEnemyStats(20, 50, 1, 1);
                    this.Texture = new TextureLoader().LoadTexture("NPC/Enemy/Water/" + texture);
                    this.BloodColorHue = 0f;
                    break;
                case WorldEnum.Type.Earth:
                    this.SetEnemyStats(40, 55, 2, 2);
                    this.Texture = new TextureLoader().LoadTexture("NPC/Enemy/Earth/" + texture);
                    this.BloodColorHue = 34f;
                    break;
                case WorldEnum.Type.Fire:
                    this.SetEnemyStats(60, 60, 3, 3);
                    this.Texture = new TextureLoader().LoadTexture("NPC/Enemy/Fire/" + texture);
                    this.BloodColorHue = 345f;
                    break;
                case WorldEnum.Type.Air:
                    this.SetEnemyStats(80, 65, 4, 4);
                    this.Texture = new TextureLoader().LoadTexture("NPC/Enemy/Air/" + texture);
                    this.BloodColorHue = 58f;
                    break;
                default: break;
            }

            this.CurrentHealth = this.MaxHealth;

            this.Scale = new Vector2(20, 20);
            this.SetScale();

            this.NPCController = new AIEnemy();
        }
    }
}
