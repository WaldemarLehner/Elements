using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
using ComputergrafikSpiel.Model.Scene;
using ComputergrafikSpiel.View.Interfaces;
using ComputergrafikSpiel.View.Renderer.Interfaces;
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
            this.Debug = DebugMask.Mask.DebugData | DebugMask.Mask.IndependentDebugData;
        }

        public bool Active { get; private set; } = true;

        public ICamera Camera { get; private set; }

        public DebugMask.Mask Debug { get; set; }

        public (int width, int height) Screen { get; private set; }

        public Dictionary<string, TextureData> TextureData { get; set; }

        private IEnumerable<IRenderable> RenderablesEnumerator => this.model.Renderables;

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
                    OpenTKRenderingHelper.RenderRenderableLayered(this, entry as IRenderableLayeredTextures);
                }
                else if (entry is IRenderableBackground)
                {
                    OpenTKRenderingHelper.RenderBackground(this, entry as IRenderableBackground);
                }
                else if (entry is IParticle)
                {
                    OpenTKRenderingHelper.RenderParticle(this, entry as IParticle);
                }
                else
                {
                    OpenTKRenderingHelper.RenderRenderable(this, entry);
                }
            }

            if (this.Debug != 0)
            {
                foreach (var entry in this.RenderablesEnumerator)
                {
                    var rand = new Random(13456);
                    OpenTKDebugRenderingHelper.RenderRenderableDebug(this, entry, rand, this.Debug);
                }

                if ((this.Debug & DebugMask.Mask.IndependentDebugData) != 0)
                {
                    foreach (var (color, verts) in Scene.Current.IndependentDebugData)
                    {
                        OpenTKDebugRenderingHelper.RenderItemDebug(this, verts, color);
                    }

                    foreach (var entry in Scene.Current.ColliderManager.CollidableEntitiesCollection)
                    {
                        OpenTKDebugRenderingHelper.RenderItemDebug(this, entry.Collider.DebugData.verts, new Color4(255, 0, 0, 255));
                    }

                    Scene.Current.IndependentDebugData.Clear();
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
    }
}
