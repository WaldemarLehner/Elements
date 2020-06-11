using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Controller.Input;

namespace ComputergrafikSpiel.Model.Character.Player.Interfaces
{
    public interface IPlayer : ICharacter
    {
        event EventHandler PlayerInc;

        // Receives a enum list of pressed player actions -> MoveUp, MoveDown, MoveLeft, MoveRight, Dash, Attack, Interaction
        void PlayerControl(List<PlayerEnum.PlayerActions> pressedActions, MouseCursor mouseCursor);

        void OnInc(EventArgs e);

        void TakingDamage(int damage);

        void IncreasePlayerStats(int incNumber, IReadOnlyList<PlayerEnum.Stats> incstats);
    }
}