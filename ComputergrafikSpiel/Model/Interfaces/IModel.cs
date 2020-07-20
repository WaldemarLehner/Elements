using System.Collections.Generic;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.Overlay;

namespace ComputergrafikSpiel.Model.Interfaces
{
    public interface IModel
    {
        (float top, float bottom, float left, float right) CurrentSceneBounds { get; }

        IEnumerable<IGUIElement[]> UiRenderables { get; }

        IEnumerable<IRenderable> Renderables { get; }

        IInputState InputState { get; }

        int Level { get; }

        void Update(float dTime);

        void UpdateInput(IInputState input);
    }
}
