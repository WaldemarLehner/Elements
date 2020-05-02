using System.Collections.Generic;

namespace ComputergrafikSpiel.Model
{
    internal interface IModel
    {
        IReadOnlyCollection<IRenderable> Renderables { get; }

        void Update(float dTime);
    }
}
