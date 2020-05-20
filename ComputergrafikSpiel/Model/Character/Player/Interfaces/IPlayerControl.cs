using System.Collections.Generic;

namespace ComputergrafikSpiel.Model.Character.Player.Interfaces
{
    internal interface IPlayerControl : IPlayer
    {
        // Receives a enum list of pressed player actions -> MoveUp, MoveDown, MoveLeft, MoveRight, Dash, Attack, Interaction
        void PlayerControl(IReadOnlyList<PlayerEnum.PlayerActions> actions);
    }
}