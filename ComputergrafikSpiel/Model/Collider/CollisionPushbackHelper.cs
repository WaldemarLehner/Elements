using System;
using System.Collections.Generic;
using System.Linq;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using OpenTK;
using OpenTK.Graphics;

namespace ComputergrafikSpiel.Model.Collider
{
    /// <summary>
    /// This helper class is designed to calculate the "pushing back" of the collider. This will make it so that one collider can slide along another one.
    /// !!! EXPECTS THAT A COLLISION DID OCCUR !!!.
    /// </summary>
    internal static class CollisionPushbackHelper
    {
        internal static Vector2 PushbackCollider(ICollidable colliderToPush, ICollidable staticCollider)
        {
            if (colliderToPush.Collider is CircleOffsetCollider && staticCollider.Collider is CircleOffsetCollider)
            {
                return CollisionPushbackHelper.PushbackCollider(colliderToPush.Collider as CircleOffsetCollider, staticCollider.Collider as CircleOffsetCollider);
            }

            if (colliderToPush.Collider is CircleOffsetCollider && staticCollider.Collider is RectangleOffsetCollider)
            {
                return CollisionPushbackHelper.PushbackCollider(colliderToPush.Collider as CircleOffsetCollider, staticCollider.Collider as RectangleOffsetCollider);
            }

            throw new NotImplementedException("only Circle -> Circle and Circle -> Rectangle is implemented");
        }

        private static Vector2 PushbackCollider(CircleOffsetCollider @dynamic, RectangleOffsetCollider @static)
        {
            var relevantEdges = GetNearestEdges(@static.Position, @static.Bounds, dynamic.Position);

            Vector2 closestPositionToCircle;

            // Check corners.
            var corners = @static.Corners.Where(e => Vector2.Distance(e, dynamic.Position) < dynamic.MaximumDistanceFromPosition).ToList();
            if (corners.Count > 0)
            {
                closestPositionToCircle = corners.First();
            }
            else
            {
                var edgePositions = new List<Vector2>(2);
                foreach (var (start, directionAndLength) in relevantEdges)
                {
                    edgePositions.Add(GetPerpendicularFootDistancePosition((start, directionAndLength), dynamic.Position));
                }

                closestPositionToCircle = (from e in edgePositions orderby Vector2.Distance(dynamic.Position, e) ascending select e).First();
            }

            var distanceCircleEdge = Vector2.Distance(closestPositionToCircle, dynamic.Position);
            if (distanceCircleEdge >= @dynamic.MaximumDistanceFromPosition)
            {
                Scene.Scene.Current.IndependentDebugData.Add((Color4.Blue, new Vector2[] { closestPositionToCircle, @dynamic.Position }));

                return @dynamic.Position;
            }

            var newPosition = closestPositionToCircle + ((@dynamic.Position - closestPositionToCircle).Normalized() * (@dynamic.MaximumDistanceFromPosition + .5f));
            Scene.Scene.Current.IndependentDebugData.Add((Color4.Crimson, new Vector2[] { closestPositionToCircle, newPosition }));

            return newPosition;
        }

        private static Vector2 PushbackCollider(CircleOffsetCollider @dynamic, CircleOffsetCollider @static)
        {
            var direction = (@dynamic.Position - @static.Position).Normalized();
            return @static.Position + (direction * (@dynamic.MaximumDistanceFromPosition + @static.MaximumDistanceFromPosition));
        }

        private static IEnumerable<(Vector2 start, Vector2 directionAndLength)> GetNearestEdges(Vector2 rectanglePosition, (float top, float bottom, float left, float right) bounds, Vector2 circlePosition)
        {
            List<(Vector2 start, Vector2 directionAndLength)> edges = new List<(Vector2 start, Vector2 directionAndLength)>();

            // Vertical Edge.
            if (rectanglePosition.X > circlePosition.X)
            {
                var start = new Vector2(bounds.left, bounds.bottom);
                var dirAndLen = new Vector2(0, bounds.top - bounds.bottom);
                edges.Add((start, dirAndLen));
            }
            else if (rectanglePosition.X < circlePosition.X)
            {
                var start = new Vector2(bounds.right, bounds.bottom);
                var dirAndLen = new Vector2(0, bounds.top - bounds.bottom);
                edges.Add((start, dirAndLen));
            }

            // Horizontal Edge.
            if (rectanglePosition.Y < circlePosition.Y)
            {
                var start = new Vector2(bounds.left, bounds.top);
                var dirAndLen = new Vector2(bounds.right - bounds.left, 0);
                edges.Add((start, dirAndLen));
            }
            else if (rectanglePosition.Y > circlePosition.Y)
            {
                var start = new Vector2(bounds.left, bounds.bottom);
                var dirAndLen = new Vector2(bounds.right - bounds.left, 0);
                edges.Add((start, dirAndLen));
            }

            return edges;
        }

        private static Vector2 GetPerpendicularFootDistancePosition((Vector2 start, Vector2 directionAndLength) edge, Vector2 position)
        {
            // https://www.youtube.com/watch?v=mdtJjvsYdQg
            // g: (s + x*n - P) * n || g: Line ; s: start; x: variable {0;1}; P: point; n: direction
            var s = edge.start;
            var n = edge.directionAndLength;
            var p = position;

            var sp = s - p;

            var scalarProductSp = sp.ScalarProduct(n);
            var scalarProductX = n.ScalarProduct(n);

            // scalarProductSp + scalarProductX = 0 → scalarProductSp = scalarProductX
            scalarProductSp *= -1;
            var x = scalarProductSp / scalarProductX;
            if (x < 0)
            {
                x = 0;
            }
            else if (x > 1)
            {
                x = 1;
            }

            Vector2 shortestPosition = edge.start + (x * edge.directionAndLength);
            return shortestPosition;
        }

        private static float ScalarProduct(this Vector2 self, Vector2 other) => (self.X * other.X) + (self.Y * other.Y);
    }
}
