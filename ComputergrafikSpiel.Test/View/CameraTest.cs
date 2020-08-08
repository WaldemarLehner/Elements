using ComputergrafikSpiel.View;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using System;

namespace ComputergrafikSpiel.Test.View
{
    [TestClass]
    public class CameraTest
    {
        [TestMethod]
        public void AssertThatWrongInputParametersThrowException()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Camera(0, 0, 0, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Camera(0, 1, 1, 0));
        }

        [TestMethod]
        public void AssertThatCameraDetectsPointsInsideBounds()
        {
            var cam = new Camera(100, 0, 0, 100);

            Assert.IsTrue(cam.CanPointBeSeenByCamera(new Vector2()));
            Assert.IsFalse(cam.CanPointBeSeenByCamera(new Vector2(-1, 0)));
        }


    }
}
