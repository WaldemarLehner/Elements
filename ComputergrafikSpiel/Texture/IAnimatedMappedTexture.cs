using System.Collections.Generic;

namespace ComputergrafikSpiel.Texture
{
    internal interface IAnimatedMappedTexture<T> : IAnimation, ITileTexture
    {
        IDictionary<T, IAnimation> MappedAnimations { get; }

        IAnimation CurrentAnimation { get; }

        float CurrentAnimationPlayTime { get; }

        Queue<IAnimation> AnimationQueue { get; }

        void QueueAnimation(T);

        void PlayAnimation(T);

        int GetQueueLength();
    }
}
