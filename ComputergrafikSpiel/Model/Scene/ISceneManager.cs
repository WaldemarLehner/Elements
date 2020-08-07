using ComputergrafikSpiel.Model.Soundtrack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputergrafikSpiel.Model.Scene
{
    public interface ISceneManager
    {
        Soundloader Play { get; set; }

        int SetDifferentDungeons { set; }

        void LoadNewScene();

        void InitializeFirstScene();

        void SetSceneTexturesToSafeZone();

        void SetSceneTexturesToWater();

        void SetSceneTexturesToEarth();

        void SetSceneTexturesToFire();

        void SetSceneTexturesToAir();
    }
}
