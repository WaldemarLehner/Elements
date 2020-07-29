using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.Interfaces;
using ComputergrafikSpiel.Model.Soundtrack;
using ComputergrafikSpiel.Model.World;

namespace ComputergrafikSpiel.Model.Scene
{
    public class SceneManager : ISceneManager
    {
        private readonly Soundloader play = new Soundloader();
        private int setDifferentDungeons = 0;

        public SceneManager(IModel model)
        {
            // Szene wird gestartet, ebenso direkt in der Safe-Zone wird das Lied gespielt
            this.play.StartSafeMusic();
            this.Model = model;
        }

        private IModel Model { get; }

        public void LoadNewScene()
        {
            this.setDifferentDungeons++;

            if (this.play.battleMusicOn == false)
            {
                // Bei Beginn des Schlachtfeldes wird die Battlemusik gestartet
                this.play.StartBattleMusic();
                this.play.battleMusicOn = true;
            }

            // bei Raum 1 werden Waldtexturen geladen
            if (this.setDifferentDungeons == 1)
            {
                this.SetSceneTexturesToForest();
            }

            // bei Raum 11 wird der Dungeon zu Fire geändert mit Texturen
            if (this.setDifferentDungeons == 11)
            {
                this.SetSceneTexturesToFire();
            }

            (this.Model as Model).FirstScene = false;
            Scene.Current.Disable();
            var worldScene = new WorldSceneGenerator(new WorldSceneDefinition(true, true, true, true, 20, 15, .1f, 32, WorldSceneDefinition.DefaultMapping)).GenerateWorldScene();
            var newScene = new Scene(worldScene);
            newScene.GiveModeltoScene(this.Model);
            newScene.SetAsActive();
            newScene.SpawningEnemies(newScene.World);

            // bei Raum 10 wird der Waldboss mitgespawnt
            if (this.setDifferentDungeons == 10)
            {
                newScene.SpawningForestBoss(newScene.World);
            }
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

        public void SetSceneTexturesToFire()
        {
            WorldTileTextureLoader.NameLookUp[TileDefinitions.Type.Water] = "Ground_Fire/WaterTileSet";
            WorldTileTextureLoader.NameLookUp[TileDefinitions.Type.Dirt] = "Ground_Fire/EarthTileSet";
            WorldTileTextureLoader.NameLookUp[TileDefinitions.Type.Grass] = "Ground_Fire/Grass";
        }

        public void SetSceneTexturesToForestBoss()
        {
            // Bodentexturen für Forest Boss
            WorldTileTextureLoader.NameLookUp[TileDefinitions.Type.Water] = "Ground_Forest/WaterTileSet";
            WorldTileTextureLoader.NameLookUp[TileDefinitions.Type.Dirt] = "Ground_Forest/EarthTileSet";
            WorldTileTextureLoader.NameLookUp[TileDefinitions.Type.Grass] = "Ground_Forest/Grass";
        }
    }
}
