using System.Collections.Generic;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.View.Interfaces;
using ComputergrafikSpiel.View.Renderer;
using ComputergrafikSpiel.View.Renderer.Interfaces;

namespace ComputergrafikSpiel.View
{
    internal class View : IView
    {
        internal View(IReadOnlyCollection<IRenderable> renderables)
        {
            this.OpenTKRenderer = new OpenTKRenderer(renderables);
        }

        public IRenderer Renderer { get => this.OpenTKRenderer; }

        internal OpenTKRenderer OpenTKRenderer { get; private set; }

        public void Render() => this.Renderer.Render();

        public void Resize(int screenWidth, int screenHeight) => this.Renderer.Resize(screenWidth, screenHeight);
    }
}
