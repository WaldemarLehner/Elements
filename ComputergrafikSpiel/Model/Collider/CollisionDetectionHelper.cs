using System.Collections.Generic;
using System.Linq;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.Entity.Particles;
using OpenTK;
using OpenTK.Graphics;

namespace ComputergrafikSpiel.Model.Collider
{
    internal static class CollisionDetectionHelper
    {
        internal static float MinDistanceBetween(ICollider collider1, ICollider collider2)
        {
            if (collider1 is CircleOffsetCollider && collider2 is CircleOffsetCollider)
            {
                return MinDistanceBetween(collider1 as CircleOffsetCollider, collider2 as CircleOffsetCollider);
            }

            if ((collider1 is RectangleOffsetCollider && collider2 is CircleOffsetCollider) || (collider1 is CircleOffsetCollider && collider2 is RectangleOffsetCollider))
            {
                var rect = (collider1 is RectangleOffsetCollider) ? collider1 as RectangleOffsetCollider : collider2 as RectangleOffsetCollider;
                var circle = (collider1 is CircleOffsetCollider) ? collider1 as CircleOffsetCollider : collider2 as CircleOffsetCollider;
                return (from corner in rect.Corners orderby DistanceVectorCircle(corner, circle) ascending select DistanceVectorCircle(corner, circle)).First();
            }

            // FallBack Generic
            return Vector2.Distance(collider1.Position, collider2.Position) - (collider1.MaximumDistanceFromPosition + collider2.MaximumDistanceFromPosition);
        }

        internal static float MinDistanceBetween(CircleOffsetCollider collider1, CircleOffsetCollider collider2) => Vector2.Distance(collider1.Position, collider2.Position) - (collider1.MaximumDistanceFromPosition + collider2.MaximumDistanceFromPosition);

        internal static bool DidCollideWith(ICollider collider1, ICollider collider2)
        {
            if (!ColliderLayer.CanCollide(collider1.CollidesWith, collider2.OwnLayer))
            {
                return false;
            }

            if (collider1 is CircleOffsetCollider && collider2 is CircleOffsetCollider)
            {
                return MinDistanceBetween(collider1 as CircleOffsetCollider, collider2 as CircleOffsetCollider) <= 0;
            }

            if ((collider1 is RectangleOffsetCollider && collider2 is CircleOffsetCollider) || (collider1 is CircleOffsetCollider && collider2 is RectangleOffsetCollider))
            {
                var rect = (collider1 is RectangleOffsetCollider) ? collider1 as RectangleOffsetCollider : collider2 as RectangleOffsetCollider;
                var circle = (collider1 is CircleOffsetCollider) ? collider1 as CircleOffsetCollider : collider2 as CircleOffsetCollider;

                return DidRectangleCollideWithCircle(rect, circle);
            }

            if (collider1 is RectangleOffsetCollider && collider2 is RectangleOffsetCollider)
            {
                var col1 = collider1 as RectangleOffsetCollider;
                var col2 = collider2 as RectangleOffsetCollider;

                var top = (col1.Position.Y > col2.Position.Y) ? col1 : col2;
                var bottom = (top == col1) ? col2 : col1;

                if (top.Bounds.bottom < bottom.Bounds.top)
                {
                    return true;
                }

                var left = (col1.Position.X < col2.Position.X) ? col1 : col2;
                var right = (col1 == left) ? col2 : col1;

                if (left.Bounds.right > right.Bounds.left)
                {
                    return true;
                }

                return false;
            }

            // FallBack Generic
            return Vector2.Distance(collider1.Position, collider2.Position) - (collider1.MaximumDistanceFromPosition + collider2.MaximumDistanceFromPosition) < 0;
        }

        private static bool DidRectangleCollideWithCircle(RectangleOffsetCollider rect, CircleOffsetCollider circle)
        {
            // Check bounding circle first, if no collision, continue. See blue ring: https://i.imgur.com/0bvJzGk.png
            if (Vector2.Distance(rect.Position, circle.Position) > (circle.MaximumDistanceFromPosition + rect.MaximumDistanceFromPosition))
            {
                Scene.Scene.Current.IndependentDebugData.Add((Color4.Red, new Vector2[] { rect.Position, rect.Position + ((circle.Position - rect.Position).Normalized() * rect.MaximumDistanceFromPosition) }));
                return false;
            }

            // Check inner circle
            if (Vector2.Distance(rect.Position, circle.Position) < ((rect.Bounds.right - rect.Bounds.left) * .5f) + circle.MaximumDistanceFromPosition)
            {
                Scene.Scene.Current.IndependentDebugData.Add((Color4.Beige, new Vector2[] { rect.Position, circle.Position }));
                return true;
            }

            // Check corners
            if (DoesAnyCornerIntersect(rect.Corners, circle.Position, circle.MaximumDistanceFromPosition))
            {
                return true;
            }

            // Do the more complex perpendicular foot calculation.
            var edges = GetNearestEdges(rect.Position, rect.Bounds, circle.Position);
            foreach (var edge in edges)
            {
                if (GetPerpendicularFootDistance(edge, circle.Position) < circle.MaximumDistanceFromPosition)
                {
                    return true;
                }
            }

            // No collisions have been found.
            return false;
        }

        private static float GetPerpendicularFootDistance((Vector2 start, Vector2 directionAndLength) edge, Vector2 position)
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
            var x = (scalarProductSp / scalarProductX).Clamp(0, 1);

            Vector2 shortestPosition = edge.start + (x * edge.directionAndLength);
            Scene.Scene.Current.IndependentDebugData.Add((Color4.AliceBlue, new Vector2[] { shortestPosition, position }));
            return Vector2.Distance(shortestPosition, position);
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

        private static bool DoesAnyCornerIntersect(Vector2[] corners, Vector2 position, float maximumDistanceFromPosition)
        {
            foreach (var corner in corners)
            {
                if (Vector2.Distance(corner, position) < maximumDistanceFromPosition)
                {
                    Scene.Scene.Current.IndependentDebugData.Add((Color4.BlanchedAlmond, new Vector2[] { corner, position }));
                    return true;
                }
            }

            return false;
        }

        private static float DistanceVectorCircle(Vector2 vec, CircleOffsetCollider circle) => Vector2.Distance(circle.Position, vec) - circle.MaximumDistanceFromPosition;

        private static float ScalarProduct(this Vector2 self, Vector2 other) => (self.X * other.X) + (self.Y * other.Y);
    }
}
