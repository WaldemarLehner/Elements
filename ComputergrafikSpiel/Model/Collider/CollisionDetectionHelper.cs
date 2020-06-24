using System.Linq;
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
            if (!ColliderLayer.CanCollide(collider1.OwnLayer, collider2.CollidesWith))
            {
                return false;
            }

            if (collider1 is CircleOffsetCollider && collider2 is CircleOffsetCollider)
            {
                return ColliderLayer.CanCollide(collider1.CollidesWith, collider2.CollidesWith) && MinDistanceBetween(collider1 as CircleOffsetCollider, collider2 as CircleOffsetCollider) <= 0;
            }

            if ((collider1 is RectangleOffsetCollider && collider2 is CircleOffsetCollider) || (collider1 is CircleOffsetCollider && collider2 is RectangleOffsetCollider))
            {
                var rect = (collider1 is RectangleOffsetCollider) ? collider1 as RectangleOffsetCollider : collider2 as RectangleOffsetCollider;
                var circle = (collider1 is CircleOffsetCollider) ? collider1 as CircleOffsetCollider : collider2 as CircleOffsetCollider;
                return (from corner in rect.Corners orderby DistanceVectorCircle(corner, circle) ascending select DistanceVectorCircle(corner, circle)).First() <= 0;
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
