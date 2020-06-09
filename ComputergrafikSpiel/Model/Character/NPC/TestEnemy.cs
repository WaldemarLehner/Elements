using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model.Character.NPC.Interfaces;
using ComputergrafikSpiel.Model.Character.NPC.NPCAI;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using OpenTK;
using OpenTK.Graphics;

namespace ComputergrafikSpiel.Model.Character.NPC
{
    public class TestEnemy : INonPlayerCharacter
    {
        public TestEnemy(int maxHealth, string texture, IPlayer player, IColliderManager colliderManager)
        {
            this.MaxHealth = maxHealth;
            this.Texture = new TextureLoader().LoadTexture("Enemy/" + texture);
            this.Position = new Vector2(400, 500);
            this.Scale = new Vector2(16, 16);
            this.Collider = new CircleOffsetCollider(this, Vector2.Zero, 10);
            this.Player = player;
            colliderManager.AddEntityCollidable(this.Collider.CollidableParent);
            this.NPCController = new AIEnemy(colliderManager);

        }

        public event EventHandler CharacterDeath;

        public event EventHandler CharacterHit;

        public event EventHandler CharacterMove;

        public INPCController NPCController { get; }

        public int MaxHealth { get; }

        public float MovementSpeed { get; } = 35;

        public int Defense { get; }

        public ICollider Collider { get; }

        public ITexture Texture { get; } = null;

        public IEnumerable<(Color4 color, Vector2[] vertices)> DebugData { get; } = new List<(Color4, Vector2[])>();

        public Vector2 Position { get; set; } = Vector2.Zero;

        public float Rotation { get; } = 0f;

        public Vector2 RotationAnker { get; } = Vector2.Zero;

        public Vector2 Scale { get; } = Vector2.One * 20;

        private IPlayer Player { get; }

        private Vector2 Direction { get; set; }

        public void OnDeath(EventArgs e)
        {
            this.CharacterDeath?.Invoke(this, e);
        }

        public void OnHit(EventArgs e)
        {
            this.CharacterHit?.Invoke(this, e);
        }

        public void OnMove(EventArgs e)
        {
            this.CharacterMove?.Invoke(this, e);
        }

        public void Update(float dtime)
        {
            this.Direction = this.NPCController.EnemyAI(this, this.Player);

            this.Position += this.Direction * this.MovementSpeed * dtime;
        }
    }
}
