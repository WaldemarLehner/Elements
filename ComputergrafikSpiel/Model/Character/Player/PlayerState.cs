namespace ComputergrafikSpiel.Model.Character.Player
{
    internal struct PlayerState : IPlayerState
    {
        public PlayerState(uint currency, uint health, uint maxHealth, float movementSpeed, float firerate, int attackDamage, float bulletTTL)
        {
            this.Currency = currency;
            this.Health = health;
            this.MaxHealth = maxHealth;
            this.MovementSpeed = movementSpeed;
            this.Firerate = firerate;
            this.AttackDamage = attackDamage;
            this.BulletTTL = bulletTTL;
        }

        public uint Currency { get; set; }

        public uint Health { get; set; }

        public uint MaxHealth { get; set; }

        public float MovementSpeed { get; set; }

        public float Firerate { get; set; }

        public float AttackDamage { get; set; }

        public float BulletTTL { get; set; }

        public IPlayerState CurrentReadonly => this;
    }
}
