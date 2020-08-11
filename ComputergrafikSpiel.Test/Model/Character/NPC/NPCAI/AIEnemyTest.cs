using ComputergrafikSpiel.Model.Scene;
using ComputergrafikSpiel.Model.World;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;

namespace ComputergrafikSpiel.Test.Model.Character.NPC.NPCAI
{
    [TestClass]
    public class AIEnemyTest
    {
        private static float obstacleProbability = .05f; // Spawn der Obstacles Anzahl
        private Vector2 Position = new Vector2(60, 60);

        private Vector2 Direction;
        private static void CreateNewScene()
        {
            Scene scene = new Scene(new WorldSceneGenerator(obstacleProbability, new WorldSceneDefinition(false, false, false, false, 10, 10, .2f, 10, new (int weight, TileDefinitions.Type type)[] { (4, TileDefinitions.Type.Dirt), (6, TileDefinitions.Type.Grass), (4, TileDefinitions.Type.Water) }, WorldEnum.Type.Water)).GenerateWorldScene(), null);
            scene.SetAsActive();
        }

        [TestMethod]
        public void AssertThatWhenPlayerIsNotInRangeEnemyMovesRandom()
        {
            CreateNewScene();
            ComputergrafikSpiel.Model.Character.Player.Player player = new ComputergrafikSpiel.Model.Character.Player.Player();
            Scene.CreatePlayer(player);
            ComputergrafikSpiel.Model.Character.NPC.Enemy enemy = new ComputergrafikSpiel.Model.Character.NPC.TankEnemy(this.Position, "Fungus", WorldEnum.Type.Water);
            Scene.Current.SpawnObject(enemy);
            ComputergrafikSpiel.Model.Character.NPC.NPCAI.AIEnemy aIEnemy = new ComputergrafikSpiel.Model.Character.NPC.NPCAI.AIEnemy();
            this.Direction = Scene.Player.Position - enemy.Position;
            Direction.Normalize();
            Vector2 DirectionAfter = aIEnemy.EnemyAIMovement(enemy, 0.2f);
            Assert.AreNotEqual(this.Direction, DirectionAfter);
        }
    }
}
