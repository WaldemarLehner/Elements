using System.Collections.Generic;

namespace ComputergrafikSpiel.View
{
    public class View : IView
    {
        public View(IReadOnlyCollection<Model.IRenderable> renderables)
        {
            this.OpenTKRenderer = new OpenTKRenderer(renderables);
        }

        public IRenderer Renderer { get => this.OpenTKRenderer; }

        public OpenTKRenderer OpenTKRenderer { get; private set; }

        public void Render() => this.Renderer.Render();

        public void Resize(int screenWidth, int screenHeight) => this.Renderer.Resize(screenWidth, screenHeight);
    }
}
