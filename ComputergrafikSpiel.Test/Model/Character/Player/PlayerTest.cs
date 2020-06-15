using ComputergrafikSpiel.Model.Character.NPC.Interfaces;
using ComputergrafikSpiel.Model.Character.Player;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using System.Collections.Generic;

namespace ComputergrafikSpiel.Test.Model.Character.Player
{
    [TestClass]
    public class PlayerTest
    {
        private ICollection<INonPlayerCharacter> EnemysList { get; set; } = new List<INonPlayerCharacter>();
        private IColliderManager ColliderManager { get; set; } = new ColliderManager(32);
        private IModel model { get; set; } = null;

        private Dictionary<PlayerEnum.Stats, IEntity> Interactable { get; set; } = new Dictionary<PlayerEnum.Stats, IEntity>();

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
            Assert.ThrowsException < ComputergrafikSpiel.View.Exceptions.ArgumentNotPositiveIntegerGreaterZeroException>(() => player.IncreasePlayerStats(-1, PlayerEnum.Stats.AttackSpeed));
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
            int Währung = player.Währung;

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
            player.IncreasePlayerStats(incNumber, PlayerEnum.Stats.Währung);
            Assert.AreNotEqual(Währung, player.Währung);
        }
    }
}