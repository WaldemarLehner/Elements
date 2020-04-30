using System.Collections.Generic;

namespace ComputergrafikSpiel.Model
{
    internal interface IPlayerControl
    {
        // Receives a enum list of pressed player actions -> MoveUp, MoveDown, MoveLeft, MoveRight, Dash, Attack
        void PlayerControl(IReadOnlyList<PlayerActionEnum.PlayerActions> actions);
    }
}