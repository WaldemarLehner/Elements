namespace ComputergrafikSpiel.Texture
{
    internal interface IAnimation
    {
        int FirstFrameIndex { get; }

        int FrameCount { get; }

        float FramesPerSecond { get; }
    }
}
