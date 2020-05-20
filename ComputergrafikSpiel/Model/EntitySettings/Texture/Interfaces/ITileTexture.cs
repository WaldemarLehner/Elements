using System;
using ComputergrafikSpiel.Model;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces
{
    internal interface ITileTexture : ITexture
    {
        int XRows { get; }

        int YRows { get; }

        Tuple<int, int> Pointer { get; }
    }
}
