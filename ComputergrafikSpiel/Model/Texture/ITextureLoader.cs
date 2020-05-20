using ComputergrafikSpiel.Model;

namespace ComputergrafikSpiel.Texture
{
    internal interface ITextureLoader
    {
        ITexture LoadTexture(string pathOrIdentifier); // Noch prüfen welcher Rückgabewert verlangt
    }
}
