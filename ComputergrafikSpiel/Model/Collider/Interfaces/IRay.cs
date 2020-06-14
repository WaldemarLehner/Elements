using System;
using System.Collections.Generic;
using OpenTK;

namespace ComputergrafikSpiel.Model.Collider.Interfaces
{
    public interface IRay
    {
        Vector2 Position { get; }

        Vector2 Direction { get; }

        float MaxDistance { get; }

        ColliderLayer.Layer Layer { get; }

        float MinimalDistanceTo(Vector2 tileCenter);
    }
}