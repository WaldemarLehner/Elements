﻿using ComputergrafikSpiel.Model.Character.NPC.NPCAI;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.World;
using OpenTK;

namespace ComputergrafikSpiel.Model.Character.NPC
{
    public class SpeedEnemy : Enemy
    {
        public SpeedEnemy(Vector2 startposition, string texture, WorldEnum.Type type)
        {
            this.Position = startposition;
            this.Variant = EnemyEnum.Variant.Speed;

            switch (type)
            {
                case WorldEnum.Type.Water:
                    this.SetEnemyStats(20, 100, 1);
                    this.Texture = new TextureLoader().LoadTexture("NPC/Enemy/Water/" + texture);
                    this.BloodColorHue = 241f;
                    this.Collider = new CircleOffsetCollider(this, Vector2.Zero, 15, ColliderLayer.Layer.Enemy, this.SetEnemyCollider(true));
                    this.Air = true;
                    break;
                case WorldEnum.Type.Earth:
                    this.SetEnemyStats(40, 110, 1);
                    this.Texture = new TextureLoader().LoadTexture("NPC/Enemy/Earth/" + texture);
                    this.BloodColorHue = 96f;
                    this.Collider = new CircleOffsetCollider(this, Vector2.Zero, 15, ColliderLayer.Layer.Enemy, this.SetEnemyCollider(false));
                    this.Air = false;
                    break;
                case WorldEnum.Type.Fire:
                    this.SetEnemyStats(60, 120, 2);
                    this.Texture = new TextureLoader().LoadTexture("NPC/Enemy/Fire/" + texture);
                    this.BloodColorHue = 51f;
                    this.Collider = new CircleOffsetCollider(this, Vector2.Zero, 15, ColliderLayer.Layer.Enemy, this.SetEnemyCollider(true));
                    this.Air = true;
                    break;
                case WorldEnum.Type.Air:
                    this.SetEnemyStats(80, 130, 2);
                    this.Texture = new TextureLoader().LoadTexture("NPC/Enemy/Air/" + texture);
                    this.BloodColorHue = 0f;
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
