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
        private float obstaclePropability; // Spawn der Obstacles Anzahl
        private float noiseScale; // Spawn der Anzahl an Gewässer / Boden
        private WorldEnum.Type elementType;

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

            switch (this.setDifferentDungeons)
            {
                case 1:
                    this.SetSceneTexturesToForest();
                    this.elementType = WorldEnum.Type.Water;
                    break;
                case 11:
                    this.SetSceneTexturesToFire();
                    this.elementType = WorldEnum.Type.Earth;
                    break;
                case 21:
                    this.SetSceneTexturesToForest();
                    this.elementType = WorldEnum.Type.Fire;
                    break;
                case 31:
                    this.SetSceneTexturesToFire();
                    this.elementType = WorldEnum.Type.Air;
                    break;
                default: break;
            }

            (this.Model as Model).FirstScene = false;
            Scene.Current.Disable();

            // Keine Obstacles & Gewässer bei Bossräumen Überprüfung
            if (this.setDifferentDungeons % 10 == 0)
            {
                this.obstaclePropability = .0f;
                this.noiseScale = .0f;
            }
            else
            {
                this.obstaclePropability = .05f;
                this.noiseScale = .1f;
            }

            var worldScene = new WorldSceneGenerator(this.obstaclePropability, new WorldSceneDefinition(true, true, true, true, 20, 15, this.noiseScale, 32, WorldSceneDefinition.DefaultMapping)).GenerateWorldScene();
            var newScene = new Scene(worldScene);
            newScene.GiveModeltoScene(this.Model);
            newScene.SetAsActive();

            switch (this.setDifferentDungeons)
            {
                case 10:
                    newScene.SpawningEnemies(newScene.World, this.elementType, true);
                    break;
                case 20:
                    newScene.SpawningEnemies(newScene.World, this.elementType, true);
                    break;
                case 30:
                    newScene.SpawningEnemies(newScene.World, this.elementType, true);
                    break;
                case 40:
                    newScene.SpawningEnemies(newScene.World, this.elementType, true);
                    break;
                default: newScene.SpawningEnemies(newScene.World, this.elementType, false);
                    break;
            }
        }

        public void InitializeFirstScene()
        {
            var worldScene = new WorldSceneGenerator(this.obstaclePropability, new WorldSceneDefinition(true, true, true, true, 20, 15, .1f, 32, WorldSceneDefinition.DefaultMapping)).GenerateWorldScene();
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
