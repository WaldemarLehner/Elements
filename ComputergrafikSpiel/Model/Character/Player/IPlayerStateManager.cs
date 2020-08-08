using System.Collections.Generic;
using ComputergrafikSpiel.Model.Overlay.UpgradeScreen;

namespace ComputergrafikSpiel.Model.Character.Player
{
    internal interface IPlayerStateManager
    {
        IPlayerState Current { get; }

        void Hurt(ref bool died, int damage);

        bool Heal();

        void AddCoin(uint value);

        IList<UpgradeOption> GetUpgradeOptions(uint level);
    }
}