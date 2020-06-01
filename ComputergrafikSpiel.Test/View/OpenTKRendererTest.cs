using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.View.Renderer.Interfaces;
using ComputergrafikSpiel.View.Exceptions;
using ComputergrafikSpiel.View.Renderer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using System;
using ComputergrafikSpiel.View;
using ComputergrafikSpiel.View.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
using ComputergrafikSpiel.Test.Model.TestHelper;

namespace ComputergrafikSpiel.Test.View
{
    [TestClass]
    public class OpenTKRendererTest
    {
        [TestMethod]
        public void AssertThatCreatingInstanceWithNullIRenderableThrowsArgumentNullException()
        {
            MockModel m = new MockModel();
            m.RenderableList = null;
            ICamera camera = new Camera(100, 0, 0, 100);
            Assert.ThrowsException<ArgumentNullException>(() => new OpenTKRenderer(m,camera));
        }

        [TestMethod]
        public void AssertThatCreatingInstanceWithEmptyListDoesNotThrowException()
        {
            
            IModel model = new MockModel();
       
            ICamera camera = new Camera(100, 0, 0, 100);
            new OpenTKRenderer(model,camera);
        }

        [DataTestMethod()]
        [DataRow(-1,1920)]
        //[DataRow(0, 1920)]  -> Zero no longer throws exception, but deactivates the renderer. This is to prevent from Crashes when minimized
        //[DataRow(2, 0)]
        [DataRow(40,-1)]
        public void AssertThatInvalidScreenDimensionsThrowArgumentNotPositiveIntegerGreaterZeroException(int width, int height)
        {
            IModel m = new MockModel();
            ICamera camera = new Camera(100, 0, 0, 100);
            IRenderer renderer = new OpenTKRenderer(m,camera);
            Assert.ThrowsException<ArgumentNotPositiveIntegerGreaterZeroException>(() => renderer.Resize(width, height));
        }

        [DataTestMethod, Ignore("Thanks to this Function calling the GL.Viewport Method, it has to be disabled for now. GL.<...> Methods require an Active OpenGL Context, such as a GameWindow. Tests using a Running GameWindow have been done, to no success.")]
        [DataRow(30,20)]
        [DataRow(100,300)]
        [DataRow(1920, 1080)]
        public void AssertThatResizeUpdatesScreenDimensions(int width, int height)
        {
            IModel m = new MockModel();
            ICamera camera = new Camera(100, 0, 0, 100);
            OpenTKRenderer renderer = new OpenTKRenderer(m,camera);
            renderer.Resize(width, height);
            Assert.AreEqual(width, renderer.Screen.width);
            Assert.AreEqual(height, renderer.Screen.height);
        }

        internal class MockRenderable : IRenderable
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
