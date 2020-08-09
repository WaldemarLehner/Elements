using ComputergrafikSpiel.Model.Soundtrack;

namespace ComputergrafikSpiel.Model.Scene
{
    public interface ISceneManager
    {
        Soundloader Play { get; set; }

        int CurrentDungeon { get; set; }

        int CurrentDungeonRoom { get; set; }

        int CurrentStageLevel { get; set; }

        void LoadNewScene();

        void InitializeFirstScene();

        void SetSceneTexturesToSafeZone();

        void SetSceneTexturesToWater();

        void SetSceneTexturesToEarth();

        void SetSceneTexturesToFire();

        void SetSceneTexturesToAir();
    }
}
