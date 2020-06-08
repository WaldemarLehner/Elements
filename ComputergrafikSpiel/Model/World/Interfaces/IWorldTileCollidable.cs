using ComputergrafikSpiel.Model.Collider.Interfaces;

namespace ComputergrafikSpiel.Model.World.Interfaces
{
    internal interface IWorldTileCollidable : IWorldTile, ICollidable
    {
        //Enum -> Type noch nicht bekannt
        private enum CollidableType { get; }

        // X, Y Koordinaten, wo Grid positioniert werden soll
        int PositionX { get; }

        int PositionY { get; }
    }
}
