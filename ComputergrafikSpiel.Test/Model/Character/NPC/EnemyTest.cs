
using ComputergrafikSpiel.Model.Character.NPC.Interfaces;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.Scene;
using ComputergrafikSpiel.Model.World;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using System.Collections.Generic;

namespace ComputergrafikSpiel.Test.Model.Character.NPC
{
    [TestClass]
    public class EnemyTest
    {
        private IColliderManager ColliderManager { get; set; } = new ColliderManager(32);

        private Vector2 Position = new Vector2(200, 200);

        private ICollection<INonPlayerCharacter> EnemysList { get; set; } = new List<INonPlayerCharacter>();

        private static void CreateNewScene()
        {
            Scene scene = new Scene(new WorldSceneGenerator(new WorldSceneDefinition(false, false, false, false, 10, 10, .2f, 10, new (int weight, TileDefinitions.Type type)[] { (4, TileDefinitions.Type.Dirt), (6, TileDefinitions.Type.Grass), (4, TileDefinitions.Type.Water) })).GenerateWorldScene());
            scene.SetAsActive();
        }


        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void AssertThatEnemyTakingDamageThrowsArgumentNotPositiveGreaterZeroException(int damage)
        {
            ComputergrafikSpiel.Model.Character.NPC.Enemy enemy = new ComputergrafikSpiel.Model.Character.NPC.Enemy(10, "Fungus", 25, 1, 4, this.Position);
            Assert.ThrowsException<ComputergrafikSpiel.View.Exceptions.ArgumentNotPositiveIntegerGreaterZeroException>(() => enemy.TakingDamage(damage));
        }

        [DataTestMethod]
        [DataRow(1, 0)]
        [DataRow(4, 3)]
        public void AssertThatLessDefenseThanDamageMakesDamage(int damage, int defense)
        {
            CreateNewScene();
            ComputergrafikSpiel.Model.Character.NPC.Enemy enemy = new ComputergrafikSpiel.Model.Character.NPC.Enemy(10, "Fungus", 25, defense, 4, this.Position);
            int Health = enemy.CurrentHealth;
            enemy.TakingDamage(damage);
            Assert.AreNotEqual(Health, enemy.CurrentHealth);
        }

        [DataTestMethod]
        [DataRow(1, 5)]
        [DataRow(2, 3)]
        public void AssertThatMoreDefenseThanDamageMakesNoDamage(int damage, int defense)
        {
            CreateNewScene();
            ComputergrafikSpiel.Model.Character.NPC.Enemy enemy = new ComputergrafikSpiel.Model.Character.NPC.Enemy(10, "Fungus", 25, defense, 4, this.Position);
            int Health = enemy.CurrentHealth;
            enemy.TakingDamage(damage);
            Assert.AreEqual(Health, enemy.CurrentHealth);
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(4)]
        public void AssertThatMulitplierIncreaseEnemyStats (int multiplier)
        {
            ComputergrafikSpiel.Model.Character.NPC.Enemy enemy = new ComputergrafikSpiel.Model.Character.NPC.Enemy(10, "Fungus", 25, 3, 4,  this.Position);
            int MaxHealth = enemy.MaxHealth;
            int Defense = enemy.Defense;
            float MovementSpeed = enemy.MovementSpeed;
            int AttackDamage = enemy.AttackDamage;
            enemy.IncreaseDifficulty(multiplier);
            Assert.AreNotEqual(MaxHealth, enemy.MaxHealth);
            Assert.AreNotEqual(Defense, enemy.Defense);
            Assert.AreNotEqual(MovementSpeed, enemy.MovementSpeed);
            Assert.AreNotEqual(AttackDamage, enemy.AttackDamage);
        }

        [TestMethod]
        public void AssertThatEnemyTextureIsFlippedCorrectlyDependingPlayerLocation()
        {
            ComputergrafikSpiel.Model.Character.NPC.Enemy enemy = new ComputergrafikSpiel.Model.Character.NPC.Enemy(10, "Fungus", 25, 3, 4, this.Position);
            Vector2 playerLocation = new Vector2(300, 0);
            enemy.LookAt(playerLocation);
            Assert.AreEqual(enemy.Scale.X, -16);

            // make sure it is not flipped again
            enemy.LookAt(playerLocation);
            Assert.AreEqual(enemy.Scale.X, -16);

            // make sure it is flipped back
            playerLocation = new Vector2(100, 0);
            enemy.LookAt(playerLocation);
            Assert.AreEqual(enemy.Scale.X, 16);
        }
    }
}
