using System.Collections.Generic;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using ComputergrafikSpiel.Model.World.Interfaces;
using OpenTK;
using OpenTK.Graphics;

namespace ComputergrafikSpiel.Model.World.Obstacles
{
    public class StumpObstacle : IWorldObstacle
    {
        private readonly float scale;

        internal StumpObstacle(Vector2 position, float scale, WorldEnum.Type type)
        {
            this.Position = position;
            this.scale = scale;
            this.Collider = new CircleOffsetCollider(this, Vector2.Zero, scale / 2f, ColliderLayer.Layer.Wall, ColliderLayer.Layer.Bullet | ColliderLayer.Layer.Enemy | ColliderLayer.Layer.Player);
            switch (type)
            {
                case WorldEnum.Type.Water: this.Texture = new TextureLoader().LoadTexture("Obstacle/TreeStump");
                    break;
                case WorldEnum.Type.Earth: this.Texture = new TextureLoader().LoadTexture("Obstacle/StoneGreen");
                    break;
                case WorldEnum.Type.Fire: this.Texture = new TextureLoader().LoadTexture("Obstacle/Fire");
                    break;
                case WorldEnum.Type.Air: this.Texture = new TextureLoader().LoadTexture("Obstacle/Vase");
                    break;
                default: break;
            }
        }

        public ICollider Collider { get; }

        public ITexture Texture { get; }

        public IEnumerable<(Color4 color, Vector2[] vertices)> DebugData => new (Color4 color, Vector2[] vertices)[] { this.Collider.DebugData };

        public Vector2 Position { get; }

        public float Rotation => 0f;

        public Vector2 RotationAnker => this.Position;

        public Vector2 Scale { get => Vector2.One * this.scale; set => _ = value; }

        public void Update(float dtime)
        {
        }
    }
}
