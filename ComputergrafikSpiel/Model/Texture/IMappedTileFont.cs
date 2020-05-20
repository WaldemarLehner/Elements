using System;
using System.Collections.Generic;

namespace ComputergrafikSpiel.Texture
{
    internal interface IMappedTileFont : ITileTexture
    {
        Dictionary<char, int> MappedPositions { get; }

        void UpdatePointer(char key);

        Tuple<int, int> GetTileOfKey(char key);
    }
}
