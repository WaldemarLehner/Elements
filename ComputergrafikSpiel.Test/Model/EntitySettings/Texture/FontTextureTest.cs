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
            List<(char, int)> mappings = new List<(char, int)>
            {
                ('a',0),
                ('b',1),
                ('c',2),
                ('d',3),
                ('e',4),
                ('f',5),
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
            List<(char, int)> mappings = new List<(char, int)>
            {
                ('a',0),
                ('b',1),
                ('c',2),
                ('d',3),
                ('e',4),
                ('f',5),
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
            Assert.AreEqual(0, pointer.x);
            Assert.AreEqual(0, pointer.y);
            fontTexture.UpdatePointer('c');
            pointer = fontTexture.Pointer;
            Assert.AreEqual(2, pointer.x);
            Assert.AreEqual(0, pointer.y);
            fontTexture.UpdatePointer('f');
            pointer = fontTexture.Pointer;
            Assert.AreEqual(1, pointer.x);
            Assert.AreEqual(1, pointer.y);

            Assert.AreEqual((-1,-1),fontTexture.GetTileOfKey('o')); // Unused char
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
            List<(char, int)> mappings = new List<(char, int)>
            {
                ('a',0),
                ('b',1),
                ('c',2),
                ('d',3),
                ('e',4),
                ('f',5),
            };

            IMappedTileFont fontTexture = new FontTexture(textureContructor, tileTextureContructor, mappings);

            var (x, y) = fontTexture.GetTileOfKey('a');
            fontTexture.UpdatePointer('a');
            Assert.AreEqual(x, fontTexture.Pointer.x);
            Assert.AreEqual(y, fontTexture.Pointer.y);

            Assert.AreEqual(0, x);
            Assert.AreEqual(0, y);

            /// a is stored in the most top left part of the picture. Coordinates
            /// range from 0 to 1, having the bottom left corner as their origin.
            /// in this test we have a tiled 4x4 texture. That way the following Coordinates are expected:
            /// (0,  1)     ; (.25,     1)
            /// (0,  .75)   ; (.25,     .75)
            var texCoords = fontTexture.TextureCoordinates;
            // Top Left
            Assert.AreEqual(0, texCoords.TopLeft.X);
            Assert.AreEqual(1, texCoords.TopLeft.Y);
            // Top Right
            Assert.AreEqual(.25, texCoords.TopRight.X);
            Assert.AreEqual(1, texCoords.TopRight.Y);
            //Bottom Right
            Assert.AreEqual(.25, texCoords.BottomRight.X);
            Assert.AreEqual(.75, texCoords.BottomRight.Y);

            //Bottom Left
            Assert.AreEqual(0, texCoords.BottomLeft.X);
            Assert.AreEqual(.75, texCoords.BottomLeft.Y);
            

        }
    }
}
