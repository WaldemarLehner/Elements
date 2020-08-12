namespace ComputergrafikSpiel.Model.World.Interfaces
{
    public interface IWorldSceneDefinition
    {
        bool DoorTop { get; }

        bool DoorBottom { get; }

        bool DoorLeft { get; }

        bool DoorRight { get; }

        (int x, int y) TileCount { get; }

        float NoiseScale { get; }

        (int weight, TileDefinitions.Type type)[] NoiseDefinition { get; }

        int TileSize { get; }

        WorldEnum.Type WorldType { get; }
    }
}
