using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;

namespace ComputergrafikSpiel.Model.EntitySettings.Interfaces
{
    internal interface IRenderable : ITransformable
    {
        ITexture Texture { get; }
    }
}