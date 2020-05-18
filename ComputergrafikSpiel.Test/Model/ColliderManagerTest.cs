using ComputergrafikSpiel.Model;
using ComputergrafikSpiel.Test.Model.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;
using System;
using System.Linq;

namespace ComputergrafikSpiel.Test.Model
{
    [TestClass]
    public class ColliderManagerTest
    {
        [DataTestMethod]
        [DataRow(0f)]
        [DataRow(-1f)]
        public void AssertThatIllegalRadiusThrowsArgumentOutOfRangeException(float radius)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => MockCircleCollidable.CreateCollidableWithCollider(Vector2.Zero, radius));
        }
        [TestMethod]
        public void AssertThatPositiveRadiusDoesNotThrowException()
        {
            MockCircleCollidable.CreateCollidableWithCollider(Vector2.Zero, 1f);
        }
        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void AssertThatIllegalTileSizeThrowsException(int size)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ColliderManager(size));
        }

        [TestMethod]
        public void AssertThatLegalTileSizeDoesNotThrowException()
        {
            new ColliderManager(1);
        }

        [TestMethod]
        public void AssertThatAddingAndRemovingCollidersDoesNotThrowException()
        {
            IColliderManager manager = new ColliderManager(1);
            ICollidable collidable1 = MockCircleCollidable.CreateCollidableWithCollider(new Vector2(0, 1), 3);
            ICollidable collidable2 = MockCircleCollidable.CreateCollidableWithCollider(new Vector2(0, 3), 4);
            manager.AddWorldTileCollidable(0, 1, collidable1);
            manager.AddWorldTileCollidable(0, 3, collidable2);

            ICollidable collidable3 = MockCircleCollidable.CreateCollidableWithCollider(new Vector2(0, 10), 5);
            manager.AddEntityCollidable(collidable3);

            Assert.AreEqual(2, manager.CollidableTileDictionary.Count);
            Assert.AreEqual(1, manager.CollidableEntitiesCollection.Count);

            manager.RemoveWorldTileCollidable(0, 1);

            Assert.AreEqual(1, manager.CollidableTileDictionary.Count);

            manager.RemoveEntityCollidable(collidable3);

            Assert.AreEqual(0, manager.CollidableEntitiesCollection.Count);

            manager.AddEntityCollidable(collidable3);

            Assert.AreEqual(1, manager.CollidableEntitiesCollection.Count);

            manager.ClearAll();

            Assert.AreEqual(0, manager.CollidableEntitiesCollection.Count);
            Assert.AreEqual(0, manager.CollidableTileDictionary.Count);

        }

        [TestMethod]
        public void AssertThatCircularReferenceBetweenParentAndChildExist()
        {
            ICollidable collidable = MockCircleCollidable.CreateCollidableWithCollider(Vector2.Zero, 33);
            Assert.IsNotNull(collidable);
            Assert.AreEqual(collidable, collidable.Collider.CollidableParent);
        }

        [TestMethod]
        public void AssertThatGettingAffectedTilesWorksCorrectly()
        {
            IColliderManager manager = new ColliderManager(1);
            AddStaticToManager(manager, 1, 2);
            AddStaticToManager(manager, 3, 1);
            AddStaticToManager(manager, 500, 1);
            AddStaticToManager(manager, 5, 1);
            AddStaticToManager(manager, 9, 5);
            Assert.AreEqual(5, manager.CollidableTileDictionary.Count);
            var response = (manager as ColliderManager).GetAffectedStaticTiles(new Vector2(3, 3), 10);
            Assert.AreEqual(4, response.Count(e=>e != null));
        }

        private static void AddStaticToManager(IColliderManager manager, int x, int y)
        {
            manager.AddWorldTileCollidable(x, y, MockCircleCollidable.CreateCollidableWithCollider(new Vector2(x, y), 1));
        }

        [TestMethod]
        public void AssertThatTouchingCollidersAreDetected()
        {
            IColliderManager manager = new ColliderManager(1);
            ICollidable collidable = MockCircleCollidable.CreateCollidableWithCollider(new Vector2(10, 12), 5);
            ICollidable static1 = MockCircleCollidable.CreateCollidableWithCollider(new Vector2(1, 1), 1);
            ICollidable static2 = MockCircleCollidable.CreateCollidableWithCollider(new Vector2(8, 10), 1);
            ICollidable dynamic1 = MockCircleCollidable.CreateCollidableWithCollider(new Vector2(3.4f, 4), 10);

            manager.AddWorldTileCollidable(1,1,static1);
            manager.AddWorldTileCollidable(8, 10, static2);
            manager.AddEntityCollidable(dynamic1);

            var collisions = manager.GetCollisions(collidable);
            Assert.IsTrue(collisions.Count > 0);
            Assert.AreEqual(2, collisions.Count);

        }
       
    }

   
}
