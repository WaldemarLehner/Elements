using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.Interfaces;
using ComputergrafikSpiel.Model.Soundtrack;
using ComputergrafikSpiel.Model.World;

namespace ComputergrafikSpiel.Model.Scene
{
    public class SceneManager : ISceneManager
    {
        private float obstaclePropability; // Spawn der Obstacles Anzahl
        private float noiseScale; // Spawn der Anzahl an Gewässer / Boden
        private WorldEnum.Type elementType;

        public SceneManager(IModel model)
        {
            // Szene wird gestartet, ebenso direkt in der Safe-Zone wird das Lied gespielt
            this.Play.StartSafeMusic();
            this.Model = model;
        }

        public Soundloader Play { get; set; } = new Soundloader();

        public int CurrentDungeon { get; set; } = 0; // Zähler für den aktuellen Dungeon (1-4)

        public int CurrentDungeonRoom { get; set; } = 0; // Zähler für den aktuellen Raum im Dungeon (1-10)

        public int CurrentStageLevel { get; set; } = 0; // Zähler wieviel Räume bereits betreten wurden insgesamt (1-40)

        private IWorldScene WorldScene { get; set; }

        private IModel Model { get; }

        public void LoadNewScene()
        {
            this.CurrentStageLevel++;
            this.CurrentDungeonRoom++;

            switch (this.CurrentStageLevel)
            {
                case 1:
                    this.Play.StartDungeon1Music();
                    this.SetSceneTexturesToWater();
                    this.elementType = WorldEnum.Type.Water;
                    this.CurrentDungeon++;
                    break;
                case 11:
                    this.Play.StartDungeon2Music();
                    this.SetSceneTexturesToEarth();
                    this.elementType = WorldEnum.Type.Earth;
                    this.CurrentDungeon++;
                    this.CurrentDungeonRoom = 1;
                    this.Model.Level = 1;
                    break;
                case 21:
                    this.Play.StartDungeon3Music();
                    this.SetSceneTexturesToFire();
                    this.elementType = WorldEnum.Type.Fire;
                    this.CurrentDungeon++;
                    this.CurrentDungeonRoom = 1;
                    this.Model.Level = 1;
                    break;
                case 31:
                    this.Play.StartDungeon4Music();
                    this.SetSceneTexturesToAir();
                    this.elementType = WorldEnum.Type.Air;
                    this.CurrentDungeon++;
                    this.CurrentDungeonRoom = 1;
                    this.Model.Level = 1;
                    break;
                default: break;
            }

            (this.Model as Model).FirstScene = false;
            Scene.Current.Disable();

            // Keine Obstacles & Gewässer bei Bossräumen Überprüfung
            if (this.CurrentStageLevel % 10 == 0)
            {
                this.obstaclePropability = .0f;
                this.noiseScale = .0f;
                this.WorldScene = new WorldSceneGenerator(this.obstaclePropability, new WorldSceneDefinition(true, true, true, true, 20, 15, this.noiseScale, 32, WorldSceneDefinition.ExtraordinaryDungeonMapping)).GenerateWorldScene();
            }
            else
            {
                this.obstaclePropability = .05f;
                this.noiseScale = .1f;
                this.WorldScene = new WorldSceneGenerator(this.obstaclePropability, new WorldSceneDefinition(true, true, true, true, 20, 15, this.noiseScale, 32, WorldSceneDefinition.DefaultMapping)).GenerateWorldScene();
            }

            var newScene = new Scene(this.WorldScene);
            newScene.GiveModeltoScene(this.Model);
            newScene.SetAsActive();

            switch (this.CurrentStageLevel)
            {
                case 10:
                    this.Play.StartDungeon1BossMusic();
                    newScene.SpawningEnemies(newScene.World, this.elementType, true);
                    break;
                case 20:
                    this.Play.StartDungeon2BossMusic();
                    newScene.SpawningEnemies(newScene.World, this.elementType, true);
                    break;
                case 30:
                    this.Play.StartDungeon3BossMusic();
                    newScene.SpawningEnemies(newScene.World, this.elementType, true);
                    break;
                case 40:
                    this.Play.StartDungeon4BossMusic();
                    newScene.SpawningEnemies(newScene.World, this.elementType, true);
                    break;
                default:
                    newScene.SpawningEnemies(newScene.World, this.elementType, false);
                    break;
            }

            this.Model.CreateTriggerZone(false, false);
        }

        public void InitializeFirstScene()
        {
            var worldScene = new WorldSceneGenerator(0f, new WorldSceneDefinition(true, true, true, true, 20, 15, 0f, 32, WorldSceneDefinition.DefaultMapping)).GenerateWorldScene();
            var initScene = new Scene(worldScene);
            (this.Model as Model).FirstScene = true;
            initScene.GiveModeltoScene(this.Model);
            initScene.SetAsActive();
            this.Model.CreateTriggerZone(true, false);
        }

        public void SetSceneTexturesToSafeZone()
        {
            WorldTileTextureLoader.NameLookUp[TileDefinitions.Type.Water] = "Ground_Safezone/WaterTileSet";
            WorldTileTextureLoader.NameLookUp[TileDefinitions.Type.Dirt] = "Ground_Safezone/EarthTileSet";
            WorldTileTextureLoader.NameLookUp[TileDefinitions.Type.Grass] = "Ground_Safezone/Grass";
        }

        public void SetSceneTexturesToWater()
        {
            WorldTileTextureLoader.NameLookUp[TileDefinitions.Type.Water] = "Ground_Water/WaterTileSet";
            WorldTileTextureLoader.NameLookUp[TileDefinitions.Type.Dirt] = "Ground_Water/EarthTileSet";
            WorldTileTextureLoader.NameLookUp[TileDefinitions.Type.Grass] = "Ground_Water/Grass";
        }

        public void SetSceneTexturesToEarth()
        {
            WorldTileTextureLoader.NameLookUp[TileDefinitions.Type.Water] = "Ground_Earth/WaterTileSet";
            WorldTileTextureLoader.NameLookUp[TileDefinitions.Type.Dirt] = "Ground_Earth/EarthTileSet";
            WorldTileTextureLoader.NameLookUp[TileDefinitions.Type.Grass] = "Ground_Earth/Grass";
        }

        public void SetSceneTexturesToFire()
        {
            WorldTileTextureLoader.NameLookUp[TileDefinitions.Type.Water] = "Ground_Fire/WaterTileSet";
            WorldTileTextureLoader.NameLookUp[TileDefinitions.Type.Dirt] = "Ground_Fire/EarthTileSet";
            WorldTileTextureLoader.NameLookUp[TileDefinitions.Type.Grass] = "Ground_Fire/Grass";
        }

        public void SetSceneTexturesToAir()
        {
            WorldTileTextureLoader.NameLookUp[TileDefinitions.Type.Water] = "Ground_Air/WaterTileSet";
            WorldTileTextureLoader.NameLookUp[TileDefinitions.Type.Dirt] = "Ground_Air/EarthTileSet";
            WorldTileTextureLoader.NameLookUp[TileDefinitions.Type.Grass] = "Ground_Air/Grass";
        }
    }
}
