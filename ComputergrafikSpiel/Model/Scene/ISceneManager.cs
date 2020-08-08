using ComputergrafikSpiel.Model.Soundtrack;

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
