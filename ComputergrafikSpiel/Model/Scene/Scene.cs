using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using ComputergrafikSpiel.Model.Character;
using ComputergrafikSpiel.Model.Character.NPC;
using ComputergrafikSpiel.Model.Character.NPC.Interfaces;
using ComputergrafikSpiel.Model.Character.Player;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.Character.Weapon;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.Entity;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.Interfaces;
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

        public Scene(IWorldScene worldScene, Scene top = null, Scene bottom = null, Scene left = null, Scene right = null, Texture background = null)
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

            var tex = background ?? new TextureLoader().LoadTexture("Wall/Wall_single");
            this.Background = new BackgroundRenderable(tex, Vector2.One * worldScene.SceneDefinition.TileSize / 2f, Vector2.One * worldScene.SceneDefinition.TileSize / 2, OpenTK.Graphics.OpenGL.TextureWrapMode.Repeat);

            this.ColliderManager = new ColliderManager(this.World.SceneDefinition.TileSize);

            foreach (var tile in from t in this.World.WorldTilesEnumerable where t is IWorldTileCollidable select t)
            {
                this.ColliderManager.AddWorldTileCollidable(tile.GridPosition.x, tile.GridPosition.y, tile as IWorldTileCollidable);
            }
        }

        public static event EventHandler ChangeScene;

        public static Scene Current { get; private set; } = null;

        public static IPlayer Player { get; private set; } = null;

        public IRenderableBackground Background { get; private set; }

        public IColliderManager ColliderManager { get; }

        public IEnumerable<IEntity> Entities => this.EntitiesList;

        public IScene TopScene { get; }

        public IScene RightScene { get; }

        public IScene LeftScene { get; }

        public IScene BottomScene { get; }

        public IWorldScene World { get; }

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

        public static bool CreatePlayer(IPlayer player)
        {
            if (Scene.Player == null)
            {
                Scene.Player = player ?? throw new ArgumentNullException(nameof(player));
                Scene.Current.ColliderManager.AddEntityCollidable(player);
                Scene.Player.Equip(new Weapon(3, 1, 4, 15, 5));
                return true;
            }

            return false;
        }

        public void SpawnEntity(IEntity entity)
        {
            this.EntitiesList.Add(entity ?? throw new ArgumentNullException(nameof(entity)));
            if (entity is ICollidable)
            {
                this.ColliderManager.AddEntityCollidable(entity as ICollidable);
            }
        }

        public void RemoveEntity(IEntity entity)
        {
            if (entity is Enemy)
            {
                this.NpcList.Remove((INonPlayerCharacter)entity ?? throw new ArgumentNullException(nameof(entity)));
            }

            this.EntitiesList.Remove(entity);
            if (entity is ICollidable)
            {
                this.ColliderManager.RemoveEntityCollidable(entity as ICollidable);
            }
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

            // TODO: Throws null ChangeScene.Invoke(this, null);
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
        }

        public void CreateNPC(INonPlayerCharacter npc)
        {
            this.NpcList.Add(npc ?? throw new ArgumentNullException(nameof(npc)));
            if (npc is ICollidable)
            {
                this.ColliderManager.AddEntityCollidable(npc as ICollidable);
            }
        }

        private void Initialize()
        {
            this.initialized = true;

            // TODO:
        }
    }
}
