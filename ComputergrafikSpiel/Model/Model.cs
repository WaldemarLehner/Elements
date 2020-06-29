﻿using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model.Character.NPC;
using ComputergrafikSpiel.Model.Character.Player;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Entity;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
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
            this.SceneManager = new Scene.SceneManager(this);
            this.SceneManager.InitializeFirstScene();
        }

        public ISceneManager SceneManager { get; }

        public IEnumerable<IRenderable> Renderables => Scene.Scene.Current.Renderables;

        public (float top, float bottom, float left, float right) CurrentSceneBounds => Scene.Scene.Current.World.WorldSceneBounds;

        public IEnumerable<IUiRenderable> UiRenderables { get; } = new List<IUiRenderable>();

        private Dictionary<PlayerEnum.Stats, IEntity> Interactable { get; set; } = null;

        /// <summary>
        /// For the Test, this will draw a Rectangle doing a loop.
        /// </summary>
        /// <param name="dTime">Time between two Update Calls in Seconds.</param>
        public void Update(float dTime)
        {
            Scene.Scene.Current.Update(dTime);
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

        public void CreateNewScene()
        {

        }

        public void SpawnHeal(float positionX, float positionY)
        {
            // Heal Interactable
            var inter = new Interactable(PlayerEnum.Stats.Heal, positionX, positionY, 1);
            this.Interactable.Add(PlayerEnum.Stats.Heal, inter);
            Scene.Scene.Current.SpawnObject(inter);
        }

        public void SpawnInteractable(PlayerEnum.Stats stat, float positionX, float positionY, int incNumber)
        {
            Scene.Scene.Current.SpawnObject(new Interactable(stat, positionX, positionY, incNumber));
        }

        // After each round the player can choose between 4 power-ups -> they spawn by calling this function
        public void CreateRoundEndInteractables()
        {
            // MaxHealth
            this.SpawnInteractable(PlayerEnum.Stats.MaxHealth, 250, 250, 1);

            // Defense Interactable
            this.SpawnInteractable(PlayerEnum.Stats.Defense, 350, 250, 2);

            // AttackSpeed Interactable
            this.SpawnInteractable(PlayerEnum.Stats.AttackSpeed, 450, 250, 3);

            // MovementSpeed Interactable
            this.SpawnInteractable(PlayerEnum.Stats.MovementSpeed, 550, 250, 10);
        }

        public void CreateRandomEnemy(int min, int max)
        {
            Random random = new Random();
            string[] texture = { "Fungus", "WaterDrop" };
            for (int i = random.Next(min, max); i > 0; i--)
            {
                var randomTexture = texture[random.Next(0, texture.Length)];
                if (randomTexture == "Fungus")
                {
                    Scene.Scene.Current.SpawnObject(new Enemy(20, randomTexture, 25, 2, 3, new Vector2(random.Next(50, 500), random.Next(50, 500))));
                }
                else if (randomTexture == "WaterDrop")
                {
                    Scene.Scene.Current.SpawnObject(new Enemy(10, randomTexture, 80, 0, 1, new Vector2(random.Next(50, 500), random.Next(50, 500))));
                }
            }
        }
    }
}