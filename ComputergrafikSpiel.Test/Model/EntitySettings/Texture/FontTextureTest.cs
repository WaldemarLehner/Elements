using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.EntitySettings.Texture.ConstructorHelpers;
using ComputergrafikSpiel.Model.EntitySettings.Texture.ConstructorHelpers.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ComputergrafikSpiel.Test.Model.EntitySettings.Texture
{
    [TestClass]
    public class FontTextureTest
    {
        [TestMethod]
        public void AssertThatInvalidInputsThrowExceptions()
        {
            ITextureContructor textureContructor = new TestHelper.MockTextureConstructorNoFilepath
            {
                Width = 64,
                Height = 64
            };

            ITileTextureContructor tileTextureContructor = new TileTextureConstructor(4, 4);
            List<Tuple<char, int>> mappings = new List<Tuple<char, int>>
            {
                new Tuple<char, int>('a',0),
                new Tuple<char, int>('b',1),
                new Tuple<char, int>('c',2),
                new Tuple<char, int>('d',3),
                new Tuple<char, int>('e',4),
                new Tuple<char, int>('f',5),
            };


            Assert.ThrowsException<ArgumentNullException>( ()=> new FontTexture(null, tileTextureContructor, mappings));
            Assert.ThrowsException<ArgumentNullException>(() => new FontTexture(textureContructor, null, mappings));
            Assert.ThrowsException<ArgumentNullException>(() => new FontTexture(textureContructor, tileTextureContructor, null));
        }

        [TestMethod]
        public void AssertThatValidInputDoesNotThrowException()
        {
            ITextureContructor textureContructor = new TestHelper.MockTextureConstructorNoFilepath
            {
                Width = 64,
                Height = 64
            };

            ITileTextureContructor tileTextureContructor = new TileTextureConstructor(4, 4);
            List<Tuple<char, int>> mappings = new List<Tuple<char, int>>
            {
                new Tuple<char, int>('a',0),
                new Tuple<char, int>('b',1),
                new Tuple<char, int>('c',2),
                new Tuple<char, int>('d',3),
                new Tuple<char, int>('e',4),
                new Tuple<char, int>('f',5),
            };

            IMappedTileFont fontTexture = new FontTexture(textureContructor, tileTextureContructor, mappings);

            Assert.AreEqual(mappings.Count, fontTexture.MappedPositions.Count);
            Assert.AreEqual(textureContructor.Width, fontTexture.Width);
            Assert.AreEqual(textureContructor.Height, fontTexture.Height);
            Assert.AreEqual(tileTextureContructor.XRows, fontTexture.XRows);
            Assert.AreEqual(tileTextureContructor.YRows, fontTexture.YRows);
            Assert.AreEqual(textureContructor.FilePath, fontTexture.FilePath);

            //Update Pointer to "a" -> internal index is 0
            fontTexture.UpdatePointer('a');
            var pointer = fontTexture.Pointer;
            Assert.AreEqual(0, pointer.Item1);
            Assert.AreEqual(0, pointer.Item2);
            fontTexture.UpdatePointer('c');
            pointer = fontTexture.Pointer;
            Assert.AreEqual(2, pointer.Item1);
            Assert.AreEqual(0, pointer.Item2);
            fontTexture.UpdatePointer('f');
            pointer = fontTexture.Pointer;
            Assert.AreEqual(1, pointer.Item1);
            Assert.AreEqual(1, pointer.Item2);

            Assert.IsNull(fontTexture.GetTileOfKey('o')); // Unused char
        }
        [TestMethod]
        public void AssertThatGettingTextureCoordinatesAreCorrect()
        {
            ITextureContructor textureContructor = new TestHelper.MockTextureConstructorNoFilepath
            {
                Width = 64,
                Height = 64
            };

            ITileTextureContructor tileTextureContructor = new TileTextureConstructor(4, 4);
            List<Tuple<char, int>> mappings = new List<Tuple<char, int>>
            {
                new Tuple<char, int>('a',0),
                new Tuple<char, int>('b',1),
                new Tuple<char, int>('c',2),
                new Tuple<char, int>('d',3),
                new Tuple<char, int>('e',4),
                new Tuple<char, int>('f',5),
            };

            IMappedTileFont fontTexture = new FontTexture(textureContructor, tileTextureContructor, mappings);

            var aPtr = fontTexture.GetTileOfKey('a');
            fontTexture.UpdatePointer('a');
            Assert.AreEqual(aPtr.Item1, fontTexture.Pointer.Item1);
            Assert.AreEqual(aPtr.Item2, fontTexture.Pointer.Item2);

            Assert.AreEqual(0, aPtr.Item1);
            Assert.AreEqual(0, aPtr.Item2);

            /// a is stored in the most top left part of the picture. Coordinates
            /// range from 0 to 1, having the bottom left corner as their origin.
            /// in this test we have a tiled 4x4 texture. That way the following Coordinates are expected:
            /// (0,  1)     ; (.25,     1)
            /// (0,  .75)   ; (.25,     .75)
            var texCoords = fontTexture.TextureCoordinates;
            // Top Left
            Assert.AreEqual(0, texCoords.Item1.X);
            Assert.AreEqual(1, texCoords.Item1.Y);
            // Top Right
            Assert.AreEqual(.25, texCoords.Item2.X);
            Assert.AreEqual(1, texCoords.Item2.Y);
            //Bottom Right
            Assert.AreEqual(.25, texCoords.Item3.X);
            Assert.AreEqual(.75, texCoords.Item3.Y);

            //Bottom Left
            Assert.AreEqual(0, texCoords.Item4.X);
            Assert.AreEqual(.75, texCoords.Item4.Y);
            

        }
    }
}
