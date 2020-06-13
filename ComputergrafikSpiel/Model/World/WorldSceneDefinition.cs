using ComputergrafikSpiel.Model.World.Interfaces;

namespace ComputergrafikSpiel.Model.World
{
    internal class WorldSceneDefinition : IWorldSceneDefinition
    {
        internal WorldSceneDefinition(bool doorTop, bool doorBottom, bool doorLeft, bool doorRight, int xCount, int yCount, int noiseScale)
        {
            this.DoorTop = doorTop;
            this.DoorBottom = doorBottom;
            this.DoorLeft = doorLeft;
            this.DoorRight = doorRight;
            this.TileCount = (xCount, yCount);
            this.NoiseScale = noiseScale;
        }

        public bool DoorTop { get; private set; }

        public bool DoorBottom { get; private set; }

        public bool DoorLeft { get; private set; }

        public bool DoorRight { get; private set; }

        public (int x, int y) TileCount { get; private set; }

        public float NoiseScale { get; private set; }

        public (int weight, WorldTile.Type type)[] NoiseDefinition { get; }
    }
}
