﻿using ComputergrafikSpiel.Model.Character.NPC.NPCAI;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.World;
using OpenTK;

namespace ComputergrafikSpiel.Model.Character.NPC
{
    public class DashEnemy : Enemy
    {
        public DashEnemy(Vector2 startposition, string texture, WorldEnum.Type type)
        {
            this.Position = startposition;
            this.Variant = EnemyEnum.Variant.Dash;

            switch (type)
            {
                case WorldEnum.Type.Water:
                    this.SetEnemyStats(20, 70, 1);
                    this.Texture = new TextureLoader().LoadTexture("NPC/Enemy/Water/" + texture);
                    this.BloodColorHue = 115f;
                    this.Collider = new CircleOffsetCollider(this, Vector2.Zero, 15, ColliderLayer.Layer.Enemy, this.SetEnemyCollider(false));
                    this.Air = false;
                    break;
                case WorldEnum.Type.Earth:
                    this.SetEnemyStats(40, 75, 1);
                    this.Texture = new TextureLoader().LoadTexture("NPC/Enemy/Earth/" + texture);
                    this.BloodColorHue = 100f;
                    this.Collider = new CircleOffsetCollider(this, Vector2.Zero, 15, ColliderLayer.Layer.Enemy, this.SetEnemyCollider(false));
                    this.Air = false;
                    break;
                case WorldEnum.Type.Fire:
                    this.SetEnemyStats(60, 80, 2);
                    this.Texture = new TextureLoader().LoadTexture("NPC/Enemy/Fire/" + texture);
                    this.BloodColorHue = 345f;
                    this.Collider = new CircleOffsetCollider(this, Vector2.Zero, 15, ColliderLayer.Layer.Enemy, this.SetEnemyCollider(false));
                    this.Air = false;
                    break;
                case WorldEnum.Type.Air:
                    this.SetEnemyStats(80, 85, 2);
                    this.Texture = new TextureLoader().LoadTexture("NPC/Enemy/Air/" + texture);
                    this.BloodColorHue = 285f;
                    this.Collider = new CircleOffsetCollider(this, Vector2.Zero, 15, ColliderLayer.Layer.Enemy, this.SetEnemyCollider(true));
                    this.Air = true;
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
