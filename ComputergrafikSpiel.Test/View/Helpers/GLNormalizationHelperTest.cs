using Microsoft.VisualStudio.TestTools.UnitTesting;
using ComputergrafikSpiel.View.Helpers;
using System.Diagnostics.CodeAnalysis;
using ComputergrafikSpiel.View.Exceptions;
using OpenTK;
using System;

namespace ComputergrafikSpiel.Test.View.Helpers
{
    
    [TestClass, ExcludeFromCodeCoverage, Obsolete]
    public class GLNormalizationHelperTest
    {
        [TestClass, Ignore("Tests Obsolete Code"), Obsolete]
        public class NormalizeGL_float
        {
            [TestMethod, Ignore("Tests Obsolete Code"), Obsolete]
            public void AssertThatZeroWillReturnMinusOne()
            {
                Assert.AreEqual(-1f, 0f.NormalizeGL(3));
            }

            [TestMethod, Ignore("Tests Obsolete Code"), Obsolete]
            public void AssertThatANegativeBoundWillThrow()
            {
                Assert.ThrowsException<ArgumentNotPositiveIntegerGreaterZeroException>(() => GLNormalizationHelper.NormalizeGL(3, -3));
            }
            [TestMethod, Ignore("Tests Obsolete Code"), Obsolete]
            public void AssertThatZeroBoundWillThrow()
            {
                Assert.ThrowsException<ArgumentNotPositiveIntegerGreaterZeroException>(() => GLNormalizationHelper.NormalizeGL(0, 0));
            }
            [TestMethod, Ignore("Tests Obsolete Code"), Obsolete]
            public void AssertThatAValueHalfOfTheBoundWillReturnZero()
            {
                int bound = 100;
                float value = bound / 2;
                Assert.AreEqual(0, value.NormalizeGL(bound));
            }
        }

        [TestClass, Ignore("Tests Obsolete Code"), Obsolete]
        public class NormalizeGL_Vector2
        {
            [TestMethod, Ignore("Tests Obsolete Code"), Obsolete]
            public void AssertThatZeroWillReturnMinusOne()
            {
                Vector2 input = Vector2.Zero;
                Vector2 normalized = input.NormalizeGL(100, 100);
                Assert.AreEqual(-1, normalized.X);
                Assert.AreEqual(-1, normalized.Y);
            }

            [TestMethod, Ignore("Tests Obsolete Code"), Obsolete]
            public void AssertThatANegativeXBoundWillThrow()
            {
                Assert.ThrowsException<ArgumentNotPositiveIntegerGreaterZeroException>(() => GLNormalizationHelper.NormalizeGL(Vector2.Zero, -3,5));
            }
            [TestMethod, Ignore("Tests Obsolete Code"), Obsolete]
            public void AssertThatANegativeYBoundWillThrow()
            {
                Assert.ThrowsException<ArgumentNotPositiveIntegerGreaterZeroException>(() => GLNormalizationHelper.NormalizeGL(Vector2.Zero, 5, -5));
            }
            [TestMethod, Ignore("Tests Obsolete Code"), Obsolete]
            public void AssertThatZeroXBoundWillThrow()
            {
                Assert.ThrowsException<ArgumentNotPositiveIntegerGreaterZeroException>(() => GLNormalizationHelper.NormalizeGL(Vector2.Zero,0, 5));
            }
            [TestMethod, Ignore("Tests Obsolete Code"), Obsolete]
            public void AssertThatZeroYBoundWillThrow()
            {
                Assert.ThrowsException<ArgumentNotPositiveIntegerGreaterZeroException>(() => GLNormalizationHelper.NormalizeGL(Vector2.Zero,2, 0));
            }
            [TestMethod, Ignore("Tests Obsolete Code"), Obsolete]
            public void AssertThatAValueHalfOfTheBoundWillReturnZero()
            {
                int bound = 100;
                Vector2 value = new Vector2(bound / 2,bound/2);
                Vector2 returnedValue = value.NormalizeGL(bound, bound);
                Assert.AreEqual(0, returnedValue.X);
                Assert.AreEqual(0, returnedValue.Y);
            }
        }



    }
}
