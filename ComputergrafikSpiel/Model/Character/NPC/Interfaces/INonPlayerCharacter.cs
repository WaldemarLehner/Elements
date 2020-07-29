namespace ComputergrafikSpiel.Model.Character.NPC.Interfaces
{
    public interface INonPlayerCharacter : ICharacter
    {
        INPCController NPCController { get; }

        void IncreaseDifficulty(int multiplier);

        void TakingDamage(int damage);

    }
}
