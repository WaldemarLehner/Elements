using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.Character.NPC.Interfaces
{
    public interface INPCController
    {
        Vector2 EnemyAI(INonPlayerCharacter enemy, IPlayer player);
    }
}
