using System;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.Collider
{
    internal class Ray : IRay
    {
        internal Ray(Vector2 position, Vector2 direction, float maxDis, ColliderLayer.Layer layer)
        {
            this.Position = position;
            this.Layer = layer;
            if (direction.X == 0 && direction.Y == 0)
            {
                throw new ArgumentOutOfRangeException("Direction can't be 0,0");
            }

            this.Direction = direction.Normalized();

            if (maxDis <= 0)
            {
                throw new ArgumentOutOfRangeException("Maximum Distance has to be above 0");
            }

            this.MaxDistance = maxDis;
        }

        public Vector2 Position { get; }

        public Vector2 Direction { get; }

        public float MaxDistance { get; }

        public ColliderLayer.Layer Layer { get; }

        public bool DidCollideWith(ICollider collider) => RayCollisionDetectionHelper.DidRayCollide(this, collider);

        public float MinimalDistanceTo(Vector2 tileCenter)
        {
            float distance;

            // use the Lotfußpunktverfahren in order to calculate min distance between the tile's center and the Ray
            float intermediateResultNotContainingR = ((this.Position.X - tileCenter.X) * this.Direction.X) + ((this.Position.Y - tileCenter.Y) * this.Direction.Y);
            float intermediateResultContainingR = ((float)Math.Pow(this.Direction.X, 2) + (float)Math.Pow(this.Direction.Y, 2)) * -1;

            float r = intermediateResultNotContainingR / intermediateResultContainingR;

            if (r <= 0)
            {
                distance = Vector2.Distance(this.Position, tileCenter);

                return distance;
            }

            Vector2 lotfusspoint = Vector2.Add(this.Position, Vector2.Multiply(this.Direction, r));

            // if the lotfuspoint is further away than the "point" of maxDistance, then use distance between max distance "point" and tileCenter instead
            if (Vector2.Distance(lotfusspoint, this.Position) > this.MaxDistance)
            {
                Vector2 maxDistPoint = Vector2.Add(this.Position, this.Direction);
                distance = Vector2.Distance(maxDistPoint, tileCenter);

                return distance;
            }

            distance = Vector2.Distance(lotfusspoint, tileCenter);

            return distance;
        }
    }
}