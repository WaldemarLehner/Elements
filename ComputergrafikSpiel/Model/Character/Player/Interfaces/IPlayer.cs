using System;
using System.Collections.Generic;

namespace ComputergrafikSpiel.Model.Character.Player.Interfaces
{
    public interface IPlayer : ICharacter
    {
        event EventHandler PlayerInc;

        // Receives a enum list of pressed player actions -> MoveUp, MoveDown, MoveLeft, MoveRight, Dash, Attack, Interaction
        void PlayerControl(IReadOnlyList<PlayerEnum.PlayerActions> actions);

        void OnInc(EventArgs e);

        void TakingDamage(int damage);

        void IncreasePlayerStats(int incNumber, IReadOnlyList<PlayerEnum.Stats> incstats);
    }
}