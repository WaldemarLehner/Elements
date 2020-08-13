using System.Collections.Generic;
using System.Linq;
using ComputergrafikSpiel.Model.Character.NPC.Interfaces;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using ComputergrafikSpiel.Model.Triggers.Interfaces;
using ComputergrafikSpiel.Model.World;
using OpenTK;
using OpenTK.Graphics;

namespace ComputergrafikSpiel.Model.Triggers
{
    internal class Trigger : ITrigger
    {
        private ColliderLayer.Layer activators;
        private bool setAsPassive;
        private World.WorldEnum.Type type;

        public Trigger(Vector2 position, ColliderLayer.Layer activators, bool passive, World.WorldEnum.Type type)
        {
            this.activators = activators;
            this.Position = position;
            this.Texture = new TextureLoader().LoadTexture("Door/Door");
            this.Scale = new Vector2(16, 32);
            this.type = type;
            if (!passive)
            {
                this.Collider = new RectangleOffsetCollider(this, new Vector2(-3, 0), 16, ColliderLayer.Layer.Trigger, this.activators);
            }
            else
            {
                this.Collider = new CircleOffsetCollider(this, Vector2.Zero, 16, ColliderLayer.Layer.Trigger, this.activators);
                this.Scale = this.Scale * new Vector2(-1, 1);
            }

            this.setAsPassive = passive;
        }

        public ICollider Collider { get; private set; }

        public Vector2 Position { get; }

        public float Rotation { get; } = 0f;

        public Vector2 RotationAnker { get; } = Vector2.Zero;

        // may have to be changed with the collider radius
        public Vector2 Scale { get; set; } = Vector2.One * 16;

        public ITexture Texture { get; set;  }

        public IEnumerable<(Color4 color, Vector2[] vertices)> DebugData => null;

        public void TriggerCollisionFunction()
        {
            List<INonPlayerCharacter> enemyCount = Scene.Scene.Current.NPCs.ToList();
            if (!this.setAsPassive && enemyCount.Count == 0)
            {
                Scene.Scene.Current.OnChangeScene();
                Scene.Scene.Player.ChangePosition();
            }
        }

        public void Update(float dtime)
        {
            if (!this.setAsPassive && Scene.Scene.Current.NpcList.Count == 0)
            {
                switch (this.type)
                {
                    case WorldEnum.Type.Water: this.Texture = new TextureLoader().LoadTexture("Door/DoorWaterOpen");
                        break;
                    case WorldEnum.Type.Earth: this.Texture = new TextureLoader().LoadTexture("Door/DoorEarthOpen");
                        break;
                    case WorldEnum.Type.Fire: this.Texture = new TextureLoader().LoadTexture("Door/DoorFireOpen");
                        break;
                    case WorldEnum.Type.Air: this.Texture = new TextureLoader().LoadTexture("Door/DoorAirOpen");
                        break;
                    case WorldEnum.Type.Safezone: this.Texture = new TextureLoader().LoadTexture("Door/DoorSafezoneOpen");
                        break;
                    default: break;
                }
            }
        }
    }
}
