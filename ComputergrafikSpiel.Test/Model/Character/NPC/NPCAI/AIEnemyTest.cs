using ComputergrafikSpiel.Model.Character.NPC.Interfaces;
using ComputergrafikSpiel.Model.Character.Player;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputergrafikSpiel.Test.Model.Character.NPC.NPCAI
{
    [TestClass]
    public class AIEnemyTest
    {
        private IColliderManager ColliderManager { get; set; } = new ColliderManager(32);

        private ICollection<INonPlayerCharacter> EnemysList { get; set; } = new List<INonPlayerCharacter>();

        private IPlayer player;

        private Vector2 Position = new Vector2(100, 100);

        private IModel model { get; set; } = null;

        private Dictionary<PlayerEnum.Stats, IEntity> Interactable { get; set; } = new Dictionary<PlayerEnum.Stats, IEntity>();

        [TestMethod]
        public void AssertThatWhenPlayerIsInRangeEnemyMoves()
        {
            this.player = new ComputergrafikSpiel.Model.Character.Player.Player(this.Interactable, this.ColliderManager, null, this.EnemysList, this.model);
            ComputergrafikSpiel.Model.Character.NPC.Enemy enemy = new ComputergrafikSpiel.Model.Character.NPC.Enemy(10, "Fungus", 25, 1, 4, this.player, this.ColliderManager, this.EnemysList, this.Position);
            ComputergrafikSpiel.Model.Character.NPC.NPCAI.AIEnemy aIEnemy = new ComputergrafikSpiel.Model.Character.NPC.NPCAI.AIEnemy(this.ColliderManager, this.EnemysList, this.player);
            Vector2 Direction = player.Position - enemy.Position;
            Direction.Normalize();
            Vector2 DirectionAfter = aIEnemy.EnemyAIMovement(enemy);
            Assert.AreEqual(Direction, DirectionAfter);

        }

    }
}
