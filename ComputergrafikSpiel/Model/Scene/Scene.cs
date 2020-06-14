﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Windows.Forms;
using ComputergrafikSpiel.Model.Character.NPC.Interfaces;
using ComputergrafikSpiel.Model.Character.Player;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.Entity;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
using ComputergrafikSpiel.Model.World.Interfaces;

namespace ComputergrafikSpiel.Model.Scene
{
    public class Scene : IScene
    {
        private bool initialized = false;
        private bool active = false;

        public Scene(IWorldScene worldScene, Scene top = null, Scene bottom = null, Scene left = null, Scene right = null)
        {
            this.World = worldScene ?? throw new ArgumentNullException(nameof(worldScene));
            this.TopScene = top;
            this.LeftScene = left;
            this.RightScene = right;
            this.BottomScene = bottom;

            if (Scene.Current == null)
            {
                this.active = true;
                this.Initialize();
                Scene.Current = this;
            }

            this.ColliderManager = new ColliderManager(this.World.SceneDefinition.TileSize);

            foreach (var tile in from t in this.World.WorldTilesEnumerable where t is IWorldTileCollidable select t)
            {
                this.ColliderManager.AddWorldTileCollidable(tile.GridPosition.x, tile.GridPosition.y, tile as IWorldTileCollidable);
            }
        }

        public void SpawnEntity(IEntity entity)
        {
            this.EntitiesList.Add(entity);
        }

        public void RemoveEntity(IEntity entity)
        {
            this.EntitiesList.Remove(entity);
        }

        public static Scene Current { get; private set; } = null;

        public static IPlayer Player { get; private set; } = null;

        public IColliderManager ColliderManager { get; }

        private List<IEntity> EntitiesList { get; } = new List<IEntity>();

        public IEnumerable<IEntity> Entities => this.EntitiesList;

        public IScene TopScene { get; }

        public IScene RightScene { get; }

        public IScene LeftScene { get; }

        public IScene BottomScene { get; }

        public IWorldScene World { get; }

        private List<INonPlayerCharacter> NPCList = new List<INonPlayerCharacter>();

        public IEnumerable<INonPlayerCharacter> NPCs => this.NPCList;

        public IEnumerable<Interactable> Interactables => from entity in this.Entities where entity is Interactable select entity as Interactable;

        public static bool CreatePlayer(IPlayer player)
        {
            if (Scene.Player == null)
            {
                Scene.Player = player;
                Scene.Current.ColliderManager.AddEntityCollidable(player);
                return true;
            }

            return false;
        }

        public void SetAsActive()
        {
            if (this.active)
            {
                return;
            }

            if (!this.initialized)
            {
                this.Initialize();
                this.ColliderManager.AddEntityCollidable(Scene.Player);
            }

            Scene.Current.Disable();
            Scene.Current = this;
        }

        public void Disable()
        {
            if (Scene.Player != null)
            {
                this.ColliderManager.RemoveEntityCollidable(Scene.Player);
            }

            this.active = false;
        }

        public void Update(float dtime)
        {
            (from renderable in this.Renderables where renderable is IUpdateable select renderable).ToList().ForEach(e => (e as IUpdateable).Update(dtime));

            if (Scene.Player != null)
            {
                Scene.Player.Update(dtime);
            }
            if (true) ;
        }

        public void CreateNPC(INonPlayerCharacter npc)
        {
            this.NPCList.Add(npc);
            if (npc is ICollidable)
            {
                this.ColliderManager.AddEntityCollidable(npc as ICollidable);
            }
        }

        public IEnumerable<IRenderable> Renderables
        {
            get
            {
                List<IRenderable> enumerable = new List<IRenderable>();

                var renderables = new IEnumerable<IRenderable>[]
                {
                    this.World.WorldTilesEnumerable,
                    this.World.Obstacles,
                    this.NPCs,
                };

                foreach (var entry in renderables)
                {
                    enumerable.AddRange(entry);
                }

                enumerable.Add(Scene.Player);
                return enumerable;
            }
        }

        private void Initialize()
        {
            this.initialized = true;
            //TODO: 
        }

    }
}
