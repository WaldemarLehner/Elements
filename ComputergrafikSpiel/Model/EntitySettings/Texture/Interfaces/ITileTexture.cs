using System;
using ComputergrafikSpiel.Model;
using OpenTK;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces
{
    public interface ITileTexture : ITexture
    {
        int XRows { get; }

        int YRows { get; }

        Tuple<int, int> Pointer { get; }

        (Vector2 TL, Vector2 TR, Vector2 BR, Vector2 BL) GetTexCoordsOfIndex(int index);
    }
}
