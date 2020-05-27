using ComputergrafikSpiel.Model;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces
{
    internal interface ITextureLoader
    {
        ITexture LoadTexture(string pathOrIdentifier);
    }
}
