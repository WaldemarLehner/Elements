using System;
using System.Collections.Generic;
using OpenTK;

namespace ComputergrafikSpiel.Model.Collider.Interfaces
{
    internal interface IRay
    {
        Vector2 Position { get; }

        Vector2 Direction { get; }

        float MaxDistance { get; }

        float MinimalDistanceTo(Vector2 tileCenter);
    }
}