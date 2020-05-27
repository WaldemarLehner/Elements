using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.View.Renderer.Interfaces;
using ComputergrafikSpiel.View.Exceptions;
using ComputergrafikSpiel.View.Renderer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using System;
using System.Collections.Generic;
using ComputergrafikSpiel.View;
using ComputergrafikSpiel.View.Interfaces;

namespace ComputergrafikSpiel.Test.View
{
    [TestClass]
    public class OpenTKRendererTest
    {
        [TestMethod]
        public void AssertThatCreatingInstanceWithNullIRenderableThrowsArgumentNullException()
        {
            List<MockRenderable> renderables = null;
            ICamera camera = new Camera(100, 0, 0, 100);
            Assert.ThrowsException<ArgumentNullException>(() => new OpenTKRenderer(renderables,camera));
        }

        [TestMethod]
        public void AssertThatCreatingInstanceWithEmptyListDoesNotThrowException()
        {
            List<MockRenderable> renderables = new List<MockRenderable>();
            ICamera camera = new Camera(100, 0, 0, 100);
            new OpenTKRenderer(renderables,camera);
        }

        [DataTestMethod()]
        [DataRow(-1,1920)]
        [DataRow(0, 1920)]
        [DataRow(2, 0)]
        [DataRow(40,-1)]
        public void AssertThatInvalidScreenDimensionsThrowArgumentNotPositiveIntegerGreaterZeroException(int width, int height)
        {
            List<MockRenderable> renderables = new List<MockRenderable>();
            ICamera camera = new Camera(100, 0, 0, 100);
            IRenderer renderer = new OpenTKRenderer(renderables,camera);
            Assert.ThrowsException<ArgumentNotPositiveIntegerGreaterZeroException>(() => renderer.Resize(width, height));
        }

        [DataTestMethod, Ignore("Thanks to this Function calling the GL.Viewport Method, it has to be disabled for now. GL.<...> Methods require an Active OpenGL Context, such as a GameWindow. Tests using a Running GameWindow have been done, to no success.")]
        [DataRow(30,20)]
        [DataRow(100,300)]
        [DataRow(1920, 1080)]
        public void AssertThatResizeUpdatesScreenDimensions(int width, int height)
        {
            List<MockRenderable> renderables = new List<MockRenderable>();
            ICamera camera = new Camera(100, 0, 0, 100);
            OpenTKRenderer renderer = new OpenTKRenderer(renderables,camera);
            renderer.Resize(width, height);
            Assert.AreEqual(width, renderer.Screen.Item1);
            Assert.AreEqual(height, renderer.Screen.Item2);
        }

        private class MockRenderable : IRenderable
        {
            public Vector2 Position { get; set; } = Vector2.Zero;

            public Vector2 Scale { get; set; } = Vector2.One;

            public float Rotation { get; set; } = 0f;

            public Vector2 RotationAnker { get; set; } = Vector2.Zero;

            public ITexture Texture { get; set; }

            ITexture IRenderable.Texture => throw new NotImplementedException();
        }
    }
}
