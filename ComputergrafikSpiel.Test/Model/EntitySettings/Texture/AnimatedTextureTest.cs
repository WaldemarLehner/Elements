using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.EntitySettings.Texture.ConstructorHelpers;
using ComputergrafikSpiel.Model.EntitySettings.Texture.ConstructorHelpers.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using ComputergrafikSpiel.Test.Model.EntitySettings.Texture.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;


namespace ComputergrafikSpiel.Test.Model.EntitySettings.Texture
{
    [TestClass]
    public class AnimatedTextureTest
    {
        [TestMethod]
        public void AssertThatUsingWrongInputThrowsException()
        {
            ITextureContructor textureConstructor = new MockTextureConstructorNoFilepath
            {
                Width = 32,
                Height = 32,
            };
            ITileTextureContructor tileConstructor = new TileTextureConstructor(4, 4);
            List<Tuple<int, IAnimation>> animationCollection = new List<Tuple<int, IAnimation>>
            {
                new Tuple<int, IAnimation>(0, new Animation(0, 5, 3f)),
                new Tuple<int, IAnimation>(1, new Animation(5, 1, 1f))
            };
            Assert.ThrowsException<ArgumentNullException>(() => new AnimatedTexture<int>(null, tileConstructor, animationCollection));
            Assert.ThrowsException<ArgumentNullException>(() => new AnimatedTexture<int>(textureConstructor, null, animationCollection));
            Assert.ThrowsException<ArgumentNullException>(() => new AnimatedTexture<int>(textureConstructor, tileConstructor, null));

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new AnimatedTexture<int>(textureConstructor, tileConstructor, new List<Tuple<int, IAnimation>>()));

            List<Tuple<string, IAnimation>> faultyCollection1 = new List<Tuple<string, IAnimation>>
            {
                new Tuple<string, IAnimation>("",null)
            };
            List<Tuple<string, IAnimation>> faultyCollection2 = new List<Tuple<string, IAnimation>>
            {
                new Tuple<string, IAnimation>(null, new Animation(0,5,3f)),
            };
            Assert.ThrowsException<ArgumentNullException>(() => new AnimatedTexture<string>(textureConstructor, tileConstructor, faultyCollection1));
            Assert.ThrowsException<ArgumentNullException>(() => new AnimatedTexture<string>(textureConstructor, tileConstructor, faultyCollection2));
        }

        [TestMethod]
        public void AssertThatUsingCorrectInputDoesNotThrowException()
        {
            ITextureContructor textureConstructor = new MockTextureConstructorNoFilepath
            {
                Width = 32,
                Height = 32,
            };
            ITileTextureContructor tileConstructor = new TileTextureConstructor(4, 4);
            List<Tuple<int, IAnimation>> animationCollection = new List<Tuple<int, IAnimation>>
            {
                new Tuple<int, IAnimation>(0, new Animation(0, 5, 3f)),
                new Tuple<int, IAnimation>(1, new Animation(5, 1, 1f))
            };
            var tex = new AnimatedTexture<int>(textureConstructor, tileConstructor, animationCollection);

            Assert.AreEqual(animationCollection.Count, tex.MappedAnimations.Count);
            Assert.AreEqual(tileConstructor.XRows, tex.XRows);
            Assert.AreEqual(tileConstructor.YRows, tex.YRows);
            Assert.AreEqual(textureConstructor.Height, tex.Height);
            Assert.AreEqual(textureConstructor.Width, tex.Width);
            Assert.AreEqual(textureConstructor.FilePath, tex.FilePath);
            
        }

        [TestMethod]
        public void AssertThatPointerGetsUpdatedCorrectly()
        {
            ITextureContructor textureConstructor = new MockTextureConstructorNoFilepath
            {
                Width = 32,
                Height = 32,
            };
            ITileTextureContructor tileConstructor = new TileTextureConstructor(4, 4);
            List<Tuple<int, IAnimation>> animationCollection = new List<Tuple<int, IAnimation>>
            {
                new Tuple<int, IAnimation>(0, new Animation(0, 5, 3f)),
                new Tuple<int, IAnimation>(1, new Animation(5, 1, 1f))
            };
            var tex = new AnimatedTexture<int>(textureConstructor, tileConstructor, animationCollection);
            tex.QueueAnimation(0); //Queue Animation with index 0
            Assert.AreEqual(0, tex.CurrentAnimationPlayTime);
            tex.Update(.1f);     //Update texture, this should enqueue the animation
            Assert.AreEqual(0, tex.GetQueueLength());
            Assert.AreEqual(.1f, tex.CurrentAnimationPlayTime);
            var currAnim = tex.CurrentAnimation;
            Assert.AreEqual(animationCollection[0].Item2.FirstFrameIndex, currAnim.FirstFrameIndex);
            Assert.AreEqual(new Tuple<int, int>(0, 0), tex.Pointer); // First frame
            tex.Update(1f / currAnim.FramesPerSecond); // Update by one frame
            Assert.AreEqual(.1f+(1f/currAnim.FramesPerSecond), tex.CurrentAnimationPlayTime);
            Assert.AreEqual(new Tuple<int, int>(1, 0), tex.Pointer);
            //Queue the next animation
            tex.QueueAnimation(1);
            tex.Update(4.2f / 3f);
            Assert.AreEqual(animationCollection[1].Item2.FirstFrameIndex, tex.CurrentAnimation.FirstFrameIndex);
            Assert.AreEqual(0, tex.AnimationQueue.Count);
        }

        [TestMethod]
        public void AssertThatTextureCoordinatesGetCalculatedCorrectly()
        {
            ITextureContructor textureConstructor = new MockTextureConstructorNoFilepath
            {
                Width = 32,
                Height = 32,
            };
            ITileTextureContructor tileConstructor = new TileTextureConstructor(4, 4);
            List<Tuple<int, IAnimation>> animationCollection = new List<Tuple<int, IAnimation>>
            {
                new Tuple<int, IAnimation>(0, new Animation(0, 5, 3f)),
                new Tuple<int, IAnimation>(1, new Animation(5, 1, 1f))
            };
            var tex = new AnimatedTexture<int>(textureConstructor, tileConstructor, animationCollection);

            tex.PlayAnimation(0);
            var coords = tex.TextureCoordinates;
            Assert.AreEqual(0, coords.Item1.X);
            Assert.AreEqual(1, coords.Item1.Y);

            Assert.AreEqual(.25, coords.Item2.X);
            Assert.AreEqual(1, coords.Item2.Y);

            Assert.AreEqual(.25, coords.Item3.X);
            Assert.AreEqual(.75, coords.Item3.Y);

            Assert.AreEqual(0, coords.Item4.X);
            Assert.AreEqual(.75, coords.Item4.Y);


        }

    }
}
