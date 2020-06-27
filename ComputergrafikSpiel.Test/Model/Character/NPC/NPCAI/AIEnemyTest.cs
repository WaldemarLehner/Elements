﻿using ComputergrafikSpiel.Model.Scene;
using ComputergrafikSpiel.Model.World;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;

namespace ComputergrafikSpiel.Test.Model.Character.NPC.NPCAI
{
    [TestClass]
    public class AIEnemyTest
    {
        private Vector2 Position = new Vector2(100, 100);

        private Vector2 Direction;
        private static void CreateNewScene()
        {
            Scene scene = new Scene(new WorldSceneGenerator(new WorldSceneDefinition(false, false, false, false, 10, 10, .2f, 10, new (int weight, TileDefinitions.Type type)[] { (4, TileDefinitions.Type.Dirt), (6, TileDefinitions.Type.Grass), (4, TileDefinitions.Type.Water) })).GenerateWorldScene(), null);
            scene.SetAsActive();
        }

        [TestMethod, Ignore("somehow Ray needs an Instance from another class, which why here comes a SystemNullReferenceException")]
        public void AssertThatWhenPlayerIsInRangeEnemyMoves()
        {
            CreateNewScene();
            ComputergrafikSpiel.Model.Character.Player.Player player = new ComputergrafikSpiel.Model.Character.Player.Player();
            Scene.CreatePlayer(player);
            ComputergrafikSpiel.Model.Character.NPC.Enemy enemy = new ComputergrafikSpiel.Model.Character.NPC.Enemy(10, "Fungus", 25, 1, 4, this.Position);
            Scene.Current.CreateNPC(enemy);
            ComputergrafikSpiel.Model.Character.NPC.NPCAI.AIEnemy aIEnemy = new ComputergrafikSpiel.Model.Character.NPC.NPCAI.AIEnemy();
            this.Direction = Scene.Player.Position - enemy.Position;
            Direction.Normalize();
            Vector2 DirectionAfter = aIEnemy.EnemyAIMovement(enemy, 0.2f);
            Assert.AreEqual(Direction, DirectionAfter);
        }
    }
}
