﻿using ComputergrafikSpiel.Model.Character.NPC.Interfaces;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputergrafikSpiel.Test.Model.Character.NPC
{
    [TestClass]
    public class EnemyTest
    {
        private IPlayer player = null;
        private IColliderManager ColliderManager { get; set; } = new ColliderManager(32);

        private Vector2 Position = new Vector2(200, 200);

        private ICollection<INonPlayerCharacter> EnemysList { get; set; } = new List<INonPlayerCharacter>();

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void AssertThatEnemyTakingDamageThrowsArgumentNotPositiveGreaterZeroException(int damage)
        {
            ComputergrafikSpiel.Model.Character.NPC.Enemy enemy = new ComputergrafikSpiel.Model.Character.NPC.Enemy(10, "Fungus", 25, 1, 4, this.player, this.ColliderManager, this.EnemysList, this.Position);
            Assert.ThrowsException<ComputergrafikSpiel.View.Exceptions.ArgumentNotPositiveIntegerGreaterZeroException>(() => enemy.TakingDamage(damage));
        }

        [DataTestMethod]
        [DataRow(1, 0)]
        [DataRow(4, 3)]
        public void AssertThatLessDefenseThanDamageMakesDamage(int damage, int defense)
        {
            ComputergrafikSpiel.Model.Character.NPC.Enemy enemy = new ComputergrafikSpiel.Model.Character.NPC.Enemy(10, "Fungus", 25, defense, 4, this.player, this.ColliderManager, this.EnemysList, this.Position);
            int Health = enemy.CurrentHealth;
            enemy.TakingDamage(damage);
            Assert.AreNotEqual(Health, enemy.CurrentHealth);
        }

        [DataTestMethod]
        [DataRow(1, 5)]
        [DataRow(2, 3)]
        public void AssertThatMoreDefenseThanDamageMakesNoDamage(int damage, int defense)
        {
            ComputergrafikSpiel.Model.Character.NPC.Enemy enemy = new ComputergrafikSpiel.Model.Character.NPC.Enemy(10, "Fungus", 25, defense, 4, this.player, this.ColliderManager, this.EnemysList, this.Position);
            int Health = enemy.CurrentHealth;
            enemy.TakingDamage(damage);
            Assert.AreEqual(Health, enemy.CurrentHealth);
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(4)]
        public void AssertThatMulitplierIncreaseEnemyStats (int multiplier)
        {
            ComputergrafikSpiel.Model.Character.NPC.Enemy enemy = new ComputergrafikSpiel.Model.Character.NPC.Enemy(10, "Fungus", 25, 3, 4, this.player, this.ColliderManager, this.EnemysList, this.Position);
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
    }
}