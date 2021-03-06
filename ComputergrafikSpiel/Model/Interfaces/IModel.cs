﻿using System.Collections.Generic;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.Overlay.EndScreen;
using ComputergrafikSpiel.Model.Overlay.ToggleMute;
using ComputergrafikSpiel.Model.Overlay.UpgradeScreen;
using ComputergrafikSpiel.Model.Scene;
using ComputergrafikSpiel.Model.World;

namespace ComputergrafikSpiel.Model.Interfaces
{
    public interface IModel
    {
        (float top, float bottom, float left, float right) CurrentSceneBounds { get; }

        IEnumerable<IRenderable> Renderables { get; }

        IInputState InputState { get; }

        int Level { get; set; }

        UpgradeScreen UpgradeScreen { get; }

        EndScreen EndScreen { get; set; }

        ToggleMute ToggleMute { get; set; }

        ISceneManager SceneManager { get; set; }

        bool Muted { get; set; }

        void Update(float dTime);

        void UpdateInput(IInputState input);

        void CreateTriggerZone(bool firstScene, bool lastScene, WorldEnum.Type type);
    }
}
