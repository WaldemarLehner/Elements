using System.Collections.Generic;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.Overlay.EndScreen;
using ComputergrafikSpiel.Model.Overlay.UpgradeScreen;
using ComputergrafikSpiel.Model.Scene;

namespace ComputergrafikSpiel.Model.Interfaces
{
    public interface IModel
    {
        (float top, float bottom, float left, float right) CurrentSceneBounds { get; }

        IEnumerable<IRenderable> Renderables { get; }

        IInputState InputState { get; }

        int Level { get; }

        UpgradeScreen UpgradeScreen { get; }

        EndScreen EndScreen { get; }

        ISceneManager SceneManager { get; set; }

        void Update(float dTime);

        void UpdateInput(IInputState input);
    }
}
