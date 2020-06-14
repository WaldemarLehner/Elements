using System;
using ComputergrafikSpiel.Model.World.Interfaces;
using SimplexNoise;

namespace ComputergrafikSpiel.Model.World
{
    internal class WorldSceneGenerator : IWorldSceneGenerator
    {
        internal WorldSceneGenerator(IWorldSceneDefinition definition, int? seed = null)
        {
            _ = definition ?? throw new ArgumentNullException(nameof(definition));
            this.WorldSceneDefinition = definition;
            this.Random = new Random(seed ?? new Random().Next(int.MinValue, int.MaxValue));
        }

        public IWorldSceneDefinition WorldSceneDefinition { get; }

        private Random Random { get; }

        public IWorldScene GenerateWorldScene()
        {
            Noise.Seed = this.Random.Next();
            var noiseResult = Noise.Calc2D(this.WorldSceneDefinition.TileCount.x, this.WorldSceneDefinition.TileCount.y, this.WorldSceneDefinition.NoiseScale);
            TileDefinitions.Type[,] tileResult = NoiseToTileConversionHelper.ConvertNoiseToTiles(noiseResult, this.WorldSceneDefinition.NoiseDefinition);

            int xMax = tileResult.GetLength(0);
            int yMax = tileResult.GetLength(1);
            IWorldTile[,] tiles = new IWorldTile[xMax, yMax];
            for (int x = 0; x < xMax; x++)
            {
                for (int y = 0; y < yMax; y++)
                {
                    var type = tileResult[x, y];
                    var neighbor = TileHelper.GetSurroundingTile(in tileResult, (x, y), (xMax, yMax));
                    if (TileHelper.IsWalkable(type))
                    {
                        tiles[x, y] = new WorldTile(this.WorldSceneDefinition.TileSize, (x, y), type, neighbor);
                    }
                    else
                    {
                        tiles[x, y] = new CollidableWorldTile(this.WorldSceneDefinition.TileSize, (x, y), type, neighbor);
                    }
                }
            }

            return new WorldScene(this.WorldSceneDefinition, tiles, new IWorldObstacle[0]);
        }
    }
}
