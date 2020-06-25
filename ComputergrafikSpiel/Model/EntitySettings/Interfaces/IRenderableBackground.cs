using OpenTK.Graphics.OpenGL;

namespace ComputergrafikSpiel.Model.EntitySettings.Interfaces
{
    public interface IRenderableBackground : IRenderable
    {
        TextureWrapMode WrapMode { get; }
    }
}
