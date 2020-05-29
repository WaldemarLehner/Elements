using ComputergrafikSpiel.Model.Character.Player;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using System.Collections.Generic;

namespace ComputergrafikSpiel.Test.Model.Character.Player
{
    [TestClass]
    public class PlayerTest
    {
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
            List<PlayerEnum.Stats> stats = new List<PlayerEnum.Stats>();
            stats.Add(PlayerEnum.Stats.Defense);
            stats.Add(PlayerEnum.Stats.MaxHealth);
            stats.Add(PlayerEnum.Stats.MovementSpeed);
            Assert.ThrowsException < ComputergrafikSpiel.View.Exceptions.ArgumentNotPositiveIntegerGreaterZeroException>(() => player.IncreasePlayerStats(-1, stats));
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
            int Defense = player.Defense;
            int MaxHealth = player.MaxHealth;
            float MovementSpeed = player.MovementSpeed;
            List<PlayerEnum.Stats> stats = new List<PlayerEnum.Stats>();
            stats.Add(PlayerEnum.Stats.Defense);
            stats.Add(PlayerEnum.Stats.MaxHealth);
            stats.Add(PlayerEnum.Stats.MovementSpeed);
            player.IncreasePlayerStats(incNumber, stats);
            Assert.AreNotEqual(Defense, player.Defense);
            Assert.AreNotEqual(MaxHealth, player.MaxHealth);
            Assert.AreNotEqual(MovementSpeed, player.MovementSpeed);
        }
    }
}