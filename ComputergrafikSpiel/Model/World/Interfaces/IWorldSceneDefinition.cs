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

        (int weight, WorldTile.Type type)[] NoiseDefinition { get; }
    }
}
