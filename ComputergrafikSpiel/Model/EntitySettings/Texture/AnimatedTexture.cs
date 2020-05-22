using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model.EntitySettings.Texture.ConstructorHelpers.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture
{
    internal class AnimatedTexture<T> : IAnimatedMappedTexture<T>
        where T : Enum // Constraint erlaubt nur Enums für T
    {
        public AnimatedTexture(ITextureContructor textureContructor, ITileTextureContructor tiletextureContructor, ICollection<Tuple<T, IAnimation>> animations)
        {
            _ = textureContructor ?? throw new ArgumentNullException(nameof(textureContructor));
            _ = tiletextureContructor ?? throw new ArgumentNullException(nameof(tiletextureContructor));
            _ = animations ?? throw new ArgumentNullException(nameof(animations));

            if (animations.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(animations), "Passed collection of animations is empty");
            }

            this.Width = textureContructor.Width;
            this.Height = textureContructor.Height;
            this.XRows = tiletextureContructor.XRows;
            this.YRows = tiletextureContructor.YRows;
            this.FilePath = textureContructor.FilePath;
            this.Animations = new Dictionary<T, IAnimation>();

            foreach (var entry in animations)
            {
                if (entry.Item1 == null)
                {
                    throw new ArgumentNullException(nameof(entry.Item1));
                }

                if (entry.Item2 == null)
                {
                    throw new ArgumentNullException(nameof(entry.Item2));
                }

                this.Animations[entry.Item1] = entry.Item2;
            }

            this.AnimationQueue = new Queue<IAnimation>();
        }

        public IDictionary<T, IAnimation> MappedAnimations => this.Animations;

        public Dictionary<T, IAnimation> Animations { get; private set; }

        public IAnimation CurrentAnimation { get; private set; } = null;

        public float CurrentAnimationPlayTime { get; private set; }

        public Queue<IAnimation> AnimationQueue { get; private set; }

        public int XRows { get; private set; }

        public int YRows { get; private set; }

        public Tuple<int, int> Pointer { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public string FilePath { get; private set; }

        public Tuple<Vector2, Vector2, Vector2, Vector2> TextureCoordinates => this.GetCurrentTextureCoordinates();

        private float AnimationDuration { get; set; }

        /// <summary>
        /// Add Animation to queue. It will be played once it's place in the queue comes up.
        /// </summary>
        /// <param name="animation">Key describing the Animation.</param>
        public void QueueAnimation(T animation)
        {
            this.AnimationQueue.Enqueue(this.Animations[animation]);
        }

        /// <summary>
        /// Play this Animation immediately. Does also clear the queue.
        /// </summary>
        /// <param name="animation">Key describing the Animation.</param>
        public void PlayAnimation(T animation)
        {
            this.AnimationQueue.Clear();
            this.CurrentAnimationPlayTime = 0;
            this.CurrentAnimation = this.Animations[animation];
        }

        public int GetQueueLength() => this.AnimationQueue.Count;

        public void Update(float dTime)
        {
            if (dTime <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(dTime), "dTime needs to be a positive float");
            }

            this.CurrentAnimationPlayTime += dTime;
            if (this.CurrentAnimationPlayTime > this.AnimationDuration)
            {
                // The Animation's duration is exceeded: Time to stop this animation and queue the next
                this.CurrentAnimationPlayTime = 0;
                if (this.AnimationQueue.Count > 0)
                {
                    this.CurrentAnimation = this.AnimationQueue.Dequeue();
                    this.AnimationDuration = this.CurrentAnimation.FramesPerSecond * this.CurrentAnimation.FrameCount;
                }
                else
                {
                    this.CurrentAnimation = null;
                    this.AnimationDuration = float.MaxValue;
                }
            }

            this.UpdatePointer();
        }

        private Tuple<Vector2, Vector2, Vector2, Vector2> GetCurrentTextureCoordinates()
        {
            var tile = this.Pointer;

            var tileWidth = this.Width / this.XRows;
            var tileHeight = this.Height / this.YRows;

            // Tuple: TopLeft, TopRight, BottomRight, BottomLeft coordinated range from 0 to 1.
            float bottom = ((this.YRows - tile.Item2 - 1) / this.YRows) * tileHeight; // Flipping Y axis
            float top = bottom + tileHeight;
            float left = tile.Item1 / this.XRows;
            float right = left + tileWidth;

            Vector2 tl = new Vector2(top, left);
            Vector2 tr = new Vector2(top, right);
            Vector2 bl = new Vector2(bottom, left);
            Vector2 br = new Vector2(bottom, right);

            return new Tuple<Vector2, Vector2, Vector2, Vector2>(tl, tr, br, bl);
        }

        private int GetCurrentTileIndex()
        {
            var current = this.CurrentAnimation;
            var framesPlayed = (int)(this.CurrentAnimationPlayTime / current.FramesPerSecond); // FramesPlayed is always positive, hence one can safely cast to int without Math.Floor()
            return current.FirstFrameIndex + framesPlayed;
        }

        private void UpdatePointer()
        {
            var currentIndex = this.GetCurrentTileIndex();
            int yRow = currentIndex / this.YRows;
            int xRow = currentIndex % this.XRows;
            this.Pointer = new Tuple<int, int>(xRow, yRow);
        }
    }
}
