using ComputergrafikSpiel.Texture;

namespace ComputergrafikSpiel.Model
{
    internal interface ITexture : ITextureLoader// , IUpdateable NOCH NICHT IMPLEMENTIERT
    {
        int Width { get; }

        int Height { get; }

        string FilePath { get; }
    }
}