using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.Trigger.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.Trigger
{
    internal class Trigger : ITrigger
    {
        internal Trigger(IColliderManager colliderManager, Vector2 position)
        {
            // will have to be changed
            this.Collider = new CircleOffsetCollider(this, Vector2.Zero, 32);
            this.ColliderManager = colliderManager;
            this.Position = position;
        }

        public IColliderManager ColliderManager { get; }

        public ICollider Collider { get; }

        public Vector2 Position { get; }

        public float Rotation { get; } = 0f;

        public Vector2 RotationAnker { get; } = Vector2.Zero;

        // may have to be changed alongside the radius of the Collider
        public Vector2 Scale { get; } = Vector2.One * 32;
    }
}
