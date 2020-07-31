namespace ComputergrafikSpiel.Model.Character.Player
{
    internal interface IPlayerState
    {
        uint Currency { get; }

        uint Health { get; }

        uint MaxHealth { get; }

        float MovementSpeed { get; }

        float Firerate { get; }

        float AttackDamage { get; }

        float BulletTTL { get; }
    }
}