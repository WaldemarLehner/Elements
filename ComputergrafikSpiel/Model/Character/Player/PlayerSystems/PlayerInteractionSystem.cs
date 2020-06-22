using System;
using System.Collections.Generic;
using System.Linq;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.Character.Weapon.Interfaces;
using ComputergrafikSpiel.Model.Entity;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;

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

                Scene.Scene.Current.RemoveEntity(inc);
            }

            // TODO: Interaction Blacksmith?!
        }
    }
}