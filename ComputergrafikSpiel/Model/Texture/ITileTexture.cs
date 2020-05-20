using System;
using ComputergrafikSpiel.Model;

namespace ComputergrafikSpiel.Texture
{
    internal interface ITileTexture : ITexture
    {
        int XRows { get; }

        int YRows { get; }

        Tuple<int, int> Pointer { get; }
    }
}
