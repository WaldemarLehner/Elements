using System.Collections.Generic;
using System.Net.NetworkInformation;
using ComputergrafikSpiel.Model.Overlay.EndScreen;
using ComputergrafikSpiel.Model.Overlay.UpgradeScreen;

namespace ComputergrafikSpiel.Model.Character.Player
{
    internal interface IPlayerStateManager
    {
        IPlayerState Current { get; }

        void Hurt(ref bool died);

        bool Heal();

        void AddCoin(uint value);

        IList<UpgradeOption> GetUpgradeOptions(uint level);
    }
}