using System;
using ComputergrafikSpiel.Model;

namespace ComputergrafikSpiel.Texture
{
    internal interface ITileTexture : ITexture
    {
        int xRows { get; }

        int yRows { get; }

        Tuple<int, int> Pointer { get; }
    }
}
