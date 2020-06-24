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
    internal class RockObstacle : IWorldObstacle
    {
        private readonly float scale;

        internal RockObstacle(Vector2 position, float scale)
        {
            this.Position = position;
            this.scale = scale;
            this.Collider = new CircleOffsetCollider(this, Vector2.Zero, scale / 2f, ColliderLayer.Layer.Wall, ColliderLayer.Layer.Bullet | ColliderLayer.Layer.Enemy | ColliderLayer.Layer.Player);
            this.Texture = new TextureLoader().LoadTexture("Obstacle/Stone");
        }

        public ICollider Collider { get; }

        public ITexture Texture { get; }

        public IEnumerable<(Color4 color, Vector2[] vertices)> DebugData => null;

        public Vector2 Position { get; }

        public float Rotation => 0f;

        public Vector2 RotationAnker => this.Position;

        public Vector2 Scale => Vector2.One * this.scale;

        public void Update(float dtime)
        {
        }
    }
}
