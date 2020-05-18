using ComputergrafikSpiel.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using System;
using System.Collections.Generic;

namespace ComputergrafikSpiel.Test.Model
{
    [TestClass]
    public class PlayerTest
    {
        [TestMethod]
        public void PlayerControlMovementTest()
        {
            Player player = new Player();
            Vector2 position = player.Position;
            List<PlayerEnum.PlayerActions> movement = new List<PlayerEnum.PlayerActions>();
            movement.Add(PlayerEnum.PlayerActions.MoveDown);
            movement.Add(PlayerEnum.PlayerActions.MoveRight);
            player.PlayerControl(movement);
            Assert.AreNotEqual(position, player.Position);
            movement.Clear();
            position = player.Position;
            movement.Add(PlayerEnum.PlayerActions.MoveUp);
            movement.Add(PlayerEnum.PlayerActions.MoveLeft);
            player.PlayerControl(movement);
            Assert.AreNotEqual(position, player.Position);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void AssertThatPlayerTakingDamageThrowsArgumentNotPositiveGreaterZeroException(int damage)
        {
            Player player = new Player();
            Assert.ThrowsException<ComputergrafikSpiel.View.Exceptions.ArgumentNotPositiveIntegerGreaterZeroException>(() => player.TakingDamage(damage));
        }

        [TestMethod]
        public void AssertThatPlayerIncreaseStatsThrowsArgumentPositiveGreaterZeroException()
        {
            Player player = new Player();
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
            Player player = new Player();
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
            Player player = new Player();
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
            Player player = new Player();
            int Defense = player.Defense;
            int MaxHealth = player.MaxHealth;
            int MovementSpeed = player.MovementSpeed;
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