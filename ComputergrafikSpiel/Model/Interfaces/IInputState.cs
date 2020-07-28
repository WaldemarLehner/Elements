using ComputergrafikSpiel.Controller.Input;
using OpenTK;
using OpenTK.Input;

namespace ComputergrafikSpiel.Model.Interfaces
{
    public interface IInputState
    {
        IMouseCursor Cursor { get; }

        MouseState MouseState { get; }

        KeyboardState KeyboardState { get; }

        bool IsWindowFocused { get; }
    }
}