using System;
using System.Collections.Generic;
using System.Linq;
using ComputergrafikSpiel.Model.Character.NPC.Interfaces;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using ComputergrafikSpiel.Model.Triggers.Interfaces;
using OpenTK;
using OpenTK.Graphics;

namespace ComputergrafikSpiel.Model.Triggers
{
    internal class Trigger : ITrigger
    {
        private List<INonPlayerCharacter> enemyCount;
        private ColliderLayer.Layer activators;
        private bool setAsPassive;

        public Trigger(Vector2 position, ColliderLayer.Layer activators, bool passive)
        {
            // radius may have to be changed
            this.enemyCount = Scene.Scene.Current.NPCs.ToList();
            this.activators = activators;
            this.Position = position;
            this.Texture = new TextureLoader().LoadTexture("Door/TreeBranchesDoor");
            this.Scale = new Vector2(16, 16);
            if (!passive)
            {
                this.Collider = new CircleOffsetCollider(this, new Vector2(-1, 0), 16, ColliderLayer.Layer.Trigger, this.activators);
            }
            else
            {
                this.Collider = new CircleOffsetCollider(this, Vector2.Zero, 16, ColliderLayer.Layer.Trigger, this.activators);
            }
            this.setAsPassive = passive;
        }

        public ICollider Collider { get; private set; }

        public Vector2 Position { get; }

        public float Rotation { get; } = 0f;

        public Vector2 RotationAnker { get; } = Vector2.Zero;

        // may have to be changed with the collider radius
        public Vector2 Scale { get; set; } = Vector2.One * 16;

        public ITexture Texture { get; }

        public IEnumerable<(Color4 color, Vector2[] vertices)> DebugData => null;

        public void TriggerCollisionFunction()
        {
            if (!this.setAsPassive)
            {
                Scene.Scene.Current.OnChangeScene();
                Scene.Scene.Player.ChangePosition();
            }
        }

        public void Update(float dtime)
        {
        }
    }
}
