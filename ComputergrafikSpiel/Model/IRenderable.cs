using OpenTK;

namespace ComputergrafikSpiel.Model
{
    internal interface IRenderable : ITransformable
    {
        ITexture Texture { get; }
    }
}