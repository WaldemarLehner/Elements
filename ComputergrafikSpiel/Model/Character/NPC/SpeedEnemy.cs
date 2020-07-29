﻿using ComputergrafikSpiel.Model.Character.NPC.NPCAI;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.World;
using OpenTK;
using System;

namespace ComputergrafikSpiel.Model.Character.NPC
{
    public class SpeedEnemy : Enemy
    {
        public SpeedEnemy(Vector2 startposition, string texture, WorldEnum.Type type)
        {
            Console.WriteLine("Creating Speed");
            this.Position = startposition;
            var collisionMask = ColliderLayer.Layer.Bullet | ColliderLayer.Layer.Player | ColliderLayer.Layer.Wall | ColliderLayer.Layer.Water;
            this.Collider = new CircleOffsetCollider(this, Vector2.Zero, 17, ColliderLayer.Layer.Enemy, collisionMask);

            switch (type)
            {
                case WorldEnum.Type.Water:
                    this.SetEnemyStats(20, 100, 1, 1);
                    this.Texture = new TextureLoader().LoadTexture("NPC/Enemy/Water/" + texture);
                    this.BloodColorHue = 241f;
                    break;
                case WorldEnum.Type.Earth:
                    this.SetEnemyStats(40, 110, 2, 2);
                    this.Texture = new TextureLoader().LoadTexture("NPC/Enemy/Earth/" + texture);
                    this.BloodColorHue = 96f;
                    break;
                case WorldEnum.Type.Fire:
                    this.SetEnemyStats(60, 120, 3, 3);
                    this.Texture = new TextureLoader().LoadTexture("NPC/Enemy/Fire/" + texture);
                    this.BloodColorHue = 51f;
                    break;
                case WorldEnum.Type.Air:
                    this.SetEnemyStats(80, 20, 4, 4);
                    this.Texture = new TextureLoader().LoadTexture("NPC/Enemy/Air/" + texture);
                    this.BloodColorHue = 0f;
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
