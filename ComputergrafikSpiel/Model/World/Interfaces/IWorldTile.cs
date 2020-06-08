using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.World.Interfaces;

namespace ComputergrafikSpiel.Model.World
{
    public interface IWorldTile : IRenderable
    {
        // Enum -> TileType wird erst noch definiert (Gras, Stein etc.)
        // TileType tileType { get; }

        // X, Y Koordinaten, wo Grid positioniert werden soll
        (int x, int y) GridPosition { get; }
    }
}
