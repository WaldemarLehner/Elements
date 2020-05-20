namespace ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces
{
    internal interface IAnimation
    {
        int FirstFrameIndex { get; }

        int FrameCount { get; }

        float FramesPerSecond { get; }
    }
}
