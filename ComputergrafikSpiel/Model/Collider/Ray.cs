using System;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.Collider
{
    internal class Ray : IRay
    {
        private readonly Vector2 position;
        private readonly Vector2 direction;

        // is not yet used
        private readonly float maxDistance;

        internal Ray(Vector2 position, Vector2 direction, float maxDis)
        {
            this.position = position;

            if (direction.X == 0 && direction.Y == 0)
            {
                throw new ArgumentOutOfRangeException("Direction can't be 0,0");
            }

            this.direction = direction;

            if (maxDis <= 0)
            {
                throw new ArgumentOutOfRangeException("Maximum Distance has to be above 0");
            }

            this.maxDistance = maxDis;
        }

        public Vector2 Position => this.position;

        public Vector2 Direction => this.direction;

        public float MaxDistance => this.maxDistance;

        public float MinimalDistanceTo(Vector2 tileCenter)
        {
            float distance;

            // use the Lotfußpunktverfahren in order to calculate min distance between the tile's center and the Ray
            float intermediateResultNotContainingR = ((this.position.X - tileCenter.X) * this.direction.X) + ((this.position.Y - tileCenter.Y) * this.direction.Y);
            float intermediateResultContainingR = ((float)Math.Pow(this.direction.X, 2) + (float)Math.Pow(this.direction.Y, 2)) * -1;

            float r = intermediateResultNotContainingR / intermediateResultContainingR;

            if (r <= 0)
            {
                distance = Vector2.Distance(this.position, tileCenter);

                return distance;
            }

            Vector2 lotfusspoint = Vector2.Add(this.position, Vector2.Multiply(this.direction, r));

            // if the lotfuspoint is further away than the "point" of maxDistance, then use distance between max distance "point" and tileCenter instead
            if (Vector2.Distance(lotfusspoint, this.position) > this.maxDistance)
            {
                Vector2 maxDistPoint = Vector2.Add(this.position, this.direction);
                distance = Vector2.Distance(maxDistPoint ,tileCenter);

                return distance;
            }

            distance = Vector2.Distance(lotfusspoint, tileCenter);

            return distance;
        }
    }
}