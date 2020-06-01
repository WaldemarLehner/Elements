using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputergrafikSpiel.Test.Model.TestHelper
{
    public class MockModel : IModel
    {
        public (float top, float bottom, float left, float right) CurrentSceneBounds => (100, 0, 0, 100);

        public List<IUiRenderable> UiRenderableList { get; set; } = new List<IUiRenderable>();

        public IReadOnlyCollection<IUiRenderable> UiRenderables => UiRenderableList;

        public List<IRenderable> RenderableList { get; set; } = new List<IRenderable>();

        public IReadOnlyCollection<IRenderable> Renderables => RenderableList;

        public void Update(float dTime)
        {
            throw new NotImplementedException();
        }
    }
}
