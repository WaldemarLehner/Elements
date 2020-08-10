using System;
using System.Collections.Generic;
using System.Linq;
using ComputergrafikSpiel.Model.Character.NPC;
using ComputergrafikSpiel.Model.Character.Player;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Entity;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
using ComputergrafikSpiel.Model.Overlay.EndScreen;
using ComputergrafikSpiel.Model.Overlay.UpgradeScreen;
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

        public bool FirstScene { get; set; }

        public ISceneManager SceneManager { get; set; }

        public IEnumerable<IRenderable> Renderables => Scene.Scene.Current.Renderables;

        public (float top, float bottom, float left, float right) CurrentSceneBounds => Scene.Scene.Current.World.WorldSceneBounds;

        public int Level { get; set; } = 1;

        public UpgradeScreen UpgradeScreen { get; private set; }

        public EndScreen EndScreen { get; set; }

        public IInputState InputState { get; private set; }

        /// <summary>
        /// For the Test, this will draw a Rectangle doing a loop.
        /// </summary>
        /// <param name="dTime">Time between two Update Calls in Seconds.</param>
        public void Update(float dTime)
        {
            if (this.UpgradeScreen != null)
            {
                this.UpgradeScreen.Update(dTime);
            }

            if (this.EndScreen != null)
            {
                this.EndScreen.Update(dTime);
            }

            Scene.Scene.Current.Update(dTime);
        }

        public void UpdateInput(IInputState inputState)
        {
            this.InputState = inputState;
        }

        public void CreateTriggerZone(bool firstScene, bool lastScene)
        {
            Vector2[] positions =
            {
                // new Vector2(368, 16),
                // new Vector2(368, 528),
                new Vector2(688, 272), // rightDoor
                new Vector2(16, 272), // left Door
            };
            if (firstScene)
            {
                Scene.Scene.Current.SpawnTrigger(new Trigger(positions[0], ColliderLayer.Layer.Player, false));
            }
            else if (lastScene)
            {
                Scene.Scene.Current.SpawnTrigger(new Trigger(positions[1], ColliderLayer.Layer.Player, true));
            }
            else
            {
                Scene.Scene.Current.SpawnTrigger(new Trigger(positions[1], ColliderLayer.Layer.Player, true));
                Scene.Scene.Current.SpawnTrigger(new Trigger(positions[0], ColliderLayer.Layer.Player, false));
            }

            return;
        }

        public void SpawnInteractable(PlayerEnum.Stats stat, float positionX, float positionY)
        {
            Scene.Scene.Current.SpawnObject(new Interactable(stat, positionX, positionY));
        }

        // After each round the player can choose between 4 power-ups -> they spawn by calling this function
        public void OnSceneCompleted()
        {
            this.Level++;

            var (top, bottom, left, right) = Scene.Scene.Current.World.WorldSceneBounds;
            var topV = new Vector2((left + right) * .5f, top);
            var width = (right - left) * .5f;
            void Callback(PlayerEnum.Stats stat)
            {
                Scene.Scene.Player.SelectOption(stat, (uint)this.Level);
                this.UpgradeScreen = null;
            }

            this.UpgradeScreen = new UpgradeScreen(Scene.Scene.Player.GetOptions((uint)this.Level), 10, topV, width, callback: Callback);
        }

        public void TriggerEndscreenButtons()
        {
            var (top, bottom, left, right) = Scene.Scene.Current.World.WorldSceneBounds;
            var centerV = new Vector2((left + right) * .5f, (top + bottom) * .5f);
            var width = (right - left) * .5f;

            this.EndScreen = new EndScreen(10, centerV, width);
        }

        public List<(int x, int y)> SpawningAreaEnemys(int min, int max, IWorldScene world)
        {
            Random random = new Random();
            int count = random.Next(min, max);
            int tileCount = world.SceneDefinition.TileCount.x * world.SceneDefinition.TileCount.y;
            List<(int x, int y)> enemySpawnIndices = new List<(int x, int y)>();
            foreach (var index in from int i in Enumerable.Range(0, count)
                                  let index = FindNextSpawnSlot(random.Next(0, tileCount), world, SpawnMask.Mask.AllowNPC)
                                  select index)
            {
                if (index == null)
                {
                    // No more open slots.
                    break;
                }

                enemySpawnIndices.Add(index ?? (0, 0)); // (0, 0) is never reached.
            }

            return enemySpawnIndices;
        }

        public void CreateRandomEnemies(int min, int max, IWorldScene world, WorldEnum.Type enemytype, bool boss)
        {
            Random random = new Random();
            EnemyManager enemyManager = new EnemyManager();
            foreach (var (x, y) in this.SpawningAreaEnemys(min, max, world))
            {
                var position = new Vector2(x + .5f, y + .5f) * world.SceneDefinition.TileSize;
                if (boss)
                {
                    enemyManager.BossSpawner(enemytype);
                }
                else
                {
                    enemyManager.EnemySpawner(position, enemytype, random.Next(0, 4));
                }
            }
        }

        private static (int x, int y)? FindNextSpawnSlot(int index, IWorldScene world, SpawnMask.Mask mask)
        {
            int checkedCount = 0;
            int maxCheck = world.SceneDefinition.TileCount.x * world.SceneDefinition.TileCount.y;
            int[] excludeX = { 0, 1, 2, 3, 4, 5 };
            while (checkedCount < maxCheck)
            {
                index %= maxCheck;
                (int x, int y) = (index % world.SceneDefinition.TileCount.x, index / world.SceneDefinition.TileCount.x);

                if (x != excludeX[0] && x != excludeX[1] && x != excludeX[2] && x != excludeX[3] && x != excludeX[4] && x != excludeX[5])
                {
                    if ((world.WorldTiles[x, y].Spawnmask & mask) != 0)
                    {
                        // This is a Spawn location. flip the bit to mark that a spawn already occured
                        world.WorldTiles[x, y].Spawnmask ^= mask;
                        return (x, y);
                    }
                }

                index++;
                checkedCount++;
            }

            return null;
        }
    }
}