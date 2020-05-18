using System;

namespace ComputergrafikSpiel.Model
{
    internal interface ICharacter
    {
        event EventHandler CharacterDeath;

        event EventHandler CharacterHit;

        event EventHandler CharacterMove;

        int MaxHealth { get; }

        int MovementSpeed { get; }

        int Defense { get; }

        void OnDeath(EventArgs e);

        void OnHit(EventArgs e);

        void OnMove(EventArgs e);
    }
}
