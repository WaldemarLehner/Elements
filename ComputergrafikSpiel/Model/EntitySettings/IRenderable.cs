using OpenTK;

namespace ComputergrafikSpiel.Model.EntitySettings
{
    internal interface IRenderable : ITransformable
    {
        ITexture Texture { get; }
    }
}