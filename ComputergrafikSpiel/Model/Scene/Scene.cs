﻿using System;
using System.Collections.Generic;
using System.Linq;
using ComputergrafikSpiel.Model.Character.NPC;
using ComputergrafikSpiel.Model.Character.NPC.Interfaces;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.Character.Weapon;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.Entity;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.Interfaces;
using ComputergrafikSpiel.Model.Triggers;
using ComputergrafikSpiel.Model.Triggers.Interfaces;
using ComputergrafikSpiel.Model.World;
using ComputergrafikSpiel.Model.World.Interfaces;
using OpenTK;
using OpenTK.Graphics;

namespace ComputergrafikSpiel.Model.Scene
{
    public class Scene : IScene
    {
        private bool initialized = false;
        private bool active = false;
        private bool lockInc = false;

        public Scene(IWorldScene worldScene, Scene top = null, Scene bottom = null, Scene left = null, Scene right = null, Texture background = null)
        {
            this.World = worldScene ?? throw new ArgumentNullException(nameof(worldScene));
            this.TopScene = top;
            this.LeftScene = left;
            this.RightScene = right;
            this.BottomScene = bottom;

            if (Scene.Current == null)
            {
                this.Initialize();
                Scene.Current = this;
            }

            var tex = background ?? new TextureLoader().LoadTexture("Wall/Wall_single");
            this.Background = new BackgroundRenderable(tex, Vector2.One * worldScene.SceneDefinition.TileSize / 2f, Vector2.One * worldScene.SceneDefinition.TileSize / 2, OpenTK.Graphics.OpenGL.TextureWrapMode.Repeat);

            this.ColliderManager = new ColliderManager(this.World.SceneDefinition.TileSize);

            foreach (var tile in from t in this.World.WorldTilesEnumerable where t is IWorldTileCollidable select t)
            {
                this.ColliderManager.AddWorldTileCollidable(tile.GridPosition.x, tile.GridPosition.y, tile as IWorldTileCollidable);
            }
        }

        public static event EventHandler ChangeScene;

        public static Scene Current { get; set; } = null;

        public static IPlayer Player { get; private set; } = null;

        public IModel Model { get; private set; }

        public IRenderableBackground Background { get; private set; }

        public IColliderManager ColliderManager { get; }

        public IEnumerable<IEntity> Entities => this.EntitiesList;

        public IEnumerable<ITrigger> Trigger => this.TriggerList;

        public IScene TopScene { get; }

        public IScene RightScene { get; }

        public IScene LeftScene { get; }

        public IScene BottomScene { get; }

        public IWorldScene World { get; }

        public bool LockPlayerAttack => (this.Model as Model).FirstScene;

        public IEnumerable<IRenderable> Renderables
        {
            get
            {
                List<IRenderable> enumerable = new List<IRenderable>();
                enumerable.Add(this.Background);
                var renderables = new IEnumerable<IRenderable>[]
                {
                    this.World.WorldTilesEnumerable,
                    this.World.Obstacles,
                };

                foreach (var entry in renderables)
                {
                    enumerable.AddRange(entry);
                }

                enumerable.AddRange(this.NPCs);
                enumerable.AddRange(this.Entities);
                enumerable.AddRange(this.Trigger);
                if (Scene.Player != null)
                {
                    enumerable.Add(Scene.Player);
                }

                return enumerable;
            }
        }

        /// <summary>
        /// Gets A list of debug data that can be drawn. This will be cleared on each draw.
        /// </summary>
        public List<(Color4 color, Vector2[] verts)> IndependentDebugData { get; } = new List<(Color4 color, Vector2[] verts)>();

        public IEnumerable<INonPlayerCharacter> NPCs => this.NpcList;

        public IEnumerable<Interactable> Interactables => from entity in this.Entities where entity is Interactable select entity as Interactable;

        private List<INonPlayerCharacter> NpcList { get; } = new List<INonPlayerCharacter>();

        private List<IEntity> EntitiesList { get; } = new List<IEntity>();

        private List<ITrigger> TriggerList { get; } = new List<ITrigger>();

