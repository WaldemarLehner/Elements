using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.View.Helpers;
using ComputergrafikSpiel.View.Renderer.Interfaces;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace ComputergrafikSpiel.View.Renderer
{
    internal static class OpenTKRendererHelper
    {
        internal static void RenderRenderableDebug(IRenderer renderer, IRenderable renderable, Random rand, DebugMask.Mask mask)
        {
            byte[] buf = new byte[3];
            rand.NextBytes(buf);

            if ((mask & DebugMask.Mask.TextureBoundingBox) != 0)
            {
                var rect = new Rectangle(renderable, true);
                var vertsWorldSpace = new Vector2[] { rect.TopLeft, rect.TopRight, rect.BottomRight, rect.BottomLeft };

                RenderItemDebug(renderer, vertsWorldSpace, new Color4(buf[0], buf[1], buf[2], 0x10));
            }

            if ((mask & DebugMask.Mask.DebugData) != 0)
            {
                if (renderable.DebugData != null)
                {
                    foreach (var debugData in renderable.DebugData)
                    {
                        Color4 color;
                        if (debugData.color == null)
                        {
                            var randBytes = new byte[3];
                            rand.NextBytes(randBytes);
                            color = new Color4(randBytes[0], randBytes[1], randBytes[2], 0xFF);
                        }
                        else
                        {
                            color = debugData.color;
                        }

                        RenderItemDebug(renderer, debugData.vertices, color);
                    }
                }
            }
        }

        internal static void RenderItemDebug(IRenderer renderer, Vector2[] vertsWorldSpace, Color4 color)
        {
            var vertsNDC = new List<Vector2>();

            foreach (var vert in vertsWorldSpace)
            {
                var multipliers = CameraCoordinateConversionHelper.CalculateAspectRatioMultiplier(renderer.Camera.AspectRatio, renderer.Screen.width / (float)renderer.Screen.height);
                vertsNDC.Add(CameraCoordinateConversionHelper.WorldToNDC(vert, multipliers, renderer.Camera));
            }

            GL.Color4(color);
            GL.Begin(PrimitiveType.LineStrip);
            foreach (var vert in vertsNDC)
            {
                GL.Vertex2(vert);
            }

            GL.End();
            GL.Color4(Color4.White);
        }
    }
}
