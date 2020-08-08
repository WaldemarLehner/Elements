using ComputergrafikSpiel.Model.Collider;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;

namespace ComputergrafikSpiel.Test.Model.Collider
{
    [TestClass]
    public class RayTest
    {
        static ColliderLayer.Layer all = (ColliderLayer.Layer)~0;

        [TestMethod]
        public void AssertThatIllegalDirectionThrowsArgumentOutOfRangeException()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Ray(Vector2.Zero, Vector2.Zero, 1, all));
        }
        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void AssertThatIllegalMaximumDistanceThrowsArgumentOutOfRangeException(float maxDis)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Ray(Vector2.Zero, Vector2.One, maxDis, all));
        }
        [DataTestMethod]
        [DataRow(4, 2, 0)]
        [DataRow(4, 3, 1)]
        [DataRow(1, 2, 1)]
        public void AssertThatMinimalDistanceToIsCalculatedCorrectly(float tileLocX, float tileLocY, float trueDistance)
        {
            Vector2 rayTestPosition = new Vector2(2, 2);
            Vector2 rayTestDirection = new Vector2(1, 0);
            float maxDis = 50;

            Vector2 tileTestCenter = new Vector2(tileLocX, tileLocY);
            Ray testRay = new Ray(rayTestPosition, rayTestDirection, maxDis, all);

            float calculatedDistance = testRay.MinimalDistanceTo(tileTestCenter);

            Assert.AreEqual(trueDistance, calculatedDistance);
        }
    }
}
