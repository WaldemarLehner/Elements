using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model.World.Interfaces;
using ComputergrafikSpiel.Model.World.Obstacles;
using OpenTK;
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
            List<IWorldObstacle> obstacles = new List<IWorldObstacle>();
            const float obstaclePropability = 0.1f;
            for (int x = 0; x < xMax; x++)
            {
                for (int y = 0; y < yMax; y++)
                {
                    var type = tileResult[x, y];
                    var neighbor = TileHelper.GetSurroundingTile(tileResult, (x, y), (xMax, yMax));
                    if (TileHelper.IsWalkable(type))
                    {
                        tiles[x, y] = new WorldTile(this.WorldSceneDefinition.TileSize, (x, y), type, neighbor, allowObstacle: !TileHelper.IsTileWalkableBorder((x, y), (xMax - 1, yMax - 1)));
                    }
                    else
                    {
                        tiles[x, y] = new CollidableWorldTile(this.WorldSceneDefinition.TileSize, (x, y), type, neighbor);
                    }

                    // Add Obstacle if appropriate
                    if (this.Random.NextDouble() < obstaclePropability)
                    {
                        if ((tiles[x, y].Spawnmask & SpawnMask.Mask.AllowObstacle) != 0)
                        {
                            // We will spawn an obstacle on this tile.
                            // We will need to change this tiles Spawnmask to no longer allow Spawning of Enemies / Obstacles / Interactables.
                            tiles[x, y].Spawnmask = SpawnMask.Mask.Disallow;

                            // Get centre of tile
                            var centre = new Vector2((x + .5f) * this.WorldSceneDefinition.TileSize, (y + .5f) * this.WorldSceneDefinition.TileSize) + new Vector2(((float)this.Random.NextDouble() - .5f) * this.WorldSceneDefinition.TileSize * .2f, ((float)this.Random.NextDouble() - .5f) * this.WorldSceneDefinition.TileSize * .2f);
                            var scale = (this.Random.Next(70, 100) / 200f) * this.WorldSceneDefinition.TileSize;

                            // If tile is water, just go w/ a rock
                            if (tiles[x, y].TileType == TileDefinitions.Type.Water)
                            {
                                obstacles.Add(new RockObstacle(centre, scale));
                            }
                            else
                            {
                                // Decide between Stump and Rock
                                if (this.Random.NextDouble() < .5f)
                                {
                                    obstacles.Add(new StumpObstacle(centre, scale));
                                }
                                else
                                {
                                    obstacles.Add(new RockObstacle(centre, scale));
                                }
                            }
                        }
                    }
                }
            }

            return new WorldScene(this.WorldSceneDefinition, tiles, obstacles.ToArray());
        }
    }
}
