using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
using ComputergrafikSpiel.Model.Overlay;
using ComputergrafikSpiel.Model.Scene;
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
            this.Debug = 0; // DebugMask.Mask.DebugData | DebugMask.Mask.IndependentDebugData;
        }

        public bool Active { get; private set; } = true;

        public ICamera Camera { get; private set; }

        public DebugMask.Mask Debug { get; set; }

        public (int width, int height) Screen { get; private set; }

        private IEnumerable<IRenderable> RenderablesEnumerator => this.model.Renderables;

        private IEnumerable<IGUIElement[]> GUIRenderables => this.model.UiRenderables;

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
                else if (entry is IRenderableBackground)
                {
                    this.RenderBackground(entry as IRenderableBackground);
                }
                else
                {
                    this.RenderRenderable(entry);
                }

            }

            // Render the GUI Elements
            foreach (var entryGroup in this.GUIRenderables)
            {
                foreach (var _entry in entryGroup)
                {
                    var entry = OpenTKRendererHelper.PopulateMissingDataGUIRenderables(_entry, this);
                    this.RenderGUIElement(entry as IGUIElement);
                }
            }

            if (this.Debug != 0)
            {
                foreach (var entry in this.RenderablesEnumerator)
                {
                    var rand = new Random(13456);
                    OpenTKRendererHelper.RenderRenderableDebug(this, entry, rand, this.Debug);
                }

                if ((this.Debug & DebugMask.Mask.IndependentDebugData) != 0)
                {
                    foreach (var entry in Scene.Current.IndependentDebugData)
                    {
                        OpenTKRendererHelper.RenderItemDebug(this, entry.verts, entry.color);
                    }

                    foreach (var entry in Scene.Current.ColliderManager.CollidableEntitiesCollection)
                    {
                        OpenTKRendererHelper.RenderItemDebug(this, entry.Collider.DebugData.verts, new Color4(255, 0, 0, 255));
                    }

                    Scene.Current.IndependentDebugData.Clear();
                }
            }
        }

        private void RenderBackground(IRenderableBackground renderableBackground)
        {
            var alignedItem = new Rectangle(renderableBackground);
            this.CreateTextureDataIfNeeded(renderableBackground.Texture, renderableBackground.WrapMode);

            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            this.TextureData[renderableBackground.Texture.FilePath].Enable();
            this.Camera.DrawAsBackground(alignedItem, this.Screen);
            this.TextureData[renderableBackground.Texture.FilePath].Disable();

            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Blend);
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
            var renderableRectangle = new Rectangle(renderable, true, true);
            var texture = renderable.Texture.Item2;
            var layers = renderable.Texture.Item1;

            this.CreateTextureDataIfNeeded(texture, TextureWrapMode.Repeat);

            // Get the bounds of the Renderable and check if it can be skipped
            if (!this.IsDrawNeeded(renderableRectangle))
            {
                return;
            }

            var renderableRect = new Rectangle(renderable, true, true);

            this.TextureData[texture.FilePath].Enable();
            foreach (var layer in layers)
            {
                var debug = new TextureCoordinates(new Vector2(0, 1), new Vector2(.2f, 1), new Vector2(.2f, .75f), new Vector2(.0f, .75f));
                this.RenderRectangle(renderableRect, layer);
            }

            this.TextureData[texture.FilePath].Disable();
        }

        private void CreateTextureDataIfNeeded(ITexture texture, TextureWrapMode wrapMode = TextureWrapMode.MirroredRepeat)
        {
            // Check if Texture Data is already stored, if not, add Texture
            if (!this.TextureData.ContainsKey(texture.FilePath))
            {
                this.TextureData[texture.FilePath] = new TextureData(texture, wrapMode);
            }
        }

        private void RenderRenderable(IRenderable renderable)
        {
            // Make Rectangle out of Renderable
            var renderableRectangle = new Rectangle(renderable, true, true);

            // Check if Texture Data is already stored, if not, add Texture
            this.CreateTextureDataIfNeeded(renderable.Texture);

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

        // see: https://i.imgur.com/2FaoJuX.png
        private void RenderGUIElement(IGUIElement element)
        {
            // Assumes that element has full positional data.
            var centre = element.Offset;

            // NDC Coordinate Bounds
            (float bottom, float top, float left, float right) = (centre.Y - element.Size.height ?? 0 / 2f, centre.Y + element.Size.height ?? 0 / 2f, centre.X - element.Size.width ?? 0 / 2f, centre.X + element.Size.width ?? 0 / 2f);

            // Check if Texture Data is already stored, if not, add Texture.
            this.CreateTextureDataIfNeeded(element.Texture);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            (Vector2 vert, Vector2 tex)[] verTexCoords = new (Vector2 vert, Vector2 tex)[]
            {
                (new Vector2(left, top), new Vector2(0,1)),
                (new Vector2(right, top), Vector2.One),
                (new Vector2(right, bottom), new Vector2(1,0)),
                (new Vector2(left, bottom), Vector2.Zero),
            };
            this.TextureData[element.Texture.FilePath].Enable();
            GL.Begin(PrimitiveType.Quads);
            foreach (var (vert, tex) in verTexCoords)
            {
                GL.TexCoord2(tex);
                GL.Vertex2(vert);
            }

            GL.End();
            this.TextureData[element.Texture.FilePath].Disable();
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
