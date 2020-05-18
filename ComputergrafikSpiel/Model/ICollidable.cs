namespace ComputergrafikSpiel.Model
{
    internal interface ICollidable : IPositionable
    {
        ICollider Collider { get; }
    }
}