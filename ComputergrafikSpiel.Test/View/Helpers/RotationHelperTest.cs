using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using System.Diagnostics.CodeAnalysis;
using ComputergrafikSpiel.View.Helpers;
using System;

namespace ComputergrafikSpiel.Test.View.Helpers
{
    [TestClass, ExcludeFromCodeCoverage]
    public class RotationHelperTest
    {
        [DataTestMethod]
        [DataRow(1, 0, 0, 1)]
        [DataRow(0, 1, -1, 0)]
        [DataRow(-1, 0, 0, -1)]
        [DataRow(0, -1, 1, 0)]
        public void Rotate90degAroundOrigin(float vecX, float vecY, float vecXresult, float vecYresult)
        {
            var vec2 = new Vector2(vecX, vecY);
            var angle = MathHelper.DegreesToRadians(90);
            var vec2_rotated = vec2.Rotate(angle);
            Assert.AreEqual(vecXresult, Math.Round(vec2_rotated.X, 5));
            Assert.AreEqual(vecYresult, Math.Round(vec2_rotated.Y, 5));
        }

        [DataTestMethod]
        [DataRow(1, 0, 1, 1, 2, 1)]
        public void Rotate90degAroundPivot(float vecX, float vecY, float pivotX, float pivotY, float vecXresult, float vecYresult)
        {
            var vec2 = new Vector2(vecX, vecY);
            var pivot = new Vector2(pivotX, pivotY);
            var angle = MathHelper.DegreesToRadians(90);
            var vec2_rotated = vec2.RotateWithPivot(pivot, angle);

            Assert.AreEqual(vecXresult, Math.Round(vec2_rotated.X, 5));
            Assert.AreEqual(vecYresult, Math.Round(vec2_rotated.Y, 5));
        }
    }
}
