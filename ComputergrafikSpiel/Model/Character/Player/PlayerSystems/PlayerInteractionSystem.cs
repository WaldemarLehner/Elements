using System.Linq;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.Entity;

namespace ComputergrafikSpiel.Model.Character.Player.PlayerSystems
{
    public class PlayerInteractionSystem
    {
        public PlayerInteractionSystem()
        {
        }

        public void PlayerInteraction(IPlayer player)
        {
            var collidables = Scene.Scene.Current.ColliderManager.GetCollisions(player);

            foreach (var inc in from i in collidables where i is Interactable select i as Interactable)
            {
                inc.PlayerStatsIncrease();

                if (inc.SingleDelete)
                {
                    inc.RemoveInteractable();
                }
                else
                {
                    var allinteractable = Scene.Scene.Current.Entities.ToList();

                    foreach (var interactable in from i in allinteractable where i is Interactable select i as Interactable)
                    {
                        if (interactable.DeleteAll)
                        {
                            interactable.RemoveInteractable();
                        }
                    }
                }
            }

            // TODO: Interaction Blacksmith?!
        }
    }
}