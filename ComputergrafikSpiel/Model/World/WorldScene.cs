using System;
using ComputergrafikSpiel.Model.World.Interfaces;

namespace ComputergrafikSpiel.Model.World
{
    internal class WorldScene : IWorldScene
    {
        internal WorldScene(IWorldSceneDefinition definition, IWorldTile[,] tiles)
        {
            this.SceneDefinition = definition ?? throw new ArgumentNullException(nameof(definition));
            this.WorldTiles = tiles ?? throw new ArgumentNullException(nameof(tiles));
        }

        public IWorldSceneDefinition SceneDefinition { get; }

        public IWorldTile[,] WorldTiles { get; }
    }
}
