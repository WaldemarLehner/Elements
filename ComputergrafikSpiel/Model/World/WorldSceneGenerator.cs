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
        private readonly float obstaclePropability; // Vielzahl an Obstacles die gespawnt werden sollen

        internal WorldSceneGenerator(float obstacleProbability, IWorldSceneDefinition definition, int? seed = null)
        {
            _ = definition ?? throw new ArgumentNullException(nameof(definition));
            this.WorldSceneDefinition = definition;
            this.Random = new Random(seed ?? new Random().Next(int.MinValue, int.MaxValue));
            this.obstaclePropability = obstacleProbability;
        }

        public IWorldSceneDefinition WorldSceneDefinition { get; }

        private Random Random { get; }

        public IWorldScene GenerateWorldScene()
        {
            Noise.Seed = this.Random.Next();
            var noiseResult = Noise.Calc2D(this.WorldSceneDefinition.TileCount.x, this.WorldSceneDefinition.TileCount.y, this.WorldSceneDefinition.NoiseScale);
            TileDefinitions.Type[,] tileResult = NoiseToTileConversionHelper.ConvertNoiseToTiles(noiseResult, this.WorldSceneDefinition.NoiseDefinition);

            List<IWorldObstacle> obstacles = new List<IWorldObstacle>();

            IWorldTile[,] tiles = this.ConstructResult(tileResult.GetLength(0), tileResult.GetLength(1), this.obstaclePropability, ref tileResult, ref obstacles);

            return new WorldScene(this.WorldSceneDefinition, tiles, obstacles.ToArray());
        }

        private IWorldTile[,] ConstructResult(int countX, int countY, float obstaclePropability, ref TileDefinitions.Type[,] tileResult, ref List<IWorldObstacle> obstacles)
        {
            var tiles = new IWorldTile[countX, countY];

            for (int x = 0; x < countX; x++)
            {
                for (int y = 0; y < countY; y++)
                {
                    var type = tileResult[x, y];
                    var neighbor = TileHelper.GetSurroundingTile(tileResult, (x, y), (countX, countY));
                    if (TileHelper.IsWalkable(type))
                    {
                        tiles[x, y] = new WorldTile(this.WorldSceneDefinition.TileSize, (x, y), type, neighbor, allowObstacle: !TileHelper.IsTileWalkableBorder((x, y), (countX - 1, countY - 1)));
                    }
                    else
                    {
                        tiles[x, y] = new CollidableWorldTile(this.WorldSceneDefinition.TileSize, (x, y), type, neighbor);
                    }

                    // Add Obstacle if appropriate
                    if (this.Random.NextDouble() < obstaclePropability)
                    {
                        this.AddObstacle(ref tiles, ref obstacles, x, y);
                    }
                }
            }

            return tiles;
        }

        private void AddObstacle(ref IWorldTile[,] tiles, ref List<IWorldObstacle> obstacles, int x, int y)
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
