using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputergrafikSpiel.Model
{
    internal interface ICharacter
    {
        int MaxHealth { get; }

        int MovementSpeed { get; }

        int AttackSpeed { get; }

        event EventHandler PlayerDeath;

        event EventHandler PlayerHit;

        event EventHandler PlayerMove;

        void OnDeath(EventArgs e);

        void OnHit(EventArgs e);

        void OnMove(EventArgs e);
    }
}
