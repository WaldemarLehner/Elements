using ComputergrafikSpiel.Model.Character.Player.PlayerSystems;
using ComputergrafikSpiel.Model.World;

namespace ComputergrafikSpiel.Model.Character.NPC.Interfaces
{
    public interface INonPlayerCharacter : ICharacter
    {
        INPCController NPCController { get; }

        EnemyEnum.Variant Variant { get; }

        void IncreaseDifficulty(int multiplier);

        void TakingDamage(int damage);

        void ShootBullet(float dtime);

        void GiveDamageToPlayer();

        void Dash();
    }
}
