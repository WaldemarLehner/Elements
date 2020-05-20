using System;
using System.Drawing;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ComputergrafikSpiel.View.Helpers
{
    internal static class IRenderableHelper
    {
        /// <summary>
        /// This is a basic debug draw function that does not take a texture. It will draw a solid color rectangle with the <seealso cref="Model.IRenderable"/> parameters.
        /// </summary>
        /// <param name="renderable">The <see cref="Model.IRenderable"/> object containing position, rotation and size.</param>
        /// <param name="screenWidth">The screen X-dimension.</param>
        /// <param name="screenHeight">The screen Y-dimension.</param>
        /// <param name="drawingColor">The optional color to be drawn. If no color is specified, Gray will be used.</param>
        /// <param name="drawAnker">Should the rotational anker be drawn.</param>
        /// <param name="drawPosition">Should the position Anker be drawn.</param>
        /// <param name="drawGhostBeforeTransformation">Should an outline be drawn of the object before transformation.</param>
        /// <param name="CallGLFunctions">This parameter is for unit tests. It can be set to false so GL.[] functions are no longer called.</param>
        internal static void RenderRectangleDebug(this IRenderable renderable, int screenWidth, int screenHeight, Color? drawingColor = null, bool drawAnker = false, bool drawPosition = false, bool drawGhostBeforeTransformation = false, bool CallGLFunctions = true)
        {
            _ = renderable ?? throw new ArgumentNullException(nameof(renderable));
            if (screenHeight <= 0)
            {
                throw new Exceptions.ArgumentNotPositiveIntegerGreaterZeroException(nameof(screenHeight));
            }

            if (screenWidth <= 0)
            {
                throw new Exceptions.ArgumentNotPositiveIntegerGreaterZeroException(nameof(screenWidth));
            }

            // Make a rectangle out of the Renderable
            Rectangle rectangle = new Rectangle(renderable, true);

            // Only Draw Calls from here. If the Draw Variable is set to false, return here
            if (!CallGLFunctions)
            {
                return;
            }

            // Draw rectangle
            GL.Color4(drawingColor ?? Color.Gray);
            GL.Begin(PrimitiveType.Quads);
            AddGLRectangleVertices(rectangle, screenWidth, screenHeight);
            GL.End();

            // If the rendering of the ghost is set to true, render the ghost
            if (drawGhostBeforeTransformation)
            {
                GL.Color4(Color.Red);
                GL.Begin(PrimitiveType.LineLoop);
                rectangle = new Rectangle(renderable, false);
                AddGLRectangleVertices(rectangle, screenWidth, screenHeight);
                GL.End();
            }

            // If the rendering of the Anker is set to true, render the point as a cross
            if (drawAnker)
            {
                Vector2 anker = renderable.RotationAnker;
                GL.Color4(Color.Green);
                AddGLPointAsCross(anker, screenWidth, screenHeight);
            }

            // If the rending of the Position is set to true, render the point as a cross
            if (drawPosition)
            {
                Vector2 position = renderable.Position;
                GL.Color4(Color.Magenta);
                AddGLPointAsCross(position, screenWidth, screenHeight);
            }
        }

        private static void AddGLPointAsCross(Vector2 location, int screenWidth, int screenHeight, int lineLength = 10)
        {
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(location.X.NormalizeGL(screenWidth), (location.Y - lineLength).NormalizeGL(screenHeight));
            GL.Vertex2(location.X.NormalizeGL(screenWidth), (location.Y + lineLength).NormalizeGL(screenHeight));
            GL.End();

            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2((location.X - lineLength).NormalizeGL(screenWidth), location.Y.NormalizeGL(screenHeight));
            GL.Vertex2((location.X + lineLength).NormalizeGL(screenWidth), location.Y.NormalizeGL(screenHeight));
            GL.End();
        }

        private static void AddGLRectangleVertices(Rectangle rectangle, int screenWidth, int screenHeight)
        {
            GL.Vertex2(rectangle.TopLeft.NormalizeGL(screenWidth, screenHeight));
            GL.Vertex2(rectangle.TopRight.NormalizeGL(screenWidth, screenHeight));
            GL.Vertex2(rectangle.BottomRight.NormalizeGL(screenWidth, screenHeight));
            GL.Vertex2(rectangle.BottomLeft.NormalizeGL(screenWidth, screenHeight));
        }
    }
}
