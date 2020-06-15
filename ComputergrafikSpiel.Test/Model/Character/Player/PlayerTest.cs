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
            ComputergrafikSpiel.Model.Character.Player.Player player = new ComputergrafikSpiel.Model.Character.Player.Player(this.Interactable, this.ColliderManager, null, this.EnemysList, this.model);
            Assert.ThrowsException<ComputergrafikSpiel.View.Exceptions.ArgumentNotPositiveIntegerGreaterZeroException>(() => player.TakingDamage(damage));
        }

        [TestMethod]
        public void AssertThatPlayerIncreaseStatsThrowsArgumentPositiveGreaterZeroException()
        {
            ComputergrafikSpiel.Model.Character.Player.Player player = new ComputergrafikSpiel.Model.Character.Player.Player(this.Interactable, this.ColliderManager, null, this.EnemysList, this.model);
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
            ComputergrafikSpiel.Model.Character.Player.Player player = new ComputergrafikSpiel.Model.Character.Player.Player(this.Interactable, this.ColliderManager, null, this.EnemysList, this.model);
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
            ComputergrafikSpiel.Model.Character.Player.Player player = new ComputergrafikSpiel.Model.Character.Player.Player(this.Interactable, this.ColliderManager, null, this.EnemysList, this.model);
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
            ComputergrafikSpiel.Model.Character.Player.Player player = new ComputergrafikSpiel.Model.Character.Player.Player(this.Interactable, this.ColliderManager, null, this.EnemysList, this.model);
            int MaxHealth = player.MaxHealth;
            int Currenthealth = player.CurrentHealth;
            int Defense = player.Defense;
            float AttackSpeed = player.AttackSpeed;
            float MovementSpeed = player.MovementSpeed;
            int Währung = player.Währung;

            List<PlayerEnum.Stats> stats = new List<PlayerEnum.Stats>();
            stats.Add(PlayerEnum.Stats.MaxHealth);
            stats.Add(PlayerEnum.Stats.Heal);
            stats.Add(PlayerEnum.Stats.Defense);
            stats.Add(PlayerEnum.Stats.AttackSpeed);
            stats.Add(PlayerEnum.Stats.MovementSpeed);
            stats.Add(PlayerEnum.Stats.Währung);

            player.IncreasePlayerStats(incNumber, stats);
            Assert.AreNotEqual(MaxHealth, player.MaxHealth);
            Assert.AreNotEqual(Currenthealth, player.CurrentHealth);
            Assert.AreNotEqual(Defense, player.Defense);
            Assert.AreNotEqual(AttackSpeed, player.AttackSpeed);
            Assert.AreNotEqual(MovementSpeed, player.MovementSpeed);
            Assert.AreNotEqual(Währung, player.Währung);
        }

        /*
        [DataTestMethod]
        [DataRow(0)]
        public void AssertThatCooldownIsSetCorrectlyAndAttackingCorrectlySetsTheCooldown(float initialCountdownTime)
        {
            ComputergrafikSpiel.Model.Character.Weapon.Weapon weapon = new ComputergrafikSpiel.Model.Character.Weapon.Weapon(3, 1, 4, 20, this.ColliderManager, 1, this.model);
            ComputergrafikSpiel.Model.Character.Player.Player player = new ComputergrafikSpiel.Model.Character.Player.Player(this.Interactable, this.ColliderManager, weapon, this.EnemysList, this.model);

            Assert.AreEqual(initialCountdownTime, player.AttackCooldownCurrnent);

            List<PlayerEnum.PlayerActions> testActions = new List<PlayerEnum.PlayerActions>();
            testActions.Add(PlayerEnum.PlayerActions.Attack);
            player.Position = new Vector2(0, 0);
            Vector2 testMouseCoordinates = new Vector2(0, 0);

            player.PlayerControl(testActions, testMouseCoordinates);

            Assert.AreEqual(player.AttackCooldown, player.AttackCooldownCurrnent);
        }
        */
    }
}