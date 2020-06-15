﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Text;
using System.Linq;
using ComputergrafikSpiel.Model.Character.NPC;
using ComputergrafikSpiel.Model.Character.NPC.Interfaces;
using ComputergrafikSpiel.Model.Character.Player;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.Character.Weapon;
using ComputergrafikSpiel.Model.Character.Weapon.Interfaces;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.Entity;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
using ComputergrafikSpiel.Model.Scene;
using ComputergrafikSpiel.Model.World;
using ComputergrafikSpiel.Model.Triggers;
using ComputergrafikSpiel.Model.Triggers.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model
{
    internal class Model : IModel
    {
        // temporary
        private IWeapon weapon;

        internal Model()
        {
            /*
            this.RenderablesList = new List<IRenderable>();
            this.Updateables = new List<IUpdateable>();
            this.Interactable = new Dictionary<PlayerEnum.Stats, IEntity>();
            this.ColliderManager = new ColliderManager(32);
            this.EnemysList = new List<INonPlayerCharacter>();
            */

            var worldScene = new WorldSceneGenerator(new WorldSceneDefinition(false, false, false, false, 20, 15, .1f, 32, WorldSceneDefinition.DefaultMapping)).GenerateWorldScene();
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
                this.weapon = new Weapon(3, 1, 4, 20, this.ColliderManager, 1, this);
                this.Player = new Player(this.Interactable, this.ColliderManager, this.weapon, this.EnemysList, this);
                controller.HookPlayer(this.Player);
                this.Updateables.Add(this.Player);
                this.RenderablesList.Add(this.Player);
                return true;
            }

            return false;
        }

        public void CreateTriggerZone()
        {
            this.TriggerZone = new Trigger(this.ColliderManager, new Vector2(30, 250));
            this.Updateables.Add(this.TriggerZone);
            this.RenderablesList.Add(this.TriggerZone);
            return;
        }

        public bool SpawnHeal(float positionX, float positionY)
        {
            // Heal Interactable
            this.IncInteractables = new CreateInteractable(PlayerEnum.Stats.Heal, positionX, positionY);
            this.Interactable.Add(PlayerEnum.Stats.Heal, this.IncInteractables);
            this.Updateables.Add(this.IncInteractables);
            this.RenderablesList.Add(this.IncInteractables);

            return Scene.Scene.CreatePlayer(player);
        }

        public void SpawnInteractable(PlayerEnum.Stats stat, float positionX, float positionY, int incNumber)
        {
            // Heal Interactable
            Scene.Scene.Current.SpawnEntity(new Interactable(stat, positionX, positionY, incNumber));
        }

        // After each round the player can choose between 4 power-ups -> they spawn by calling this function
        public void CreateRoundEndInteractables()
        {
            // MaxHealth Interactable
            this.SpawnInteractable(PlayerEnum.Stats.MaxHealth, 250, 250, 1);

            // Defense Interactable
            this.SpawnInteractable(PlayerEnum.Stats.Defense, 350, 250, 2);

            // AttackSpeed Interactable
            this.SpawnInteractable(PlayerEnum.Stats.AttackSpeed, 450, 250, 3);

            // MovementSpeed Interactable
            this.SpawnInteractable(PlayerEnum.Stats.MovementSpeed, 550, 250, 10);
        }

        public void CreateEnemy()
        {
                Scene.Scene.Current.CreateNPC(new Enemy(10, "Fungus", 20, 1, 2, new Vector2(300, 200)));
        }

        public void CreateProjectile(int attackDamage, int projectileCreationCount, Vector2 position, Vector2 direction, float bulletTTL, float bulletSize, IColliderManager colliderManager, ICollection<INonPlayerCharacter> enemyList)
        {
            for (int i = 0; i < projectileCreationCount; i++)
            {
                Projectile projectile = new Projectile(attackDamage, position, direction, bulletTTL, bulletSize, colliderManager, this, enemyList);
                this.Updateables.Add(projectile);
                this.RenderablesList.Add(projectile);
            }
        }
    }
}