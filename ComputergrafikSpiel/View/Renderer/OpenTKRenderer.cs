using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.View.Helpers;
using ComputergrafikSpiel.View.Interfaces;
using ComputergrafikSpiel.View.Renderer.Interfaces;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ComputergrafikSpiel.View.Renderer
{
    internal class OpenTKRenderer : IRenderer
    {
        internal OpenTKRenderer(IReadOnlyCollection<IRenderable> renderables, ICamera camera)
        {
            _ = renderables ?? throw new ArgumentNullException(nameof(renderables));
            _ = camera ?? throw new ArgumentNullException(nameof(camera));

            this.RenderablesCollection = renderables;
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
            if (!this.Active)
            {
                return;
            }

            // Clear the Screen
            GL.Clear(ClearBufferMask.ColorBufferBit);

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

            // Get the bounds of the Renderable and check if it can be skipped
            if (!this.IsDrawNeeded(renderableRectangle))
            {
                return;
            }

            var texCoords = renderable.Texture.TextureCoordinates;
            this.TextureData[renderable.Texture.FilePath].Enable();
            this.Camera.DrawRectangle(renderableRectangle, texCoords, this.Screen);
            this.TextureData[renderable.Texture.FilePath].Disable();
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
