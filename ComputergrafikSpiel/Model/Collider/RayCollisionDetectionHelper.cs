using System;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.Collider
{
    public static class RayCollisionDetectionHelper
    {
        internal static bool DidRayCollide(IRay ray, ICollider collider, bool considerLayers = true)
        {
            if (!ColliderLayer.CanCollide(ray.Layer, collider.OwnLayer) && considerLayers)
            {
                return false;
            }

            if (collider is RectangleOffsetCollider)
            {
                return RayCollisionDetectionHelper.DidRayColliderRectangleCollider(ray, collider as RectangleOffsetCollider);
            }

            if (collider is CircleOffsetCollider)
            {
                return RayCollisionDetectionHelper.DidRayCollideCircleCollider(ray, collider as CircleOffsetCollider);
            }

            throw new NotImplementedException();
        }

        internal static bool DidRayCollideCircleCollider(IRay ray, CircleOffsetCollider collider)
        {
            return DidRayCollideCircleCollider(ray, collider.Position, collider.Radius);
        }

        internal static bool DidRayCollideCircleCollider(IRay ray, Vector2 colliderPosition, float radius)
        {
            Vector2 rayPositionCirclePositionDifference = ray.Position - colliderPosition;
            Vector2 rayDirection = ray.Direction.Normalized();

            var constantScalar = (rayPositionCirclePositionDifference * rayDirection).Scalar();
            var variableScalar = (rayDirection * rayDirection).Scalar();

            var rResult = -constantScalar / variableScalar;

            if (rResult < 0)
            {
                rResult = 0;
            }
            else if (rResult > ray.MaxDistance)
            {
                rResult = ray.MaxDistance;
            }

            Vector2 closestPosition = ray.Position + (rayDirection * rResult);

            if (Vector2.Distance(closestPosition, colliderPosition) <= radius)
            {
                return true;
            }

            return false;
        }

        internal static bool DidRayColliderRectangleCollider(IRay ray, RectangleOffsetCollider collider)
        {
            var (top, bottom, left, right) = collider.Bounds;

            var leftY = ray.EvalX(left);
            var rightY = ray.EvalX(right);
            var topX = ray.EvalY(top);
            var bottomX = ray.EvalY(bottom);

            if (leftY > top && rightY > top)
            {
                return false;
            }

            if (leftY < bottom && rightY < bottom)
            {
                return false;
            }

            if (topX < left && bottomX < left)
            {
                return false;
            }

            if (topX > right && bottomX > right)
            {
                return false;
            }

            // Calculate Rx value to see if the found collider is in negative ray direction.
            var isXpositive = ray.Direction.X >= 0;
            if (isXpositive && (right < ray.Position.X))
            {
                return false;
            }

            if (!isXpositive && (left > ray.Position.X))
            {
                return false;
            }

            return true;
        }

        private static float Scalar(this Vector2 vec)
        {
            return vec.X + vec.Y;
        }

        private static float EvalX(this IRay ray, float x)
        {
            return ((ray.Direction.Y / ray.Direction.X) * (x - ray.Position.X)) + ray.Position.Y;
        }

        private static float EvalY(this IRay ray, float y)
        {
            return ((ray.Direction.X / ray.Direction.Y) * (y - ray.Position.Y)) + ray.Position.X;
        }
    }
}
