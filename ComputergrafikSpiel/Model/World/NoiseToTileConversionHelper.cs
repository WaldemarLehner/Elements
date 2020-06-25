using System;
using System.Collections.Generic;
using System.Linq;

namespace ComputergrafikSpiel.Model.World
{
    internal static class NoiseToTileConversionHelper
    {
        internal static TileDefinitions.Type[,] ConvertNoiseToTiles(float[,] noiseTiles, (int weight, TileDefinitions.Type type)[] noiseMapping)
        {
            (float lowerBound, TileDefinitions.Type type)[] map = NoiseToTileConversionHelper.GenerateMapping(ref noiseMapping);
            var returnArray = new TileDefinitions.Type[noiseTiles.GetLength(0) + 2, noiseTiles.GetLength(1) + 2];
            for (int x = 0; x < noiseTiles.GetLength(0); x++)
            {
                for (int y = 0; y < noiseTiles.GetLength(1); y++)
                {
                    // Simplex produces a range from 0..255.
                    noiseTiles[x, y] /= 255;
                    var tile = NoiseToTileConversionHelper.GetTileFromNoise(noiseTiles[x, y], map);
                    returnArray[x + 1, y + 1] = tile;
                }
            }

            // Create Border
            NoiseToTileConversionHelper.AddBorder(ref returnArray);

            // Create Walkable Edge
            NoiseToTileConversionHelper.CreateWalkableBorder(ref returnArray);
            return returnArray;
        }

        private static void CreateWalkableBorder(ref TileDefinitions.Type[,] returnArray)
        {
            int upperX = returnArray.GetLength(0) - 2;
            int upperY = returnArray.GetLength(1) - 2;

            for (int x = 2; x < upperX; x++)
            {
                var top = returnArray[x, 2];
                var bottom = returnArray[x, upperY];
                if (!TileHelper.IsWalkable(top))
                {
                    returnArray[x, 2] = TileDefinitions.Type.Grass;
                }

                if (!TileHelper.IsWalkable(bottom))
                {
                    returnArray[x, upperY] = TileDefinitions.Type.Grass;
                }
            }

            for (int y = 2; y < upperY; y++)
            {
                var left = returnArray[2, y];
                var right = returnArray[upperX, y];
                if (!TileHelper.IsWalkable(left))
                {
                    returnArray[2, y] = TileDefinitions.Type.Grass;
                }

                if (!TileHelper.IsWalkable(right))
                {
                    returnArray[upperX, y] = TileDefinitions.Type.Grass;
                }
            }
        }

        private static void AddBorder(ref TileDefinitions.Type[,] returnArray)
        {
            // Border Bounds 
            int upperX = returnArray.GetLength(0) - 1;
            int upperY = returnArray.GetLength(1) - 1;
            int lowerX = 0, lowerY = 0;

            // Horizontals
            for (int x = lowerX; x < upperX; x++)
            {
                returnArray[x, lowerY] = TileDefinitions.Type.WallTrim;
                returnArray[x, upperY] = TileDefinitions.Type.WallTrim;
            }

            // Verticals
            for (int y = lowerY; y < upperY; y++)
            {
                returnArray[lowerX, y] = TileDefinitions.Type.WallTrim;
                returnArray[upperX, y] = TileDefinitions.Type.WallTrim;
            }

            // Corners
            returnArray[lowerX, lowerY] =
                returnArray[lowerX, upperY] =
                returnArray[upperX, lowerY] =
                returnArray[upperX, upperY] = TileDefinitions.Type.WallTrim;
        }

        private static (float lowerBound, TileDefinitions.Type type)[] GenerateMapping(ref (int weight, TileDefinitions.Type type)[] noiseMapping)
        {
            var weightSum = noiseMapping.Sum(e => e.weight);
            int sum = 0;
            List<(float lowerBound, TileDefinitions.Type type)> returnList = new List<(float lowerBound, TileDefinitions.Type type)>();
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

        private static TileDefinitions.Type GetTileFromNoise(float value, (float lowerBound, TileDefinitions.Type type)[] mappings)
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
                return mappings[--firstIndexLargerValue].type;
            }

            return mappings[0].type;
        }
    }
}
