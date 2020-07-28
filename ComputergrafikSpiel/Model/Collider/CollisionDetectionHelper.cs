using System.Collections.Generic;
using System.Linq;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using OpenTK;

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

                // Check bounding circle first, if no collision, continue. See blue ring: https://i.imgur.com/0bvJzGk.png
                if (Vector2.Distance(rect.Position, circle.Position) > (circle.MaximumDistanceFromPosition + rect.MaximumDistanceFromPosition))
                {
                    return false;
                }

                // Check the relative position between Circle and Rectangle. 2 Borders need to be checked.
                // Convert them to Rays and calculate the shortest perpendicalar foot.
                List<IRay> edgesToCheck = new List<IRay>();
                if (circle.Position.X < rect.Position.X)
                {
                    // Circle is left of rectangle. Check left edge for collision
                    Vector2 topLeft = new Vector2(rect.Bounds.left, rect.Bounds.top);
                    Vector2 bottomLeft = new Vector2(rect.Bounds.left, rect.Bounds.bottom);
                    var ray = new Ray(bottomLeft, topLeft - bottomLeft, (topLeft - bottomLeft).Length, ~ColliderLayer.Layer.Empty); // we can pass All Layers here as the check has already been done
                    edgesToCheck.Add(ray);
                }
                else
                {
                    // Circle is right of rectangle. Check right edge for collision
                    Vector2 topRight = new Vector2(rect.Bounds.right, rect.Bounds.top);
                    Vector2 bottomRight = new Vector2(rect.Bounds.right, rect.Bounds.bottom);
                    var ray = new Ray(bottomRight, topRight - bottomRight, (topRight - bottomRight).Length, ~ColliderLayer.Layer.Empty); // we can pass All Layers here as the check has already been done
                    edgesToCheck.Add(ray);
                }

                if (circle.Position.Y < rect.Position.Y)
                {
                    // Circle is below Rectangle. Check bottom edge for collision
                    Vector2 bottomLeft = new Vector2(rect.Bounds.left, rect.Bounds.bottom);
                    Vector2 bottomRight = new Vector2(rect.Bounds.right, rect.Bounds.bottom);
                    var ray = new Ray(bottomRight, bottomLeft - bottomRight, (bottomLeft - bottomRight).Length, ~ColliderLayer.Layer.Empty); // we can pass All Layers here as the check has already been done
                    edgesToCheck.Add(ray);
                }
                else
                {
                    // Circle is below Rectangle. Check top edge for collision
                    Vector2 topLeft = new Vector2(rect.Bounds.left, rect.Bounds.top);
                    Vector2 topRight = new Vector2(rect.Bounds.right, rect.Bounds.top);
                    var ray = new Ray(topRight, topLeft - topRight, (topLeft - topRight).Length, ~ColliderLayer.Layer.Empty); // we can pass All Layers here as the check has already been done
                    edgesToCheck.Add(ray);
                }

                var anyCollisionsEdges = edgesToCheck.Any(e => RayCollisionDetectionHelper.DidRayCollideCircleCollider(e, circle));
                if (anyCollisionsEdges)
                {
                    // At least one edge has been hit. There is a collision.
                    return true;
                }

                // If the method hits this part, the circle collider is either:
                // > Inside the Rectangle.
                // > Right outside the Rectangle.

                // Check the inner circle. For reference: Red circle here: https://i.imgur.com/0bvJzGk.png .
                // If distance < 0, circle is inside the rectangle.
                var height = rect.Bounds.top - rect.Bounds.bottom;
                var width = rect.Bounds.right - rect.Bounds.left;

                var innerRectRadius = (height < width) ? height / 2f : width / 2f;
                var distanceInnerCircleCircleColliderDistance = Vector2.Distance(circle.Position, rect.Position) - (innerRectRadius + circle.MaximumDistanceFromPosition);

                return distanceInnerCircleCircleColliderDistance < 0;
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

        private static float DistanceVectorCircle(Vector2 vec, CircleOffsetCollider circle) => Vector2.Distance(circle.Position, vec) - circle.MaximumDistanceFromPosition;
    }
}
