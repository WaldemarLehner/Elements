namespace ComputergrafikSpiel.Model.World.Interfaces
{
    public interface IWorldGenerator
    {
        IWorldTile WorldTile { get; }

        IWorldSceneDefinition WorldSceneDefinition { get; }
    }
}
