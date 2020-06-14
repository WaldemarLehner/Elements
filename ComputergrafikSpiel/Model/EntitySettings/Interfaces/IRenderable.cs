using System.Collections.Generic;
using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.EntitySettings.Interfaces
{
    public interface IRenderable : ITransformable
    {
        ITexture Texture { get; }

        IEnumerable<(OpenTK.Graphics.Color4 color, Vector2[] vertices)> DebugData { get; }
    }

    public interface IRenderableLayeredTextures : IRenderable
    {
        new (IEnumerable<TextureCoordinates>, ITileTexture) Texture { get; }
    }
}