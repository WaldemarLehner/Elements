using ComputergrafikSpiel.View.Renderer.Interfaces;

namespace ComputergrafikSpiel.View.Interfaces
{
    public interface IView : IRenderer
    {
        IRenderer Renderer { get; }
    }
}
