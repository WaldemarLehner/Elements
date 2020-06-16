using System.Collections.Generic;

namespace ComputergrafikSpiel.Model.World.Interfaces
{
    public interface IWorldScene
    {
        IWorldSceneDefinition SceneDefinition { get; }

        IWorldTile[,] WorldTiles { get; }

        IEnumerable<IWorldObstacle> Obstacles { get; }

        IEnumerable<IWorldTile> WorldTilesEnumerable { get; }

        (float top, float bottom, float left, float right) WorldSceneBounds { get; }
    }
}
