using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputergrafikSpiel.Model
{
    internal class ICharacter
    {

        private int MaxHealth;
        //private int MovementSpeed; müssen noch rein
        //private int AttackSpeed;

        public ICharacter(int passedMaxHealth)
        {
            this.MaxHealth = passedMaxHealth;

            if (this.MaxHealth == 0)
            {
                /*Falls wir Argumente mit einbinden müssen
                 * CharacterEventArgs args = new CharacterEventArgs();
                //args.TimeReached = DateTime.Now;*/
                OnDeath(EventArgs.Empty);
            }
        }

        public event EventHandler PlayerDeath;

        public event EventHandler PlayerHit;

        public event EventHandler PlayerMove;

        protected virtual void OnDeath(EventArgs e)
        {
            EventHandler handler = PlayerDeath;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnHit(EventArgs e)
        {
            EventHandler handler = PlayerHit;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnMove(EventArgs e)
        {
            EventHandler handler = PlayerMove;
            if (handler != null)
            {
                handler(this, e);
            }
        }

    }

    /* Nur Falls wir Argumente brauchen (erstmal aber nicht)
      public class CharacterEventArgs : EventArgs
    {
        public DateTime TimeReached { get; set; }
    }*/
}
