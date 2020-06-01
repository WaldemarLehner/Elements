﻿using System.Collections.Generic;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;

namespace ComputergrafikSpiel.Model.Interfaces
{
    internal interface IModel
    {
        (float top, float bottom, float left, float right) CurrentSceneBounds { get; }

        IReadOnlyCollection<IUiRenderable> UiRenderables { get; }

        IReadOnlyCollection<IRenderable> Renderables { get; }

        void Update(float dTime);
    }
}
