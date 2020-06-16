﻿using System;
using System.Collections.Generic;
using System.Linq;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture;
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
        private readonly IModel model;

        internal OpenTKRenderer(IModel model, ICamera camera)
        {
            this.model = model ?? throw new ArgumentNullException(nameof(model));
            _ = model.Renderables ?? throw new ArgumentNullException(nameof(model.Renderables));
            this.Camera = camera ?? throw new ArgumentNullException(nameof(camera));
            this.Camera.AttachRenderer(this);
            this.TextureData = new Dictionary<string, TextureData>();
            this.Debug = true;
        }

        public bool Active { get; private set; } = true;

        public ICamera Camera { get; private set; }

        public bool Debug { get; set; } = false;

        public (int width, int height) Screen { get; private set; }

        private IEnumerable<IRenderable> RenderablesEnumerator => this.model.Renderables;

        private Dictionary<string, TextureData> TextureData { get; set; }

        public void Render()
        {
            if (!this.Active)
            {
                return;
            }

            // Clear the Screen
            GL.ClearColor(new Color4(0x13, 0x0e, 0x1c, 0xff));
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Render each IRenderable, in their order from 1st to last.
            foreach (var entry in this.RenderablesEnumerator)
            {
                if (entry is IRenderableLayeredTextures)
                {
                    this.RenderRenderableLayered(entry as IRenderableLayeredTextures);
                }
                else
                {
                    this.RenderRenderable(entry);
                }
            }

            if (this.Debug)
            {
                foreach (var entry in this.RenderablesEnumerator)
                {
                    var rand = new Random(13456);
                    OpenTKRendererHelper.RenderRenderableDebug(this, entry, rand);
                }
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

        private void RenderRenderableLayered(IRenderableLayeredTextures renderable)
        {
            // Make Rectangle out of Renderable
            var renderableRectangle = new Rectangle(renderable, true);
            var texture = renderable.Texture.Item2;
            var layers = renderable.Texture.Item1;

            // Check if Texture Data is already stored, if not, add Texture
            if (!this.TextureData.ContainsKey(texture.FilePath))
            {
                this.TextureData[texture.FilePath] = new TextureData(texture);
            }

            // Get the bounds of the Renderable and check if it can be skipped
            if (!this.IsDrawNeeded(renderableRectangle))
            {
                return;
            }

            if (layers.Any(e => !e.IsXYAligned))
            {
                if (true) ;
            }

            var renderableRect = new Rectangle(renderable, true);

            this.TextureData[texture.FilePath].Enable();
            foreach (var layer in layers)
            {
                var debug = new TextureCoordinates(new Vector2(0, 1), new Vector2(.2f, 1), new Vector2(.2f, .75f), new Vector2(.0f, .75f));
                this.RenderRectangle(renderableRect, layer);
            }

            this.TextureData[texture.FilePath].Disable();
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

            this.TextureData[renderable.Texture.FilePath].Enable();
            this.RenderRectangle(renderableRectangle, renderable.Texture.TextureCoordinates);
            this.TextureData[renderable.Texture.FilePath].Disable();
        }

        private void RenderRectangle(Rectangle rect, TextureCoordinates texCoords)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            this.Camera.DrawRectangle(rect, texCoords, this.Screen);
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