        public static bool CreatePlayer(IPlayer player)
        {
            if (Scene.Player == null)
            {
                Scene.Player = player ?? throw new ArgumentNullException(nameof(player));
                Scene.Player.Equip(new Weapon(3, 1, 4, 12, 5));
                return true;
            }

            return false;
        }

        public void GiveModeltoScene(IModel model)
        {
            this.Model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public void SpawnTrigger(ITrigger trigger)
        {
            this.TriggerList.Add(trigger ?? throw new ArgumentNullException(nameof(trigger)));

            if (trigger is ICollidable)
            {
                this.ColliderManager.AddTriggerCollidable((int)trigger.Position.X, (int)trigger.Position.Y, trigger);
            }
        }

        public void RemoveTrigger(ITrigger trigger)
        {
            this.TriggerList.Remove(trigger ?? throw new ArgumentNullException(nameof(trigger)));

            if (trigger is ICollidable)
            {
                this.ColliderManager.RemoveTriggerCollidable((int)trigger.Position.X, (int)trigger.Position.Y);
            }
        }

        public void SpawnObject(IEntity entity)
        {
            if (entity is Enemy)
            {
                this.NpcList.Add((INonPlayerCharacter)entity ?? throw new ArgumentNullException(nameof(entity)));
            }
            else
            {
                this.EntitiesList.Add(entity ?? throw new ArgumentNullException(nameof(entity)));
            }

            if (entity is ICollidable)
            {
                this.ColliderManager.AddEntityCollidable(entity as ICollidable);
            }
        }

        public void RemoveObject(IEntity entity)
        {
            if (entity is Enemy)
            {
                this.NpcList.Remove((INonPlayerCharacter)entity ?? throw new ArgumentNullException(nameof(entity)));
            }
            else if (entity is Trigger)
            {
                this.TriggerList.Remove((ITrigger)entity ?? throw new ArgumentNullException(nameof(entity)));
            }
            else
            {
                this.EntitiesList.Remove(entity ?? throw new ArgumentNullException(nameof(entity)));
            }

            if (entity is ICollidable)
            {
                this.ColliderManager.RemoveEntityCollidable(entity as ICollidable);
            }
        }

        public void SetAsActive()
        {
            if (this.active)
            {
                Console.WriteLine("Scene already active");
                return;
            }

            if (this.initialized)
            {
                this.Initialize();
                Scene.Current.ColliderManager.AddEntityCollidable(Scene.Player);
            }

            Scene.Current = this;
            this.active = true;
        }

        public void Disable()
        {
            if (Scene.Player != null)
            {
                Scene.Current.ColliderManager.RemoveEntityCollidable(Scene.Player);
            }

            this.active = false;
            Scene.Current = null;
        }

        public void Update(float dtime)
        {
            (from renderable in this.Renderables where renderable is IUpdateable select renderable).ToList().ForEach(e => (e as IUpdateable).Update(dtime));

            if (Scene.Player != null)
            {
                Scene.Player.Update(dtime);
            }

            // Spawn Interactable when all enemies are dead
            if (this.NpcList.Count == 0 && this.lockInc)
            {
                if ((this.Model as Model).FirstScene)
                {
                    (this.Model as Model).CreateTriggerZone();
                }
                else
                {
                    (this.Model as Model).CreateRoundEndInteractables();
                    (this.Model as Model).CreateTriggerZone();
                }

                this.lockInc = false;
            }
        }

        public void OnChangeScene()
        {
            foreach (var trigger in this.TriggerList.ToList())
            {
                this.RemoveTrigger(trigger);
            }

            foreach (var interactable in from i in this.EntitiesList.ToList() where i is Interactable select i as Interactable)
            {
                this.RemoveObject(interactable);
            }

            (this.Model as Model).SceneManager.LoadNewScene();
        }

        public void SpawningEnemies()
        {
            (this.Model as Model).CreateRandomEnemy(2, 5);
        }

        private void Initialize()
        {
            if (this.initialized == true)
            {
                return;
            }

            this.lockInc = true;
            this.initialized = true;
        }
    }
}
