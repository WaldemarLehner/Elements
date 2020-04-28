using System;
using OpenTK;

namespace ComputergrafikSpiel.View.Helpers
{
    internal static class RotationHelper
    {
        /// <summary>
        /// Returns a rotated Vector2 with a given Pivot and Rotation.
        /// </summary>
        /// <param name="vectorToRotate">The <see cref="Vector2"/> to rotate.</param>
        /// <param name="pivot"> The <see cref="Vector2"/> that is used as the rotational pivot.</param>
        /// <param name="rotationInRadians">The rotation in radians (theta) .</param>
        /// <returns>A rotated Vector2.</returns>
        internal static Vector2 RotateWithPivot(this Vector2 vectorToRotate, Vector2 pivot, float rotationInRadians)
        {
            // Move the vectorToRotate and the pivot in a way so that the pivot become the centre of the coordinate system.
            vectorToRotate -= pivot;

            // Now, the vector can be rotated around the origin.
            float newX = ((float)Math.Cos(rotationInRadians) * vectorToRotate.X) - ((float)Math.Sin(rotationInRadians) * vectorToRotate.Y);
            float newY = ((float)Math.Sin(rotationInRadians) * vectorToRotate.X) + ((float)Math.Cos(rotationInRadians) * vectorToRotate.Y);
            Vector2 newVector = new Vector2(newX, newY);

            // Move the pivot back to the original position
            newVector += pivot;
            return newVector;
        }

        /// <summary>
        /// Rotate a vector around the Origin.
        /// </summary>
        /// <param name="vectorToRotate">The <see cref="Vector2"/> to rotate.</param>
        /// <param name="rotationInRadians">The angle in radians.</param>
        /// <returns>A new Vector rotated around 0,0.</returns>
        internal static Vector2 Rotate(this Vector2 vectorToRotate, float rotationInRadians) => vectorToRotate.RotateWithPivot(Vector2.Zero, rotationInRadians);
    }
}
