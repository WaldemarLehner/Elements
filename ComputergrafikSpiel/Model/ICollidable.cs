namespace ComputergrafikSpiel.Model
{
    internal interface ICollidable : ITransformable
    {
        ICollider Collider { get; }
    }
}