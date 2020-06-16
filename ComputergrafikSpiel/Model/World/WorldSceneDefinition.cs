using ComputergrafikSpiel.Model.World.Interfaces;

namespace ComputergrafikSpiel.Model.World
{
    internal class WorldSceneDefinition : IWorldSceneDefinition
    {
        internal WorldSceneDefinition(bool doorTop, bool doorBottom, bool doorLeft, bool doorRight, int xCount, int yCount, float noiseScale, int size, (int weight, TileDefinitions.Type type)[] tiletypeWeight)
        {
            this.DoorTop = doorTop;
            this.DoorBottom = doorBottom;
            this.DoorLeft = doorLeft;
            this.DoorRight = doorRight;
            this.TileCount = (xCount, yCount);
            this.NoiseScale = noiseScale;
            this.TileSize = size;
            this.NoiseDefinition = tiletypeWeight;
        }

        public static (int weight, TileDefinitions.Type type)[] DefaultMapping => new (int weight, TileDefinitions.Type type)[] { (3, TileDefinitions.Type.Water), (5, TileDefinitions.Type.Grass), (2, TileDefinitions.Type.Dirt) };

        public bool DoorTop { get; private set; }

        public bool DoorBottom { get; private set; }

        public bool DoorLeft { get; private set; }

        public bool DoorRight { get; private set; }

        public (int x, int y) TileCount { get; private set; }

        public float NoiseScale { get; private set; }

        public int TileSize { get; private set; }

        public (int weight, TileDefinitions.Type type)[] NoiseDefinition { get; }
    }
}
