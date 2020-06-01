using ComputergrafikSpiel.View;
using ComputergrafikSpiel.View.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputergrafikSpiel.Test.View
{
    [TestClass]
    public class CameraCoordinateConversionHelperTest
    {
        [TestMethod]
        public void WorldToScreenCenter()
        {
            var cam = new Camera(10, 0, 0, 10);
            var center = new Vector2(5, 5);

            var result = CameraCoordinateConversionHelper.WorldToScreen(cam, center);
            Assert.AreEqual(new Vector2(0.5f,0.5f), result);
        }

        [TestMethod]
        public void WorldToScreenBottomLeft()
        {
            var cam = new Camera(10, 0, 0, 10);
            var bottomLeft = new Vector2(0, 0);

            var result = CameraCoordinateConversionHelper.WorldToScreen(cam, bottomLeft);
            Assert.AreEqual(Vector2.Zero, result);
        }
    }
}
