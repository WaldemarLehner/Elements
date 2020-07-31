using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputergrafikSpiel.Model.Scene
{
    public interface ISceneManager
    {
        void LoadNewScene();

        void InitializeFirstScene();

        void SetSceneTexturesToWater();

        void SetSceneTexturesToEarth();

        void SetSceneTexturesToFire();

        void SetSceneTexturesToAir();
    }
}
