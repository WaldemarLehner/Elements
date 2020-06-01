namespace ComputergrafikSpiel.Model.Character.Player.Interfaces
{
    internal interface IInputController
    {
        void PlayerAction();

        void HookPlayer(IPlayer control);
    }
}