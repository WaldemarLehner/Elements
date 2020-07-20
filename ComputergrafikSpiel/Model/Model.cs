﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Activation;
using ComputergrafikSpiel.Model.Character.NPC;
using ComputergrafikSpiel.Model.Character.Player;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Entity;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
using ComputergrafikSpiel.Model.Overlay;
using ComputergrafikSpiel.Model.Scene;
using ComputergrafikSpiel.Model.Triggers;
using ComputergrafikSpiel.Model.World;
using OpenTK;

namespace ComputergrafikSpiel.Model
{
    public class Model : IModel
    {
        internal Model()
        {
            this.SceneManager = new SceneManager(this);
            this.SceneManager.InitializeFirstScene();
        }

        private int level = 1;

        public bool FirstScene { get; set; }

        public ISceneManager SceneManager { get; set; }

        public IEnumerable<IRenderable> Renderables => Scene.Scene.Current.Renderables;

        public (float top, float bottom, float left, float right) CurrentSceneBounds => Scene.Scene.Current.World.WorldSceneBounds;

        public int Level => this.level;

        public IEnumerable<IGUIElement[]> UiRenderables => Scene.Scene.Current.UIRenderables;

        public IInputState InputState { get; private set; }

        /// <summary>
        /// For the Test, this will draw a Rectangle doing a loop.
        /// </summary>
        /// <param name="dTime">Time between two Update Calls in Seconds.</param>
        public void Update(float dTime)
        {
            Scene.Scene.Current.Update(dTime);

        }

        public void UpdateInput(IInputState inputState)
        {
            this.InputState = inputState;
        }

        public void CreateTriggerZone()
        {
            Vector2[] positions =
            {
                new Vector2(16, 272),
                new Vector2(368, 16),
                new Vector2(368, 528),
                new Vector2(688, 272),
            };
            for (int i = 0; i < positions.Length; i++)
            {
                Scene.Scene.Current.SpawnTrigger(new Trigger(positions[i], ColliderLayer.Layer.Player));
            }

            return;
        }

        public void SpawnInteractable(PlayerEnum.Stats stat, float positionX, float positionY, int incNumber)
        {
            Scene.Scene.Current.SpawnObject(new Interactable(stat, positionX, positionY, incNumber));
        }

        // After each round the player can choose between 4 power-ups -> they spawn by calling this function
        public void OnSceneCompleted(IWorldScene world)
        {
            this.level++;

            var positions = new (int x, int y)[]
            {
                (1, 1),
                (1, world.SceneDefinition.TileCount.y),
                (world.SceneDefinition.TileCount.x, 1),
                (world.SceneDefinition.TileCount.x, world.SceneDefinition.TileCount.y),
            };

            var upgradesToSelect = new (PlayerEnum.Stats, int)[] { (PlayerEnum.Stats.MaxHealth, 1), (PlayerEnum.Stats.Defense, 2), (PlayerEnum.Stats.AttackSpeed, 1), (PlayerEnum.Stats.MovementSpeed, 10) };
            float tileSize = world.SceneDefinition.TileSize;
            for (int i = 0; i < 4; i++)
            {
                var (x, y) = positions[i];
                this.SpawnInteractable(upgradesToSelect[i].Item1, (x + .5f) * tileSize, (y + .5f) * tileSize, upgradesToSelect[i].Item2);
            }
        }

        public void CreateRandomEnemies(int min, int max, IWorldScene world)
        {
            Random random = new Random();
            int count = random.Next(min, max);
            int tileCount = world.SceneDefinition.TileCount.x * world.SceneDefinition.TileCount.y;
            List<(int x, int y)> enemySpawnIndices = new List<(int x, int y)>();
            foreach (int i in Enumerable.Range(0, count))
            {
                var index = FindNextSpawnSlot(random.Next(0, tileCount), world, SpawnMask.Mask.AllowNPC);
                if (index == null)
                {
                    // No more open slots.
                    break;
                }

                enemySpawnIndices.Add(index ?? (0, 0)); // (0, 0) is never reached.
            }

            string[] texture = { "Fungus", "WaterDrop" };
            foreach (var (x, y) in enemySpawnIndices)
            {
                var position = new Vector2(x + .5f, y + .5f) * world.SceneDefinition.TileSize;
                var randomTexture = texture[random.Next(0, texture.Length)];
                if (randomTexture == "Fungus")
                {
                    Scene.Scene.Current.SpawnObject(new Enemy(20, randomTexture, 25, 2, 3, position));
                }
                else if (randomTexture == "WaterDrop")
                {
                    Scene.Scene.Current.SpawnObject(new Enemy(10, randomTexture, 80, 0, 1, position));
                }
            }
        }

        private static (int x, int y)? FindNextSpawnSlot(int index, IWorldScene world, SpawnMask.Mask mask)
        {
            int checkedCount = 0;
            int maxCheck = world.SceneDefinition.TileCount.x * world.SceneDefinition.TileCount.y;
            while (checkedCount < maxCheck)
            {
                index %= maxCheck;
                (int x, int y) = (index % world.SceneDefinition.TileCount.x, index / world.SceneDefinition.TileCount.x);
                if ((world.WorldTiles[x, y].Spawnmask & mask) != 0)
                {
                    // This is a Spawn location. flip the bit to mark that a spawn already occured
                    world.WorldTiles[x, y].Spawnmask ^= mask;
                    return (x, y);
                }

                index++;
                checkedCount++;
            }

            return null;
        }
    }
}