using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.World.Interfaces;

namespace ComputergrafikSpiel.Model.World
{
    public interface IWorldTile : IRenderable
    {
        // Enum -> ColType wird erst noch definiert
        // ColType CollidableType { get; }

        // X, Y Koordinaten, wo Grid positioniert werden soll
        (int x, int y) GridPosition { get; }
    }
}
