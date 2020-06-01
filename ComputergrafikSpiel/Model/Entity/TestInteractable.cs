﻿using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.Entity
{
    internal class TestInteractable : IEntity
    {

        public TestInteractable()
        {
            this.Position = new Vector2(500, 500);
            this.Collider = new CircleOffsetCollider(this, Vector2.Zero, 10);
        }

        public ICollider Collider { get; set; }

        public ITexture Texture { get; } = null;

        public Vector2 Position { get; set; } = Vector2.Zero;

        public float Rotation { get; } = 0f;

        public Vector2 RotationAnker { get; } = Vector2.Zero;

        public Vector2 Scale { get; } = Vector2.One * 20;

        public void Update(float dtime)
        {
        }
    }
}
