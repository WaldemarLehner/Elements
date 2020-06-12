using System;
using System.Collections.Generic;
using System.Linq;

namespace ComputergrafikSpiel.Model.World
{
    internal static class NoiseToTileConversionHelper
    {
        internal static WorldTile.Type[,] ConvertNoiseToTiles(float[,] noiseTiles, (int weight, WorldTile.Type type)[] noiseMapping)
        {
            (float lowerBound, WorldTile.Type type)[] map = NoiseToTileConversionHelper.GenerateMapping(noiseMapping);
            var returnArray = new WorldTile.Type[noiseTiles.GetLength(0) + 4, noiseTiles.GetLength(1) + 4];
            for (int x = 0; x < noiseTiles.GetLength(0); x++)
            {
                for (int y = 0; y < noiseTiles.GetLength(1); y++)
                {
                    var tile = NoiseToTileConversionHelper.GetTileFromNoise(noiseTiles[x, y], map);
                    returnArray[x + 2, y + 2] = tile;
                }
            }

            // Create Border
            NoiseToTileConversionHelper.AddBorder(ref returnArray);

            // Create Walkable Edge
            NoiseToTileConversionHelper.CreateWalkableBorder(ref returnArray);
            return returnArray;
        }

        private static void CreateWalkableBorder(ref WorldTile.Type[,] returnArray)
        {
            int upperX = returnArray.GetLength(0) - 2;
            int upperY = returnArray.GetLength(1) - 2;

            for (int x = 2; x < upperX; x++)
            {
                var top = returnArray[x, 2];
                var bottom = returnArray[x, upperY];
                if (!TileHelper.IsWalkable(top))
                {
                    returnArray[x, 2] = WorldTile.Type.Grass;
                }

                if (!TileHelper.IsWalkable(bottom))
                {
                    returnArray[x, upperY] = WorldTile.Type.Grass;
                }
            }

            for (int y = 2; y < upperY; y++)
            {
                var left = returnArray[2, y];
                var right = returnArray[upperX, y];
                if (!TileHelper.IsWalkable(left))
                {
                    returnArray[2, y] = WorldTile.Type.Grass;
                }

                if (!TileHelper.IsWalkable(right))
                {
                    returnArray[upperX, y] = WorldTile.Type.Grass;
                }
            }
        }

        private static void AddBorder(ref WorldTile.Type[,] returnArray)
        {
            int upperX = returnArray.GetLength(0) - 2;
            int upperY = returnArray.GetLength(1) - 2;
            int lowerX = 1, lowerY = 1;

            // Horizontals
            for (int x = lowerX; x < upperX; x++)
            {
                returnArray[x, lowerY] = returnArray[x, upperX] = WorldTile.Type.Wall;
                returnArray[x, lowerY - 1] = returnArray[x, upperX + 1] = WorldTile.Type.WallTrim;
            }

            // Verticals
            for (int y = lowerY; y < upperY; y++)
            {
                returnArray[lowerX, y] = returnArray[upperX, y] = WorldTile.Type.Wall;
                returnArray[lowerX - 1, y] = returnArray[upperX + 1, y] = WorldTile.Type.WallTrim;
            }

            // Corners
            returnArray[lowerX - 1, lowerY - 1] =
                returnArray[lowerX - 1, upperY + 1] =
                returnArray[upperX + 1, lowerY - 1] =
                returnArray[upperX + 1, upperY + 1] = WorldTile.Type.WallTrim;
        }

        private static (float lowerBound, WorldTile.Type type)[] GenerateMapping((int weight, WorldTile.Type type)[] noiseMapping)
        {
            var weightSum = noiseMapping.Sum(e => e.weight);
            int sum = 0;
            List<(float lowerBound, WorldTile.Type type)> returnList = new List<(float lowerBound, WorldTile.Type type)>();
            foreach (var (weight, type) in noiseMapping)
            {
                if (weight == 0)
                {
                    continue;
                }

                if (weight < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(weight), "Weight needs to be positive");
                }

                returnList.Add((sum / (float)weightSum, type));
                sum += weight;
            }

            return returnList.ToArray();
        }

        private static WorldTile.Type GetTileFromNoise(float value, in (float lowerBound, WorldTile.Type type)[] mappings)
        {
            int firstIndexLargerValue = 0;
            for (; firstIndexLargerValue < mappings.Length; firstIndexLargerValue++)
            {
                if (value < mappings[firstIndexLargerValue].lowerBound)
                {
                    break;
                }
            }

            if (firstIndexLargerValue > 0)
            {
                return mappings[firstIndexLargerValue--].type;
            }

            return mappings[0].type;
        }

    }
}
