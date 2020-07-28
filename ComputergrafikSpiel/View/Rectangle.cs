using System;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.View.Helpers;
using OpenTK;

namespace ComputergrafikSpiel.View
{
    public struct Rectangle
    {
        internal Rectangle(Vector2 center, float radius)
        {
            if (radius <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(radius), "Radius needs to be greater Zero");
            }

            this.TopLeft = center + new Vector2(-radius, -radius);
            this.TopRight = center + new Vector2(+radius, -radius);
            this.BottomLeft = center + new Vector2(-radius, +radius);
            this.BottomRight = center + new Vector2(+radius, +radius);
        }

        internal Rectangle(IRenderable renderable, bool applyRotation = false, bool overfill = false)
        {
            var scale = renderable.Scale;
            if (overfill)
            {
                scale *= 1.01f;
            }

            this.TopLeft = new Vector2(renderable.Position.X - scale.X, renderable.Position.Y + scale.Y);
            this.TopRight = new Vector2(renderable.Position.X + scale.X, renderable.Position.Y + scale.Y);
            this.BottomLeft = new Vector2(renderable.Position.X - scale.X, renderable.Position.Y - scale.Y);
            this.BottomRight = new Vector2(renderable.Position.X + scale.X, renderable.Position.Y - scale.Y);

            if (applyRotation)
            {
                var rotation = renderable.Rotation;
                var anker = renderable.RotationAnker;

                this.TopLeft = this.TopLeft.RotateWithPivot(anker, rotation);
                this.TopRight = this.TopRight.RotateWithPivot(anker, rotation);
                this.BottomLeft = this.BottomLeft.RotateWithPivot(anker, rotation);
                this.BottomRight = this.BottomRight.RotateWithPivot(anker, rotation);
            }
        }

        internal Rectangle(Vector2 topLeft, float width, float height)
        {
            if (width <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(width), "Width needs to be greater Zero");
            }

            if (height <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(height), "Height needs to be greater Zero");
            }

            this.TopLeft = topLeft;
            this.TopRight = topLeft + new Vector2(width, 0);
            this.BottomLeft = topLeft + new Vector2(0, height);
            this.BottomRight = topLeft + new Vector2(width, height);
        }

        internal Vector2 TopLeft { get; private set; }

        internal Vector2 TopRight { get; private set; }

        internal Vector2 BottomLeft { get; private set; }

        internal Vector2 BottomRight { get; private set; }

        internal float Height => Math.Abs(this.TopLeft.Y - this.BottomLeft.Y);

        internal float Width => Math.Abs(this.TopLeft.X - this.TopRight.X);
    }
}
