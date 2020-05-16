using System.Collections.Generic;

namespace ComputergrafikSpiel.Model
{
    internal interface IPlayerControl : IPlayer
    {
        // Receives a enum list of pressed player actions -> MoveUp, MoveDown, MoveLeft, MoveRight, Dash, Attack, Interaction
        void PlayerControl(IReadOnlyList<PlayerActionEnum.PlayerActions> actions);
    }
}