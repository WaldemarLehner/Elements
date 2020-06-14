using System;
using System.Collections.Generic;
using System.Linq;
using ComputergrafikSpiel.Model.Character.NPC;
using ComputergrafikSpiel.Model.Character.NPC.Interfaces;
using ComputergrafikSpiel.Model.Character.Player;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.Entity;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
using ComputergrafikSpiel.Model.Scene;
using ComputergrafikSpiel.Model.World;
using OpenTK;

namespace ComputergrafikSpiel.Model
{
    internal class Model : IModel
    {
        internal Model()
        {
            /*
            this.RenderablesList = new List<IRenderable>();
            this.Updateables = new List<IUpdateable>();
            this.Interactable = new Dictionary<PlayerEnum.Stats, IEntity>();
            this.ColliderManager = new ColliderManager(32);
            this.EnemysList = new List<INonPlayerCharacter>();
            */

            var worldScene = new WorldSceneGenerator(new WorldSceneDefinition(false, false, false, false, 20, 15, .3f, 32, WorldSceneDefinition.DefaultMapping)).GenerateWorldScene();
            new Scene.Scene(worldScene);
        }

        public IEnumerable<IRenderable> Renderables => Scene.Scene.Current.Renderables;

        public (float top, float bottom, float left, float right) CurrentSceneBounds => Scene.Scene.Current.World.WorldSceneBounds;

        public IEnumerable<IUiRenderable> UiRenderables { get; } = new List<IUiRenderable>();

        private IEntity IncInteractables { get; set; } = null;

        private INonPlayerCharacter Enemys { get; set; } = null;

        private ICollection<INonPlayerCharacter> EnemysList { get; set; } = null;

        private Dictionary<PlayerEnum.Stats, IEntity> Interactable { get; set; } = null;

        private IColliderManager ColliderManager { get; set; }

        /// <summary>
        /// For the Test, this will draw a Rectangle doing a loop.
        /// </summary>
        /// <param name="dTime">Time between two Update Calls in Seconds.</param>
        public void Update(float dTime)
        {
            Scene.Scene.Current.Update(dTime);
        }

        public bool CreatePlayerOnce(IInputController controller)
        {
            if (Scene.Scene.Player != null)
            {
                return false;
            }

            var player = new Player(this.Interactable, this);
            controller.HookPlayer(player);

            return Scene.Scene.CreatePlayer(player);
        }

        public void SpawnInteractable(PlayerEnum.Stats stat, float positionX, float positionY)
        {
            // Heal Interactable
            this.IncInteractables = new Interactable(stat, positionX, positionY);
            Scene.Scene.Current.SpawnEntity(this.IncInteractables);
        }

        public void SpawnCurrency(float positionX, float positionY)
        {
            // Währung Interactable
            Scene.Scene.Current.SpawnEntity(new Interactable(PlayerEnum.Stats.Währung, positionX, positionY));
        }

        // After each round the player can choose between 4 power-ups -> they spawn by calling this function
        public void CreateRoundEndInteractables()
        {
            // MaxHealth Interactable
            this.SpawnInteractable(PlayerEnum.Stats.MaxHealth, 250, 250);

            // Defense Interactable
            this.SpawnInteractable(PlayerEnum.Stats.Defense, 350, 250);

            // AttackSpeed Interactable
            this.SpawnInteractable(PlayerEnum.Stats.AttackSpeed, 450, 250);

            // MovementSpeed Interactable
            this.SpawnInteractable(PlayerEnum.Stats.MovementSpeed, 550, 250);
        }

        public void CreateEnemy()
        {
                Scene.Scene.Current.CreateNPC(new Enemy(10, "Fungus", 20, 1, 2, new Vector2(300, 200)));
        }
    }
}
