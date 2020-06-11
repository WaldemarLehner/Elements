using System;
using ComputergrafikSpiel.View.Renderer.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Controller.Input
{
    public class MouseCursor
    {
        public Vector2 WindowNDCCoordinates { get; private set; }

        public Vector2? WorldCoordinates { get; private set; }

        public void Update(IRenderer renderer, Vector2 cursorNDC)
        {
            _ = renderer ?? throw new ArgumentNullException(nameof(renderer));

            // Cursor has origin in top left corner, rest of this program expects bottom left for screen Coordinates
            cursorNDC.Y *= -1;
            this.WindowNDCCoordinates = cursorNDC;
            this.WorldCoordinates = renderer.Camera.NDCToWorld(cursorNDC);
        }
    }
}