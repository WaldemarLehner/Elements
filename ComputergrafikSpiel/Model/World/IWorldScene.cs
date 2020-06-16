using System.Collections.Generic;
using ComputergrafikSpiel.Model.World.Interfaces;

namespace ComputergrafikSpiel.Model.World
{
    public interface IWorldScene
    {
        IWorldSceneDefinition SceneDefinition { get; }

        IWorldTile[,] WorldTiles { get; }

        IEnumerable<IWorldTile> WorldTilesEnumerable { get; }

        (float top, float bottom, float left, float right) WorldSceneBounds { get; }

        IEnumerable<IWorldObstacle> Obstacles { get; }
    }
}