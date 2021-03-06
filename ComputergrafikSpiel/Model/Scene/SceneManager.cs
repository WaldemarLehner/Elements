﻿using ComputergrafikSpiel.Model.EntitySettings.Texture;
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
            this.Play.StartSafeMusic();
            this.Model = model;
        }

        public Soundloader Play { get; set; } = new Soundloader();

        public int CurrentDungeon { get; set; } = 0; // Zähler für den aktuellen Dungeon (1-4)

        public int CurrentDungeonRoom { get; set; } = 0; // Zähler für den aktuellen Raum im Dungeon (1-10)

        public int CurrentStageLevel { get; set; } = 0; // Zähler wieviel Räume bereits betreten wurden insgesamt (1-40)

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
            IWorldScene scene;

            // Keine Obstacles & Gewässer bei Bossräumen
            if (this.CurrentStageLevel % 10 == 0)
            {
                this.obstaclePropability = .0f;
                this.noiseScale = .1f;
                scene = new WorldSceneGenerator(this.obstaclePropability, new WorldSceneDefinition(true, true, true, true, 20, 15, this.noiseScale, 32, WorldSceneDefinition.BossRoomMapping, this.elementType)).GenerateWorldScene();
            }
            else
            {
                this.obstaclePropability = .05f;
                this.noiseScale = .1f;
                scene = new WorldSceneGenerator(this.obstaclePropability, new WorldSceneDefinition(true, true, true, true, 20, 15, this.noiseScale, 32, WorldSceneDefinition.DefaultMapping, this.elementType)).GenerateWorldScene();
            }

            var newScene = new Scene(scene);
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

            if (this.CurrentStageLevel == 40)
            {
                this.Model.CreateTriggerZone(false, true, this.elementType);
            }
            else
            {
                this.Model.CreateTriggerZone(false, false, this.elementType);
            }
        }

        public void InitializeFirstScene()
        {
            if (this.Model.Muted)
            {
                this.Play.MuteMusic();
            }

            this.elementType = WorldEnum.Type.Safezone;
            var worldScene = new WorldSceneGenerator(0f, new WorldSceneDefinition(true, true, true, true, 20, 15, 0f, 32, WorldSceneDefinition.DefaultMapping, this.elementType)).GenerateWorldScene();
            var initScene = new Scene(worldScene);
            (this.Model as Model).FirstScene = true;
            initScene.GiveModeltoScene(this.Model);
            initScene.SetAsActive();
            this.Model.CreateTriggerZone(true, false, this.elementType);
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
