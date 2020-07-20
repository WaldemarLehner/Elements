using ComputergrafikSpiel.Model.Interfaces;
using ComputergrafikSpiel.View.Renderer.Interfaces;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputergrafikSpiel.Controller.Input
{
    internal static class IInputStateGenerationHelper
    {
        internal static IInputState GenerateInputState(IRenderer renderer, Vector2 cursorNDC)
        {
            var state = new InputState();
            state.MouseState = Mouse.GetCursorState();
            state.KeyboardState = Keyboard.GetState();
            var cursor = new MouseCursor();
            cursor.Update(renderer, cursorNDC);
            state.Cursor = cursor;
            return state;
        }

        private class InputState : IInputState
        {

            public MouseState MouseState { get; set; }

            public KeyboardState KeyboardState { get; set; }

            public IMouseCursor Cursor { get; set; }
        }

    }
}
