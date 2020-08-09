using OpenTK;

namespace ComputergrafikSpiel.Model.Character.NPC.Interfaces
{
    public interface INPCController
    {
        Vector2 EnemyAIMovement(INonPlayerCharacter myself, float dtime);
    }
}
