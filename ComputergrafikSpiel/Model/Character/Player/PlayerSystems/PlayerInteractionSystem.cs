using System.Collections.Generic;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.Entity;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;

namespace ComputergrafikSpiel.Model.Character.Player.PlayerSystems
{
    internal class PlayerInteractionSystem
    {
        private IEntity stats;
        private IReadOnlyDictionary<PlayerEnum.Stats, IEntity> incInteractable;
        private List<PlayerEnum.Stats> statsList;

        public PlayerInteractionSystem(IReadOnlyDictionary<PlayerEnum.Stats, IEntity> interactable)
        {
            this.incInteractable = interactable;
            this.statsList = new List<PlayerEnum.Stats>();
        }

        public void PlayerInteraction(IPlayer player)
        {
            this.statsList.Clear();
            // Need other collider (From NPC etc.) to determine if player is in range of interaction => playercollider.DidCollideWith(otherCollider)
            if (this.incInteractable.TryGetValue(PlayerEnum.Stats.MaxHealth, out this.stats))
            {
                if (player.Collider.DidCollideWith(this.stats.Collider))
                {
                    this.statsList.Add(PlayerEnum.Stats.MaxHealth);
                    player.IncreasePlayerStats(1, this.statsList);
                }
            }

            if (this.incInteractable.TryGetValue(PlayerEnum.Stats.Heal, out this.stats))
            {
                if (player.Collider.DidCollideWith(this.stats.Collider))
                {
                    this.statsList.Add(PlayerEnum.Stats.Heal);
                    player.IncreasePlayerStats(1, this.statsList);
                }
            }

            if (this.incInteractable.TryGetValue(PlayerEnum.Stats.Defense, out this.stats))
            {
                if (player.Collider.DidCollideWith(this.stats.Collider))
                {
                    this.statsList.Add(PlayerEnum.Stats.Defense);
                    player.IncreasePlayerStats(1, this.statsList);
                }
            }

            if (this.incInteractable.TryGetValue(PlayerEnum.Stats.AttackSpeed, out this.stats))
            {
                if (player.Collider.DidCollideWith(this.stats.Collider))
                {
                    this.statsList.Add(PlayerEnum.Stats.AttackSpeed);
                    player.IncreasePlayerStats(5, this.statsList);
                }
            }

            if (this.incInteractable.TryGetValue(PlayerEnum.Stats.MovementSpeed, out this.stats))
            {
                if (player.Collider.DidCollideWith(this.stats.Collider))
                {
                    this.statsList.Add(PlayerEnum.Stats.MovementSpeed);
                    player.IncreasePlayerStats(20, this.statsList);
                }
            }

            if (this.incInteractable.TryGetValue(PlayerEnum.Stats.Währung, out this.stats))
            {
                if (player.Collider.DidCollideWith(this.stats.Collider))
                {
                    this.statsList.Add(PlayerEnum.Stats.Währung);
                    player.IncreasePlayerStats(200, this.statsList);
                }
            }

        }
    }
}
