using ComputergrafikSpiel.Model.Interfaces;

namespace ComputergrafikSpiel.Model.EntitySettings.Interfaces
{
    public interface IParticle : IRenderable, IUpdateable
    {
        (byte r, byte g, byte b) Color { get; }
    }
}
