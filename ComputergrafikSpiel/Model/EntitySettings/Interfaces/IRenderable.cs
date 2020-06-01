using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;

namespace ComputergrafikSpiel.Model.EntitySettings.Interfaces
{
    public interface IRenderable : ITransformable
    {
        ITexture Texture { get; }
    }
}