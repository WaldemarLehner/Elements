using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
using ComputergrafikSpiel.View.Interfaces;
using ComputergrafikSpiel.View.Renderer.Interfaces;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace ComputergrafikSpiel.View.Renderer
{
    internal class OpenTKRenderer : IRenderer
    {
        private Shader.Shader textureShader;

        internal OpenTKRenderer(IModel model, ICamera camera)
        {
            _ = model ?? throw new ArgumentNullException(nameof(model));
            _ = model.Renderables ?? throw new ArgumentNullException(nameof(model.Renderables));
            _ = camera ?? throw new ArgumentNullException(nameof(camera));

            this.RenderablesCollection = model.Renderables;
            this.Camera = camera;
            this.TextureData = new Dictionary<string, TextureData>();
        }

        public bool Active { get; private set; } = true;

        public ICamera Camera { get; private set; }

        public (int width, int height) Screen { get; private set; }

        private IReadOnlyCollection<IRenderable> RenderablesCollection { get; }

        private Dictionary<string, TextureData> TextureData { get; set; }

        public void Render()
        {
            if (this.textureShader == null)
            {
                // this.textureShader = new Shader.Shader("basic.vert", "basic.frag");
            }

            if (!this.Active)
            {
                return;
            }

            // this.textureShader.Use();

            // Clear the Screen
            GL.ClearColor(new Color4(150, 150, 150, 255));
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Render each IRenderable, in their order from 1st to last.
            foreach (var entry in this.RenderablesCollection)
            {
                this.RenderRenderable(entry);
            }
        }

        public void Resize(int screenWidth, int screenHeight)
        {
            if (screenHeight < 0)
            {
                throw new Exceptions.ArgumentNotPositiveIntegerGreaterZeroException(nameof(screenHeight));
            }

            if (screenWidth < 0)
            {
                throw new Exceptions.ArgumentNotPositiveIntegerGreaterZeroException(nameof(screenWidth));
            }

            if (screenHeight == 0 || screenWidth == 0)
            {
                this.Active = false;
                return;
            }

            this.Active = true;

            this.Screen = (screenWidth, screenHeight);
            GL.Viewport(0, 0, screenWidth, screenHeight);
        }

        private void RenderRenderable(IRenderable renderable)
        {
            // Make Rectangle out of Renderable
            var renderableRectangle = new Rectangle(renderable, true);

            // Check if Texture Data is already stored, if not, add Texture
            if (!this.TextureData.ContainsKey(renderable.Texture.FilePath))
            {
                this.TextureData[renderable.Texture.FilePath] = new TextureData(renderable.Texture);
            }

            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            // Get the bounds of the Renderable and check if it can be skipped
            if (!this.IsDrawNeeded(renderableRectangle))
            {
                return;
            }

            var texCoords = renderable.Texture.TextureCoordinates;
            this.TextureData[renderable.Texture.FilePath].Enable();
            this.Camera.DrawRectangle(renderableRectangle, texCoords, this.Screen);
            this.TextureData[renderable.Texture.FilePath].Disable();
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Blend);
        }

        private bool IsDrawNeeded(Rectangle rect)
        {
            List<Vector2> points = new List<Vector2>
            {
                rect.TopLeft,
                rect.TopRight,
                rect.BottomLeft,
                rect.BottomRight,
            };

            foreach (var point in points)
            {
                if (this.Camera.CanPointBeSeenByCamera(point))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
