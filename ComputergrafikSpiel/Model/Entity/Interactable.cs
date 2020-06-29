using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model.Character.Player;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using OpenTK;
using OpenTK.Graphics;

namespace ComputergrafikSpiel.Model.Entity
{
    public class Interactable : IEntity
    {
        private readonly string texturename;
        private int incNumber;
        private PlayerEnum.Stats stats;
        public bool singleDelete = false;

        public Interactable(PlayerEnum.Stats stats, float positionX, float positionY, int incNumber)
        {
            this.Scale = new Vector2(10, 10);
            this.Position = new Vector2(positionX, positionY);
            this.Collider = new CircleOffsetCollider(this, Vector2.Zero, 10, ColliderLayer.Layer.Interactable, ColliderLayer.Layer.Player);

            switch (stats)
            {
                case PlayerEnum.Stats.MaxHealth:
                    this.texturename = "MaxHealthIncrease";
                    this.incNumber = incNumber;
                    this.stats = stats;
                    break;
                case PlayerEnum.Stats.Heal:
                    this.texturename = "HealIncrease";
                    this.incNumber = incNumber;
                    this.stats = stats;
                    this.Scale = new Vector2(5, 5);
                    this.singleDelete = true;
                    break;
                case PlayerEnum.Stats.Defense:
                    this.texturename = "DefenseIncrease";
                    this.incNumber = incNumber;
                    this.stats = stats;
                    break;
                case PlayerEnum.Stats.AttackSpeed:
                    this.texturename = "AttackSpeedIncrease";
                    this.incNumber = incNumber;
                    this.stats = stats;
                    break;
                case PlayerEnum.Stats.MovementSpeed:
                    this.texturename = "MovementSpeedIncrease";
                    this.incNumber = incNumber;
                    this.stats = stats;
                    break;
                case PlayerEnum.Stats.Währung:
                    this.texturename = "Währung";
                    this.incNumber = incNumber;
                    this.stats = stats;
                    this.Scale = new Vector2(5, 5);
                    this.singleDelete = true;
                    break;
                default:
                    Console.WriteLine("ENUM STATS DOES NOT EXIST.");
                    break;
            }

            this.Texture = new ComputergrafikSpiel.Model.EntitySettings.Texture.TextureLoader().LoadTexture("StatIncrease/" + this.texturename);
        }

        public ICollider Collider { get; set; }

        public ITexture Texture { get; } = null;

        public Vector2 Position { get; set; } = Vector2.Zero;

        public float Rotation { get; } = 0f;

        public Vector2 RotationAnker { get; } = Vector2.Zero;

        public Vector2 Scale { get; }

        public IEnumerable<(Color4 color, Vector2[] vertices)> DebugData => new (Color4 color, Vector2[] vertices)[] { this.Collider.DebugData };

        public void Update(float dtime)
        {
        }

        public void PlayerStatsIncrease()
        {
            Scene.Scene.Player.IncreasePlayerStats(this.incNumber, this.stats);
        }

        public void RemoveInteractable()
        {
            Scene.Scene.Current.RemoveObject(this);
        }
    }
}
