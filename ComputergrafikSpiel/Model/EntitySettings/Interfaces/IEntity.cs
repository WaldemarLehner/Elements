using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;

namespace ComputergrafikSpiel.Model.EntitySettings.Interfaces
{
    public interface IEntity : ICollidable, IUpdateable, IRenderable
    {
    }
}
