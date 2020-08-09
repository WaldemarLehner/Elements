namespace ComputergrafikSpiel.Model.Character.NPC.Interfaces
{
    public interface INonPlayerCharacter : ICharacter
    {
        INPCController NPCController { get; }

        EnemyEnum.Variant Variant { get; }

        bool Air { get; set; }

        void IncreaseDifficulty(int multiplier);

        void TakingDamage(int damage);

        void ShootBullet(float dtime);

        void GiveDamageToPlayer();

        void Dash();
    }
}
