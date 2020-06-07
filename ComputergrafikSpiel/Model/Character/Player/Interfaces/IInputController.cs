using ComputergrafikSpiel.View.Interfaces;
using ComputergrafikSpiel.View.Renderer.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.Character.Player.Interfaces
{
    public interface IInputController
    {
        void PlayerAction(IRenderer renderer, Vector2 cursorNdc);

        void HookPlayer(IPlayer control);
    }
}