using System.Collections.Generic;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces
{
    internal interface IMappedTileFont : ITileTexture
    {
        Dictionary<char, int> MappedPositions { get; }

        void UpdatePointer(char key);

        (int x, int y) GetTileOfKey(char key);
    }
}
