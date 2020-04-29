﻿namespace ComputergrafikSpiel.View
{
    public interface IRenderer
    {
        /// <summary>
        /// Triggers the Rendering of the current IRenderables.
        /// </summary>
        void Render();

        /// <summary>
        /// Informs the View that the screen Size has been changed.
        /// </summary>
        /// <param name="screenWidth">The screen's width.</param>
        /// <param name="screenHeight">The screen's height.</param>
        void Resize(int screenWidth, int screenHeight);
    }
}
