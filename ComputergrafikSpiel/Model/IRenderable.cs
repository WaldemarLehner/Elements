using OpenTK;

namespace ComputergrafikSpiel.Model
{
    internal interface IRenderable : IPositionable
    {
        ITexture Texture { get; }
    }
}