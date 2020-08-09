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
            this.Position = startposition;
            var collisionMask = ColliderLayer.Layer.Bullet | ColliderLayer.Layer.Player | ColliderLayer.Layer.Wall | ColliderLayer.Layer.Water;
            this.Collider = new CircleOffsetCollider(this, Vector2.Zero, 15, ColliderLayer.Layer.Enemy, collisionMask);
            this.Variant = EnemyEnum.Variant.Boss;

            switch (type)
            {
                case WorldEnum.Type.Water:
                    this.SetEnemyStats(200, 70, 1);
                    this.Texture = new TextureLoader().LoadTexture("NPC/Boss/" + texture);
                    this.BloodColorHue = 13f;
                    this.ProjectileHue = (348f, 19f);
                    this.AttackCooldown = 2f;
                    break;
                case WorldEnum.Type.Earth:
                    this.SetEnemyStats(300, 75, 2);
                    this.Texture = new TextureLoader().LoadTexture("NPC/Boss/" + texture);
                    this.BloodColorHue = 51f;
                    this.ProjectileHue = (64f, 47f);
                    this.AttackCooldown = 1.5f;
                    break;
                case WorldEnum.Type.Fire:
                    this.SetEnemyStats(400, 80, 3);
                    this.Texture = new TextureLoader().LoadTexture("NPC/Boss/" + texture);
                    this.BloodColorHue = 25f;
                    this.ProjectileHue = (0f, 50f);
                    this.AttackCooldown = 1f;
                    break;
                case WorldEnum.Type.Air:
                    this.SetEnemyStats(600, 85, 4);
                    this.Texture = new TextureLoader().LoadTexture("NPC/Boss/" + texture);
                    this.BloodColorHue = 0f;
                    this.ProjectileHue = (261f, 295f);
                    this.AttackCooldown = .5f;
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
