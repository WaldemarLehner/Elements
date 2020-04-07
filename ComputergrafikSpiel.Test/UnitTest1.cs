using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComputergrafikSpiel.Test
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.IsTrue(true);
        }

        [DataTestMethod]
        [DataRow(2,2,4)]
        [DataRow(0,-1,-1)]
        public void Add(int x, int y, int result)
        {
            Assert.AreEqual(x + y, result);
        }
    }
}
