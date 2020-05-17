using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputergrafikSpiel.Model
{
    internal interface ICharacter : IRenderable
    {
        int MaxHealth { get; }

        int CurrentHealth { get; }

        int MovementSpeed { get; }

        int Defense { get; }

        event EventHandler CharacterDeath;

        event EventHandler CharacterHit;

        event EventHandler CharacterMove;

        void OnDeath(EventArgs e);

        void OnHit(EventArgs e);

        void OnMove(EventArgs e);
    }
}
