﻿using System;
using System.Collections.Generic;
using ComputergrafikSpiel.View.Helpers;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ComputergrafikSpiel.View
{
    internal class OpenTKRenderer : IRenderer
    {
        internal OpenTKRenderer(IReadOnlyCollection<Model.IRenderable> renderables)
        {
            _ = renderables ?? throw new ArgumentNullException(nameof(renderables));
            this.RenderablesCollection = renderables;
        }

        public Tuple<int, int> Screen { get; private set; }

        private IReadOnlyCollection<Model.IRenderable> RenderablesCollection { get; }

        public void Render()
        {
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
            if (screenHeight <= 0)
            {
                throw new Exceptions.ArgumentNotPositiveIntegerGreaterZeroException(nameof(screenHeight));
            }

            if (screenWidth <= 0)
            {
                throw new Exceptions.ArgumentNotPositiveIntegerGreaterZeroException(nameof(screenWidth));
            }

            this.Screen = new Tuple<int, int>(screenWidth, screenHeight);
        }

        private void RenderRenderable(Model.IRenderable renderable)
        {
            renderable.RenderRectangleDebug(
                this.Screen.Item1,
                this.Screen.Item2,
                drawAnker: true,
                drawPosition: true,
                drawGhostBeforeTransformation: true);
        }
    }
}