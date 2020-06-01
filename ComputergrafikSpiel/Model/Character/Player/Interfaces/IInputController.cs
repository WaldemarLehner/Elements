namespace ComputergrafikSpiel.Model.Character.Player.Interfaces
{
    public interface IInputController
    {
        void PlayerAction();

        void HookPlayer(IPlayer control);
    }
}