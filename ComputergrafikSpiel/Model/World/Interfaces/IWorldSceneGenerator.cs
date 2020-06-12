namespace ComputergrafikSpiel.Model.World.Interfaces
{
    public interface IWorldSceneGenerator
    {
        IWorldSceneDefinition WorldSceneDefinition { get; }

        IWorldScene GenerateWorldScene();

        

        
    }
}
