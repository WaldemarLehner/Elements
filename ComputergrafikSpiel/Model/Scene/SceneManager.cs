﻿using ComputergrafikSpiel.Model.Interfaces;
using ComputergrafikSpiel.Model.World;

namespace ComputergrafikSpiel.Model.Scene
{
    public class SceneManager : ISceneManager
    {
        public SceneManager(IModel model)
        {
            this.Model = model;
        }

        private IModel Model { get; }

        public void LoadNewScene()
        {
            Scene.Current.Disable();
            var worldScene = new WorldSceneGenerator(new WorldSceneDefinition(true, true, true, true, 20, 15, .1f, 32, WorldSceneDefinition.DefaultMapping)).GenerateWorldScene();
            var newScene = new Scene(worldScene, this.Model);
            newScene.SetAsActive();
        }

        public void InitializeFirstScene()
        {
            var worldScene = new WorldSceneGenerator(new WorldSceneDefinition(true, true, true, true, 20, 15, .1f, 32, WorldSceneDefinition.DefaultMapping)).GenerateWorldScene();
            new Scene(worldScene, this.Model);
        }
    }
}
