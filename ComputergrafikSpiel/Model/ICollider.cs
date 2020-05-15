namespace ComputergrafikSpiel.Model
{
    public interface ICollider
    {
        bool DidCollideWith(ICollider otherCollider);

        float MinimalDistanceTo(ICollider otherCollider);

        IReadOnlyCollection<ICollider> GetCollisions();
    }
}