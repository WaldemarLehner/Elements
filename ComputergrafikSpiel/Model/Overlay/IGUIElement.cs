using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.Overlay
{
    public interface IGUIElement
    {
        ITexture Texture { get; }

        Vector2 Offset { get; }

        (float? width, float? height) Size { get; set; }

        float AspectRatio { get; }
    }
}