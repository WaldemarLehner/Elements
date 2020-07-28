using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputergrafikSpiel.Model.Soundtrack
{
    public interface ISoundloader
    {
        void StartMainThemeMusic();

        void StartSafeMusic();

        void StartBattleMusic();

        void StartBossMusic();

        void StopMusik();
    }
}
