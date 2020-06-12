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
            var tileResult = NoiseToTileConversionHelper.ConvertNoiseToTiles(noiseResult, this.WorldSceneDefinition.NoiseDefinition);

            return null;
        }

        
    }
}
