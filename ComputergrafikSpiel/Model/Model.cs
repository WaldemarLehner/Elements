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
using OpenTK;

namespace ComputergrafikSpiel.Model
{
    internal class Model : IModel
    {
        // temporary
        private IWeapon weapon;

        internal Model()
        {
            this.RenderablesList = new List<IRenderable>();
            this.Updateables = new List<IUpdateable>();
            this.Interactable = new Dictionary<PlayerEnum.Stats, IEntity>();
            this.ColliderManager = new ColliderManager(32);
            this.EnemysList = new List<INonPlayerCharacter>();
        }

        public IEnumerable<IRenderable> Renderables => this.RenderablesList;

        public (float top, float bottom, float left, float right) CurrentSceneBounds => (500, 0, 0, 800);

        public IEnumerable<IUiRenderable> UiRenderables { get; } = new List<IUiRenderable>();

        private List<IRenderable> RenderablesList { get; } = new List<IRenderable>();

        private List<IUpdateable> Updateables { get; } = new List<IUpdateable>();

        private IPlayer Player { get; set; } = null;

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
            if (this.Updateables != null)
            {
                foreach (var entry in this.Updateables.Reverse<IUpdateable>())
                {
                    entry.Update(dTime);
                }
            }
        }

        public bool CreatePlayerOnce(IInputController controller)
        {
            if (this.Player == null)
            {
                this.weapon = new Weapon(3, 1, 20, 2, this.ColliderManager, 1, this);
                this.Player = new Player(this.Interactable, this.ColliderManager, this.weapon, this.EnemysList, this);
                controller.HookPlayer(this.Player);
                this.Updateables.Add(this.Player);
                this.RenderablesList.Add(this.Player);
                return true;
            }

            return false;
        }

        public bool SpawnHeal(float positionX, float positionY)
        {
            // Heal Interactable
            this.IncInteractables = new CreateInteractable(PlayerEnum.Stats.Heal, positionX, positionY);
            this.Interactable.Add(PlayerEnum.Stats.Heal, this.IncInteractables);
            this.Updateables.Add(this.IncInteractables);
            this.RenderablesList.Add(this.IncInteractables);

            return false;
        }

        public bool SpawnWährung(float positionX, float positionY)
        {
            // Währung Interactable
            this.IncInteractables = new CreateInteractable(PlayerEnum.Stats.Währung, positionX, positionY);
            this.Interactable.Add(PlayerEnum.Stats.Währung, this.IncInteractables);
            this.Updateables.Add(this.IncInteractables);
            this.RenderablesList.Add(this.IncInteractables);

            return false;
        }

        // After each round the player can choose between 4 power-ups -> they spawn by calling this function
        public bool CreateRoundEndInteractables()
        {
            // MaxHealth Interactable
            this.IncInteractables = new CreateInteractable(PlayerEnum.Stats.MaxHealth, 250, 250);
            this.Interactable.Add(PlayerEnum.Stats.MaxHealth, this.IncInteractables);
            this.Updateables.Add(this.IncInteractables);
            this.RenderablesList.Add(this.IncInteractables);

            // Defense Interactable
            this.IncInteractables = new CreateInteractable(PlayerEnum.Stats.Defense, 350, 250);
            this.Interactable.Add(PlayerEnum.Stats.Defense, this.IncInteractables);
            this.Updateables.Add(this.IncInteractables);
            this.RenderablesList.Add(this.IncInteractables);

            // AttackSpeed Interactable
            this.IncInteractables = new CreateInteractable(PlayerEnum.Stats.AttackSpeed, 450, 250);
            this.Interactable.Add(PlayerEnum.Stats.AttackSpeed, this.IncInteractables);
            this.Updateables.Add(this.IncInteractables);
            this.RenderablesList.Add(this.IncInteractables);

            // MovementSpeed Interactable
            this.IncInteractables = new CreateInteractable(PlayerEnum.Stats.MovementSpeed, 550, 250);
            this.Interactable.Add(PlayerEnum.Stats.MovementSpeed, this.IncInteractables);
            this.Updateables.Add(this.IncInteractables);
            this.RenderablesList.Add(this.IncInteractables);

            return false;
        }

        public bool CreateEnemy()
        {
            return false;
            if (this.Enemys == null)
            {
                this.Enemys = new Enemy(10, "Fungus", 20, 1, 2, this.Player, this.ColliderManager, this.EnemysList, new Vector2(300, 200));
                this.Updateables.Add(this.Enemys);
                this.RenderablesList.Add(this.Enemys);
                this.EnemysList.Add(this.Enemys);
                return true;
                this.Enemys = new Enemy(10, "WaterDrop", 35, 0, 2, this.Player, this.ColliderManager, this.EnemysList, new Vector2(300, 400));
                this.Updateables.Add(this.Enemys);
                this.RenderablesList.Add(this.Enemys);
                this.EnemysList.Add(this.Enemys);
                return true;
            }

            return false;
        }

        // Somehow the Object will not be destroyed entirely. It will just dissapear.
        public void DestroyObject(IPlayer player, IEntity entity, INonPlayerCharacter npc)
        {
            if (player != null)
            {
                this.ColliderManager.RemoveEntityCollidable(player.Collider.CollidableParent);
                this.Updateables.Remove(player);
                this.RenderablesList.Remove(player);
                player = null;
            }
            else if (entity != null)
            {
                this.ColliderManager.RemoveEntityCollidable(entity.Collider.CollidableParent);
                this.Updateables.Remove(entity);
                this.RenderablesList.Remove(entity);
                entity = null;
            }
            else if (npc != null)
            {
                this.ColliderManager.RemoveEntityCollidable(entity.Collider.CollidableParent);
                this.Updateables.Remove(npc);
                this.RenderablesList.Remove(npc);
                npc = null;
            }
        }

        public void CreateProjectile(int projectileCreationCount, Vector2 position, Vector2 direction, int bulletTTL, float bulletSize, IColliderManager colliderManager)
        {
            for (int i = 0; i <= projectileCreationCount; i++)
            {
                Projectile projectile = new Projectile(position, direction, bulletTTL, bulletSize, colliderManager, this);
                this.Updateables.Add(projectile);
                this.RenderablesList.Add(projectile);
            }
        }
    }
}