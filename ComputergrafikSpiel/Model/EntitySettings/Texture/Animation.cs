using System;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using ComputergrafikSpiel.View.Exceptions;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture
{
    internal class Animation : IAnimation
    {
        internal Animation(int firstFrame, int frameCount, float framesPerSecond)
        {
            if (firstFrame <= 0)
            {
                throw new ArgumentNotPositiveIntegerGreaterZeroException(nameof(firstFrame));
            }

            if (frameCount <= 0)
            {
                throw new ArgumentNotPositiveIntegerGreaterZeroException(nameof(frameCount));
            }

            if (framesPerSecond <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(framesPerSecond), "FPS needs to be a positive value != 0");
            }

            this.FrameCount = frameCount;
            this.FirstFrameIndex = firstFrame;
            this.FramesPerSecond = framesPerSecond;
        }

        public int FirstFrameIndex { get; private set; }

        public int FrameCount { get; private set; }

        public float FramesPerSecond { get; private set; }
    }
}
