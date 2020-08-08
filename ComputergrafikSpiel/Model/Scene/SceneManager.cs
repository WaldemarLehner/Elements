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

            switch (this.setDifferentDungeons)
            {
                case 1:
                    this.play.StartDungeon1Music();
                    this.SetSceneTexturesToWater();
                    this.elementType = WorldEnum.Type.Water;
                    break;
                case 11:
                    this.play.StartDungeon2Music();
                    this.SetSceneTexturesToEarth();
                    this.elementType = WorldEnum.Type.Earth;
                    this.Model.Level = 1;
                    break;
                case 21:
                    this.play.StartDungeon3Music();
                    this.SetSceneTexturesToFire();
                    this.elementType = WorldEnum.Type.Fire;
                    this.Model.Level = 1;
                    break;
                case 31:
                    this.play.StartDungeon4Music();
                    this.SetSceneTexturesToAir();
                    this.elementType = WorldEnum.Type.Air;
                    this.Model.Level = 1;
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
                    this.play.StartDungeon1BossMusic();
                    newScene.SpawningEnemies(newScene.World, this.elementType, true);
                    break;
                case 20:
                    this.play.StartDungeon2BossMusic();
                    newScene.SpawningEnemies(newScene.World, this.elementType, true);
                    break;
                case 30:
                    this.play.StartDungeon3BossMusic();
                    newScene.SpawningEnemies(newScene.World, this.elementType, true);
                    break;
                case 40:
                    this.play.StartDungeon4BossMusic();
                    newScene.SpawningEnemies(newScene.World, this.elementType, true);
                    break;
                default: newScene.SpawningEnemies(newScene.World, this.elementType, false);
                    break;
            }

            this.Model.CreateTriggerZone(false, false);
        }

        public void InitializeFirstScene()
        {
            var worldScene = new WorldSceneGenerator(this.obstaclePropability, new WorldSceneDefinition(true, true, true, true, 20, 15, .1f, 32, WorldSceneDefinition.DefaultMapping)).GenerateWorldScene();
            var initScene = new Scene(worldScene);
            (this.Model as Model).FirstScene = true;
            initScene.GiveModeltoScene(this.Model);
            initScene.SetAsActive();
            this.Model.CreateTriggerZone(true, false);
        }

        public void SetSceneTexturesToWater()
        {
            WorldTileTextureLoader.NameLookUp[TileDefinitions.Type.Water] = "Ground_Water/WaterTileSet";
            WorldTileTextureLoader.NameLookUp[TileDefinitions.Type.Dirt] = "Ground_Water/EarthTileSet";
            WorldTileTextureLoader.NameLookUp[TileDefinitions.Type.Grass] = "Ground_Water/Grass";
        }

        public void SetSceneTexturesToEarth()
        {
            WorldTileTextureLoader.NameLookUp[TileDefinitions.Type.Water] = "Ground_Earth/EarthAbyssTileSet";
            WorldTileTextureLoader.NameLookUp[TileDefinitions.Type.Dirt] = "Ground_Earth/EarthTileSet";
            WorldTileTextureLoader.NameLookUp[TileDefinitions.Type.Grass] = "Ground_Earth/EarthGround";
        }

        public void SetSceneTexturesToFire()
        {
            WorldTileTextureLoader.NameLookUp[TileDefinitions.Type.Water] = "Ground_Fire/LavaTileSet";
            WorldTileTextureLoader.NameLookUp[TileDefinitions.Type.Dirt] = "Ground_Fire/EarthLavaTileSet";
            WorldTileTextureLoader.NameLookUp[TileDefinitions.Type.Grass] = "Ground_Fire/FireLavaTile";
        }

        public void SetSceneTexturesToAir()
        {
            WorldTileTextureLoader.NameLookUp[TileDefinitions.Type.Water] = "Ground_Air/AirSkyTileSet";
            WorldTileTextureLoader.NameLookUp[TileDefinitions.Type.Dirt] = "Ground_Air/AirGroundTileSet";
            WorldTileTextureLoader.NameLookUp[TileDefinitions.Type.Grass] = "Ground_Air/AirPlate";
        }
    }
}
