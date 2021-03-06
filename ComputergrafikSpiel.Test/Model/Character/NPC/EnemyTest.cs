﻿using ComputergrafikSpiel.Model.Scene;
using ComputergrafikSpiel.Model.World;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;

namespace ComputergrafikSpiel.Test.Model.Character.NPC
{
    [TestClass]
    public class EnemyTest
    {
        private static float obstacleProbability = .05f; // Spawn der Obstacles Anzahl
        private Vector2 Position = new Vector2(200, 200);

        private static void CreateNewScene()
        {
            Scene scene = new Scene(new WorldSceneGenerator(obstacleProbability, new WorldSceneDefinition(false, false, false, false, 10, 10, .2f, 10, new (int weight, TileDefinitions.Type type)[] { (4, TileDefinitions.Type.Dirt), (6, TileDefinitions.Type.Grass), (4, TileDefinitions.Type.Water) }, WorldEnum.Type.Water)).GenerateWorldScene(), null);
            scene.SetAsActive();
        }


        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void AssertThatEnemyTakingDamageThrowsArgumentNotPositiveGreaterZeroException(int damage)
        {
            ComputergrafikSpiel.Model.Character.NPC.Enemy enemy = new ComputergrafikSpiel.Model.Character.NPC.TankEnemy(this.Position, "Fungus", WorldEnum.Type.Water);
            Assert.ThrowsException<ComputergrafikSpiel.View.Exceptions.ArgumentNotPositiveIntegerGreaterZeroException>(() => enemy.TakingDamage(damage));
        }

        [DataTestMethod]
        [DataRow(5, 2)]
        [DataRow(4, 3)]
        public void AssertThatLessDefenseThanDamageMakesDamage(int damage, int defense)
        {
            CreateNewScene();
            ComputergrafikSpiel.Model.Character.NPC.Enemy enemy = new ComputergrafikSpiel.Model.Character.NPC.TankEnemy(this.Position, "Fungus", WorldEnum.Type.Water);
            int Health = enemy.CurrentHealth;
            enemy.TakingDamage(damage);
            Assert.AreNotEqual(Health, enemy.CurrentHealth);
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(4)]
        public void AssertThatMulitplierIncreaseEnemyStats(int multiplier)
        {
            ComputergrafikSpiel.Model.Character.NPC.Enemy enemy = new ComputergrafikSpiel.Model.Character.NPC.TankEnemy(this.Position, "Fungus", WorldEnum.Type.Water);
            int MaxHealth = enemy.MaxHealth;
            float MovementSpeed = enemy.MovementSpeed;
            int AttackDamage = enemy.AttackDamage;
            enemy.IncreaseDifficulty(multiplier);
            Assert.AreNotEqual(MaxHealth, enemy.MaxHealth);
            Assert.AreNotEqual(MovementSpeed, enemy.MovementSpeed);
            Assert.AreNotEqual(AttackDamage, enemy.AttackDamage);
        }

        [TestMethod]
        public void AssertThatEnemyTextureIsFlippedCorrectlyDependingPlayerLocation()
        {
            ComputergrafikSpiel.Model.Character.NPC.Enemy enemy = new ComputergrafikSpiel.Model.Character.NPC.TankEnemy(this.Position, "Fungus", WorldEnum.Type.Water);
            Vector2 playerLocation = new Vector2(300, 0);
            enemy.LookAt(playerLocation);
            Assert.AreEqual(enemy.Scale.X, -20);

            // make sure it is not flipped again
            enemy.LookAt(playerLocation);
            Assert.AreEqual(enemy.Scale.X, -20);

            // make sure it is flipped back
            playerLocation = new Vector2(100, 0);
            enemy.LookAt(playerLocation);
            Assert.AreEqual(enemy.Scale.X, 20);
        }
    }
}
