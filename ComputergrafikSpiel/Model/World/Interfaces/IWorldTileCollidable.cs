using ComputergrafikSpiel.Model.Collider.Interfaces;

namespace ComputergrafikSpiel.Model.World.Interfaces
{
    public interface IWorldTileCollidable : IWorldTile, ICollidable
    {
        // Enum -> ColType wird erst noch definiert
        // ColType CollidableType { get; }

        // X, Y Koordinaten, wo Grid positioniert werden soll
        int PositionX { get; }

        int PositionY { get; }
    }
}
