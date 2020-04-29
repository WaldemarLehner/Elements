using System.Collections.Generic;

namespace ComputergrafikSpiel.Model
{
    internal interface IPlayerControl
    {
        // Receives a enum list of pressed player actions -> Up, Down, Left, Right, Dash, Attack
        void PlayerControl(IReadOnlyList<PlayerActionEnum.PlayerActions> actions);
    }
}