using System;
using ComputergrafikSpiel.Model;
using OpenTK;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces
{
    public interface ITileTexture : ITexture
    {
        int XRows { get; }

        int YRows { get; }

        (int x, int y) Pointer { get; }

        TextureCoordinates GetTexCoordsOfIndex(int index);
    }
}
