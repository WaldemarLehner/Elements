using ComputergrafikSpiel.Model.Collider.Interfaces;

namespace ComputergrafikSpiel.Model.Character.Player.PlayerSystems
{
    internal class PlayerInteractionSystem
    {
        public void PlayerInteraction(ICollider playercollider)
        {
            // Need other collider (From NPC etc.) to determine if player is in range of interaction => playercollider.DidCollideWith(otherCollider)
        }
    }
}
