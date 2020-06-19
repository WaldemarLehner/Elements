﻿using System;
using System.Collections.Generic;
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
        public Trigger(Vector2 position, ColliderLayer.Layer activators)
        {
            // radius may have to be changed
            this.Collider = new CircleOffsetCollider(this, Vector2.Zero, 16, activators);
            Scene.Scene.Current.ColliderManager.AddTriggerCollidable((int)position.X, (int)position.Y, this);
            this.Position = position;
            this.Texture = new TextureLoader().LoadTexture("Door/TreeBranchesDoor");
            this.Scale = new Vector2(32, 32);
        }

        public ICollider Collider { get; }

        public Vector2 Position { get; }

        public float Rotation { get; } = 0f;

        public Vector2 RotationAnker { get; } = Vector2.Zero;

        // may have to be changed with the collider radius
        public Vector2 Scale { get; } = Vector2.One * 16;

        public ITexture Texture { get; }

        public IEnumerable<(Color4 color, Vector2[] vertices)> DebugData => null;

        public void TriggerCollisionFunction()
        {
            // will be changed once scenes are implemented
            Console.WriteLine("Door Activate");
        }

        public void Update(float dtime)
        {
        }
    }
}