using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.Interfaces;
using ComputergrafikSpiel.Model.Soundtrack;
using ComputergrafikSpiel.Model.World;
using OpenTK;

namespace ComputergrafikSpiel.Model.Scene
{
    public class SceneManager : ISceneManager
    {
        private readonly Soundloader play = new Soundloader();

        public SceneManager(IModel model)
        {
            // Szene wird gestartet, ebenso direkt in der Safe-Zone wird das Lied gespielt
            this.play.StartSafeMusic();
            this.Model = model;
        }

        private IModel Model { get; }

        public void LoadNewScene()
        {
            if (this.play.battleMusicOn == false)
            {
                // Bei Beginn des Schlachtfeldes wird die Battlemusik gestartet
                this.play.StartBattleMusic();
                this.play.battleMusicOn = true;
            }

            (this.Model as Model).FirstScene = false;
            Scene.Current.Disable();
            this.SetSceneTexturesToForest(); // Ändert die Texturen zu Wald
            var worldScene = new WorldSceneGenerator(new WorldSceneDefinition(true, true, true, true, 20, 15, .1f, 32, WorldSceneDefinition.DefaultMapping)).GenerateWorldScene();
            var newScene = new Scene(worldScene);
            newScene.GiveModeltoScene(this.Model);
            newScene.SetAsActive();
            newScene.SpawningEnemies(newScene.World);
        }

        public void InitializeFirstScene()
        {
            var worldScene = new WorldSceneGenerator(new WorldSceneDefinition(true, true, true, true, 20, 15, .1f, 32, WorldSceneDefinition.DefaultMapping)).GenerateWorldScene();
            var initScene = new Scene(worldScene);
            (this.Model as Model).FirstScene = true;
            initScene.GiveModeltoScene(this.Model);
            initScene.SetAsActive();
        }

        public void SetSceneTexturesToForest()
        {
            WorldTileTextureLoader.NameLookUp[TileDefinitions.Type.Water] = "Ground_Forest/WaterTileSet";
            WorldTileTextureLoader.NameLookUp[TileDefinitions.Type.Dirt] = "Ground_Forest/EarthTileSet";
            WorldTileTextureLoader.NameLookUp[TileDefinitions.Type.Grass] = "Ground_Forest/Grass";
        }
    }
}
