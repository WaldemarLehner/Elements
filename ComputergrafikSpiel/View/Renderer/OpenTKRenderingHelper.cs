using System.Collections.Generic;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using ComputergrafikSpiel.View.Renderer.Interfaces;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ComputergrafikSpiel.View.Renderer
{
    internal static class OpenTKRenderingHelper
    {
        internal static void RenderRenderableLayered(IRenderer renderer, IRenderableLayeredTextures renderable)
        {
            // Make Rectangle out of Renderable
            var renderableRectangle = new Rectangle(renderable, true, true);
            var texture = renderable.Texture.Item2;
            var layers = renderable.Texture.Item1;
            OpenTKRenderingHelper.CreateTextureDataIfNeeded(renderer, texture, TextureWrapMode.Repeat);

            // Get the bounds of the Renderable and check if it can be skipped
            if (!OpenTKRenderingHelper.IsDrawNeeded(renderer, renderableRectangle))
            {
                return;
            }

            var renderableRect = new Rectangle(renderable, true, true);

            renderer.TextureData[texture.FilePath].Enable();
            foreach (var layer in layers)
            {
                OpenTKRenderingHelper.RenderRectangle(renderer, renderableRect, layer);
            }

            renderer.TextureData[texture.FilePath].Disable();
        }

        internal static void RenderRenderable(IRenderer renderer, IRenderable renderable)
        {
            var renderableRectangle = new Rectangle(renderable, true, true);

            // Check if Texture Data is already stored, if not, add Texture
            OpenTKRenderingHelper.CreateTextureDataIfNeeded(renderer, renderable.Texture);

            // Get the bounds of the Renderable and check if it can be skipped
            if (!OpenTKRenderingHelper.IsDrawNeeded(renderer, renderableRectangle))
            {
                return;
            }

            renderer.TextureData[renderable.Texture.FilePath].Enable();
            OpenTKRenderingHelper.RenderRectangle(renderer, renderableRectangle, renderable.Texture.TextureCoordinates);
            renderer.TextureData[renderable.Texture.FilePath].Disable();
        }

        internal static void RenderBackground(IRenderer renderer, IRenderableBackground renderableBackground)
        {
            var alignedItem = new Rectangle(renderableBackground);
            OpenTKRenderingHelper.CreateTextureDataIfNeeded(renderer, renderableBackground.Texture, renderableBackground.WrapMode);

            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            renderer.TextureData[renderableBackground.Texture.FilePath].Enable();
            renderer.Camera.DrawAsBackground(alignedItem, renderer.Screen);
            renderer.TextureData[renderableBackground.Texture.FilePath].Disable();

            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Blend);
        }

        private static void CreateTextureDataIfNeeded(IRenderer renderer, ITexture texture, TextureWrapMode wrapMode = TextureWrapMode.MirroredRepeat)
        {
            // Check if Texture Data is already stored, if not, add Texture
            if (!renderer.TextureData.ContainsKey(texture.FilePath))
            {
                renderer.TextureData[texture.FilePath] = new TextureData(texture, wrapMode);
            }
        }

        private static void RenderRectangle(IRenderer renderer, Rectangle rect, TextureCoordinates texCoords)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            renderer.Camera.DrawRectangle(rect, texCoords, renderer.Screen);
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Blend);
        }

        private static bool IsDrawNeeded(IRenderer renderer, Rectangle rect)
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
                if (renderer.Camera.CanPointBeSeenByCamera(point))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
