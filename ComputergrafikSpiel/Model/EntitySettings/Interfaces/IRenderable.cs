using System.Collections.Generic;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.EntitySettings.Interfaces
{
    public interface IRenderable : ITransformable
    {
        ITexture Texture { get; }

        IEnumerable<(OpenTK.Graphics.Color4 color, Vector2[] vertices)> DebugData { get; }
    }

    public interface IRenderableLayeredTextures : ITransformable
    {
        (IEnumerable<(Vector2 TL, Vector2 TR, Vector2 BR, Vector2 BL)>, ITileTexture) Texture { get; }

        IEnumerable<(OpenTK.Graphics.Color4 color, Vector2[] vertices)> DebugData { get; }
    }
}