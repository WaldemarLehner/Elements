using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
using System;
using System.Collections.Generic;

namespace ComputergrafikSpiel.Test.Model.TestHelper
{
    public class MockModel : IModel
    {
        public (float top, float bottom, float left, float right) CurrentSceneBounds => (100, 0, 0, 100);

        public List<IUiRenderable> UiRenderableList { get; set; } = new List<IUiRenderable>();

        public IEnumerable<IUiRenderable> UiRenderables => UiRenderableList;

        public List<IRenderable> RenderableList { get; set; } = new List<IRenderable>();

        public IEnumerable<IRenderable> Renderables => RenderableList;

        public bool CreatePlayerOnce(IInputController controller)
        {
            throw new NotImplementedException();
        }

        public bool CreateTestEnemy()
        {
            throw new NotImplementedException();
        }

        public bool CreateTestInteractable()
        {
            throw new NotImplementedException();
        }

        public void Update(float dTime)
        {
            throw new NotImplementedException();
        }
    }
}
