using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model.EntitySettings.Texture.ConstructorHelpers.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture
{
    internal class AnimatedTexture<T> : IAnimatedMappedTexture<T>
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

        public Tuple<int, int> Pointer => (this.CurrentAnimation == null) ? null : this.GetPointer();

        public int Width { get; private set; }

        public int Height { get; private set; }

        public string FilePath { get; private set; }

        /// <summary>
        /// Gets the Texture Coordinates for the current frame to be drawn. Order is: TL, TR, BR, BL.
        /// </summary>
        public (Vector2 TL, Vector2 TR, Vector2 BR, Vector2 BL) TextureCoordinates => this.GetCurrentTextureCoordinates();

        /// <summary>
        /// Gets or sets the current Animation's playtime. If 0, no animation is played and the current frame is frozen.
        /// If Queue has values, these will immediately be drawn.
        /// </summary>
        private float AnimationDuration { get; set; } = 0f;

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
            if (!this.Animations.ContainsKey(animation))
            {
                throw new ArgumentException(nameof(animation), "The given animation cannot be found.");
            }

            this.AnimationQueue.Clear();
            this.CurrentAnimationPlayTime = 0;
            this.CurrentAnimation = this.Animations[animation];
            this.AnimationDuration = this.CurrentAnimation.FrameCount / this.CurrentAnimation.FramesPerSecond;
            this.Update(0);
        }

        public int GetQueueLength() => this.AnimationQueue.Count;

        public void Update(float dTime)
        {
            if (dTime < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(dTime), "dTime needs to be a positive float or Zero");
            }

            if (this.AnimationDuration == 0)
            {
                // This is a Fixed frame. If the queue is empty, return, else enqueue the new animation.
                if (this.GetQueueLength() == 0)
                {
                    return;
                }

                this.CurrentAnimation = this.AnimationQueue.Dequeue();
                this.CurrentAnimationPlayTime = 0;
                this.AnimationDuration = this.CurrentAnimation.FrameCount / this.CurrentAnimation.FramesPerSecond;
            }

            this.CurrentAnimationPlayTime += dTime;
            if (this.CurrentAnimationPlayTime > this.AnimationDuration)
            {
                // The Animation's duration is exceeded: Time to stop this animation and queue the next
                this.CurrentAnimationPlayTime -= this.AnimationDuration;
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
        }

        private (Vector2 TL, Vector2 TR, Vector2 BR, Vector2 BL) GetCurrentTextureCoordinates()
        {
            var tile = this.Pointer;

            // Tuple: TopLeft, TopRight, BottomRight, BottomLeft coordinated range from 0 to 1.
            float left = tile.Item1 / this.XRows;
            float right = left + (1 / (float)this.XRows);

            float bottom = (this.YRows - tile.Item2 - 1) / (float)this.YRows;
            float top = bottom + (1 / (float)this.YRows);

            Vector2 tl = new Vector2(left, top);
            Vector2 tr = new Vector2(right, top);
            Vector2 bl = new Vector2(left, bottom);
            Vector2 br = new Vector2(right, bottom);

            return (tl, tr, br, bl);
        }

        private int GetCurrentTileIndex()
        {
            if (this.CurrentAnimation == null)
            {
                return -1;
            }

            return this.CurrentAnimation.GetCurrentFrameIndex(this.CurrentAnimationPlayTime);
        }

        private Tuple<int, int> GetPointer()
        {
            var currentIndex = this.GetCurrentTileIndex();
            if (currentIndex == -1)
            {
                return null;
            }

            int x = currentIndex % this.YRows;
            int y = currentIndex / this.YRows;
            return new Tuple<int, int>(x, y);
        }
    }
}
