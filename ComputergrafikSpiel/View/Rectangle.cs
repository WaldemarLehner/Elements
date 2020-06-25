using System;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.View.Helpers;
using OpenTK;

namespace ComputergrafikSpiel.View
{
    public struct Rectangle
    {
        internal Rectangle(Vector2 topLeft, Vector2 bottomRight)
        {
            this.TopLeft = topLeft;
            this.BottomRight = bottomRight;
            this.TopRight = new Vector2(topLeft.Y, bottomRight.X);
            this.BottomLeft = new Vector2(topLeft.X, bottomRight.Y);

            if (this.TopLeft.X == this.TopRight.X)
            {
                throw new ArgumentException("The provided Rectangle has a width of 0");
            }

            if (this.TopLeft.Y == this.BottomLeft.Y)
            {
                throw new ArgumentException("The provided Rectangle has a height of 0");
            }

            // Swap the values if needed.
            if (this.TopLeft.X > this.TopRight.X)
            {
                var temp = this.TopLeft;
                this.TopLeft = this.TopRight;
                this.TopRight = temp;
            }

            if (this.BottomLeft.X > this.BottomRight.X)
            {
                var temp = this.BottomLeft;
                this.BottomLeft = this.BottomRight;
                this.BottomRight = temp;
            }

            if (this.TopLeft.Y < this.BottomLeft.Y)
            {
                var temp = this.BottomLeft;
                this.BottomLeft = this.TopLeft;
                this.TopLeft = temp;
            }

            if (this.TopRight.Y < this.BottomRight.Y)
            {
                var temp = this.BottomRight;
                this.BottomRight = this.TopRight;
                this.TopRight = temp;
            }
        }

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

        internal Rectangle(IRenderable renderable, bool applyRotation = false)
        {
            this.TopLeft = new Vector2(renderable.Position.X - renderable.Scale.X, renderable.Position.Y +renderable.Scale.Y);
            this.TopRight = new Vector2(renderable.Position.X + renderable.Scale.X, renderable.Position.Y + renderable.Scale.Y);
            this.BottomLeft = new Vector2(renderable.Position.X - renderable.Scale.X, renderable.Position.Y - renderable.Scale.Y);
            this.BottomRight = new Vector2(renderable.Position.X + renderable.Scale.X, renderable.Position.Y - renderable.Scale.Y);

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
