using ComputergrafikSpiel.Model;
using ComputergrafikSpiel.Test.Model.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenTK;

namespace ComputergrafikSpiel.Test.Model
{
    [TestClass]
    public class CircleOffsetColliderTest
    {
        [TestMethod]
        public void AssertThatMinimalDistanceToOtherCircleOffsetColliderIsCalculatedCorrectly()
        {
            var collidable1 = MockCircleCollidable.CreateCollidableWithCollider(Vector2.Zero, 5);
            var collidable2 = MockCircleCollidable.CreateCollidableWithCollider(new Vector2(10, 10), 5);
            //Distance ~ 14,14 > 5+5 = Expect No Collision


            Assert.IsTrue(collidable1.Collider.MinimalDistanceTo(collidable2.Collider) > 0);
            Assert.IsFalse(collidable1.Collider.DidCollideWith(collidable2.Collider));

            var collidable3 = MockCircleCollidable.CreateCollidableWithCollider(new Vector2(10, 0), 5); //Just about Touches Collidable1
            Assert.AreEqual(0, collidable1.Collider.MinimalDistanceTo(collidable3.Collider));
            Assert.IsTrue(collidable1.Collider.DidCollideWith(collidable3.Collider));
        }
    }
}
