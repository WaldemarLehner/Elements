using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using System.Diagnostics.CodeAnalysis;
using ComputergrafikSpiel.View.Helpers;
using System;

namespace ComputergrafikSpiel.Test.View.Helpers
{
    [TestClass, ExcludeFromCodeCoverage]
    public class IRenderableHelperTest
    {
        [TestMethod]
        public void AssertThatNullRenderableWillThrowArgumentNullException()
        {
            MockRenderable renderable = null;
            Assert.ThrowsException<ArgumentNullException>(() => renderable.RenderRectangleDebug(20, 20));
        }

        [DataTestMethod]
        [DataRow(0,0)]
        [DataRow(0,20)]
        [DataRow(20,0)]
        [DataRow(-1, 3)]
        [DataRow(20,-4)]
        public void AssertThatInvalidWidthAndHeightWillThrowArgumentNotPositiveIntegerGreaterZeroException(int width, int height)
        {
            MockRenderable renderable = new MockRenderable();
            Assert.ThrowsException<ComputergrafikSpiel.View.Exceptions.ArgumentNotPositiveIntegerGreaterZeroException>(() => renderable.RenderRectangleDebug(width, height));
        }
        [TestMethod]
        public void AssertThatValidInputsWillNotThrowException()
        {
            //Assert that does not throw
            MockRenderable renderable = new MockRenderable { Position = new Vector2(50, 50) };
            int width = 100, height = 100;
            renderable.RenderRectangleDebug(width, height,callGLFunctions:false);
        }

        private class MockRenderable : IRenderable
        {
            public Vector2 Position { get; set; } = Vector2.Zero;

            public Vector2 Scale { get; set; } = Vector2.One;

            public float Rotation { get; set; } = 0f;

            public Vector2 RotationAnker { get; set; } = Vector2.Zero;

            public ITexture Texture { get; set; }
        }
    }
}
