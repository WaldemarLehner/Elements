using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model.Character.Player;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace ComputergrafikSpiel.Model.Entity
{
    public class Interactable : IEntity
    {
        private readonly string texturename;
        private readonly PlayerEnum.Stats stat;

        public Interactable(PlayerEnum.Stats stat, float positionX, float positionY)
        {
            this.Scale = new Vector2(10, 10);
            this.Position = new Vector2(positionX, positionY);
            this.Collider = new CircleOffsetCollider(this, Vector2.Zero, 10, ColliderLayer.Layer.Interactable, ColliderLayer.Layer.Player);

            switch (stat)
            {
                case PlayerEnum.Stats.Heal:
                    this.texturename = "StatIncrease/HealIncrease";
                    this.stat = stat;
                    this.Scale = new Vector2(5, 5);
                    this.SingleDelete = true;
                    break;
                case PlayerEnum.Stats.Money:
                    this.texturename = "GUI/Upgrades/Coin";
                    this.stat = stat;
                    this.Scale = new Vector2(5, 5);
                    this.SingleDelete = true;
                    break;
                default:
                    Console.WriteLine("ENUM STATS DOES NOT EXIST.");
                    break;
            }

            this.Texture = new EntitySettings.Texture.TextureLoader().LoadTexture(this.texturename);
        }

        public bool SingleDelete { get; private set; } = false;

        public bool DeleteAll { get; private set; } = false;

        public ICollider Collider { get; set; }

        public ITexture Texture { get; } = null;

        public Vector2 Position { get; set; } = Vector2.Zero;

        public float Rotation { get; } = 0f;

        public Vector2 RotationAnker { get; } = Vector2.Zero;

        public Vector2 Scale { get; set; }

        public IEnumerable<(Color4 color, Vector2[] vertices)> DebugData => new (Color4 color, Vector2[] vertices)[] { this.Collider.DebugData };

        public void Update(float dtime)
        {
            IReadOnlyCollection<ICollidable> interactableCollision = Scene.Scene.Current.ColliderManager.GetCollisions(this);

            foreach (var collisions in interactableCollision)
            {
                foreach (var tileCollidable in Scene.Scene.Current.ColliderManager.CollidableTileDictionary)
                {
                    if (interactableCollision == tileCollidable.Value)
                    {
                        this.RemoveInteractable();
                    }
                }
            }
        }

        public void PlayerStatsIncrease()
        {
            switch (this.stat)
            {
                case PlayerEnum.Stats.Heal:
                    Scene.Scene.Player.TakeHeal();
                    return;
                case PlayerEnum.Stats.Money:
                    Scene.Scene.Player.TakeMoney();
                    return;
            }
        }

        public void RemoveInteractable()
        {
            Scene.Scene.Current.RemoveObject(this);
        }
    }
}
