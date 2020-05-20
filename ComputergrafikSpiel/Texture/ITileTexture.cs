using ComputergrafikSpiel.Model;
using System;

namespace ComputergrafikSpiel.Texture
{
    internal interface ITileTexture : ITexture
    {
        int xRows { get; }

        int yRows { get; }

        Tuple<int, int> Pointer { get; }
    }
}
