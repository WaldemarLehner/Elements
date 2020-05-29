using System.Collections.Generic;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;

namespace ComputergrafikSpiel.Model.Interfaces
{
    internal interface IModel
    {
        IReadOnlyCollection<IRenderable> Renderables { get; }

        void Update(float dTime);

        bool CreatePlayerOnce(IInputController controller);
    }
}
