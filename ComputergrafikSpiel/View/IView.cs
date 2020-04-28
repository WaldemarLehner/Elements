namespace ComputergrafikSpiel.View
{
    public interface IView : IRenderer
    {
        IRenderer Renderer { get; }
    }
}
