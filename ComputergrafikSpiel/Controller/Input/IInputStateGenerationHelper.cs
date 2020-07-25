﻿using ComputergrafikSpiel.Model.Interfaces;
using ComputergrafikSpiel.View.Renderer.Interfaces;
using OpenTK;
using OpenTK.Input;

namespace ComputergrafikSpiel.Controller.Input
{
    internal static class IInputStateGenerationHelper
    {
        internal static IInputState GenerateInputState(IRenderer renderer, Vector2 cursorNDC)
        {
            var cursor = new MouseCursor();
            cursor.Update(renderer, cursorNDC);
            return new InputState
            {
                MouseState = Mouse.GetCursorState(),
                KeyboardState = Keyboard.GetState(),
                Cursor = cursor,
            };
        }

        private class InputState : IInputState
        {
            public MouseState MouseState { get; set; }

            public KeyboardState KeyboardState { get; set; }

            public IMouseCursor Cursor { get; set; }
        }
    }
}
