using ComputergrafikSpiel.Model.EntitySettings.Interfaces;

namespace ComputergrafikSpiel.Model.World
{
    public interface IWorldTile : IRenderableLayeredTextures
    {
        // X, Y Koordinaten, wo Grid positioniert werden soll
        (int x, int y) GridPosition { get; }

        TileDefinitions.Type TileType { get; }
    }
}
