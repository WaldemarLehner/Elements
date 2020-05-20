using System.Collections.Generic;
using ComputergrafikSpiel.Model.EntitySettings;

namespace ComputergrafikSpiel.Model
{
    internal interface IModel
    {
        IReadOnlyCollection<IRenderable> Renderables { get; }

        void Update(float dTime);
    }
}
