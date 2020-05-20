using System;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;

namespace ComputergrafikSpiel.Model.Character
{
    internal interface ICharacter : IRenderable
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
