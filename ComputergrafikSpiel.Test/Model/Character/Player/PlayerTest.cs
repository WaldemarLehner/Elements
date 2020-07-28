using ComputergrafikSpiel.Model.Character.NPC.Interfaces;
using ComputergrafikSpiel.Model.Character.Player;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
using ComputergrafikSpiel.Model.Scene;
using ComputergrafikSpiel.Model.World;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace ComputergrafikSpiel.Test.Model.Character.Player
{
    [TestClass]
    public class PlayerTest
    {
        private ICollection<INonPlayerCharacter> EnemysList { get; set; } = new List<INonPlayerCharacter>();
        private IColliderManager ColliderManager { get; set; } = new ColliderManager(32);
        private IModel model { get; set; } = null;

        //private Scene scene = new Scene(new WorldSceneGenerator(new WorldSceneDefinition(false, false, false, false, 20, 15, .1f, 32, WorldSceneDefinition.DefaultMapping)).GenerateWorldScene());


        private Dictionary<PlayerEnum.Stats, IEntity> Interactable { get; set; } = new Dictionary<PlayerEnum.Stats, IEntity>();

        private static void CreateNewScene()
        {
            Scene scene = new Scene(new WorldSceneGenerator(new WorldSceneDefinition(false, false, false, false, 10, 10, .2f, 10, new (int weight, TileDefinitions.Type type)[] { (4, TileDefinitions.Type.Dirt), (6, TileDefinitions.Type.Grass), (4, TileDefinitions.Type.Water) })).GenerateWorldScene(), null);
            scene.SetAsActive();
        }
        /*
        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void AssertThatPlayerTakingDamageThrowsArgumentNotPositiveGreaterZeroException(int damage)
        {
            ComputergrafikSpiel.Model.Character.Player.Player player = new ComputergrafikSpiel.Model.Character.Player.Player();
            Assert.ThrowsException<ComputergrafikSpiel.View.Exceptions.ArgumentNotPositiveIntegerGreaterZeroException>(() => player.TakingDamage(damage));
        }

        [TestMethod]
        public void AssertThatPlayerIncreaseStatsThrowsArgumentPositiveGreaterZeroException()
        {
            ComputergrafikSpiel.Model.Character.Player.Player player = new ComputergrafikSpiel.Model.Character.Player.Player();
            Assert.ThrowsException<ComputergrafikSpiel.View.Exceptions.ArgumentNotPositiveIntegerGreaterZeroException>(() => player.IncreasePlayerStats(-1, PlayerEnum.Stats.AttackSpeed));
        }

        [DataTestMethod]
        [DataRow(1, 0)]
        [DataRow(4, 3)]
        public void AssertThatLessDefenseThanDamageMakesDamage(int damage, int defense)
        {
            ComputergrafikSpiel.Model.Character.Player.Player player = new ComputergrafikSpiel.Model.Character.Player.Player();
            int Health = player.CurrentHealth;
            player.Defense = defense;
            player.TakingDamage(damage);
            Assert.AreNotEqual(Health, player.CurrentHealth);
        }

        [DataTestMethod]
        [DataRow(1, 1)]
        [DataRow(3, 6)]
        public void AssertThatMoreDefenseThanDamageMakesNoDamage(int damage, int defense)
        {
            ComputergrafikSpiel.Model.Character.Player.Player player = new ComputergrafikSpiel.Model.Character.Player.Player();
            int Health = player.CurrentHealth;
            player.Defense = defense;
            player.TakingDamage(damage);
            Assert.AreEqual(Health, player.CurrentHealth);
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        public void AssertThatIncreasingPlayerStatsIncreaseRightStats(int incNumber)
        {
            ComputergrafikSpiel.Model.Character.Player.Player player = new ComputergrafikSpiel.Model.Character.Player.Player();
            int MaxHealth = player.MaxHealth;
            int Currenthealth = player.CurrentHealth;
            int Defense = player.Defense;
            float AttackSpeed = player.AttackSpeed;
            float MovementSpeed = player.MovementSpeed;
            int Währung = player.Money;

            player.IncreasePlayerStats(incNumber, PlayerEnum.Stats.MaxHealth);
            Assert.AreNotEqual(MaxHealth, player.MaxHealth);
            player.IncreasePlayerStats(incNumber, PlayerEnum.Stats.Heal);
            Assert.AreNotEqual(Currenthealth, player.CurrentHealth);
            player.IncreasePlayerStats(incNumber, PlayerEnum.Stats.Defense);
            Assert.AreNotEqual(Defense, player.Defense);
            player.IncreasePlayerStats(incNumber, PlayerEnum.Stats.AttackSpeed);
            Assert.AreNotEqual(AttackSpeed, player.AttackSpeed);
            player.IncreasePlayerStats(incNumber, PlayerEnum.Stats.MovementSpeed);
            Assert.AreNotEqual(MovementSpeed, player.MovementSpeed);
            player.IncreasePlayerStats(incNumber, PlayerEnum.Stats.Money);
            Assert.AreNotEqual(Währung, player.Money);
        }
        */
        [TestMethod]
        public void AssertThatPlayerTextureIsFlippedCorrectlyDependingOnMouseLocation()
        {
            ComputergrafikSpiel.Model.Character.Player.Player player = new ComputergrafikSpiel.Model.Character.Player.Player();
            player.Position = new Vector2(5, 5);
            Vector2 mousePosition = new Vector2(0, 0);
            player.LookAt(mousePosition);
            Assert.AreEqual(player.Scale.X, -24);

            // make sure it is not flipped again
            player.LookAt(mousePosition);
            Assert.AreEqual(player.Scale.X, -24);

            // make sure it is flipped back
            mousePosition = new Vector2(10, 0);
            player.LookAt(mousePosition);
            Assert.AreEqual(player.Scale.X, 24);
        }

       
    }
}