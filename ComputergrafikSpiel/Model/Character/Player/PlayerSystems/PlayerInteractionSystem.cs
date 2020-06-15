using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.Character.Weapon.Interfaces;
using ComputergrafikSpiel.Model.Entity;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;

namespace ComputergrafikSpiel.Model.Character.Player.PlayerSystems
{
    internal class PlayerInteractionSystem
    {
        private IEntity stats;
        private IReadOnlyDictionary<PlayerEnum.Stats, IEntity> incInteractable;
        private List<PlayerEnum.Stats> statsList;
        private IModel model;

        public PlayerInteractionSystem(IReadOnlyDictionary<PlayerEnum.Stats, IEntity> interactable, IModel model)
        {
            this.incInteractable = interactable;
            this.statsList = new List<PlayerEnum.Stats>();
            this.model = model;
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
                    this.model.DestroyObject(null, this.stats, null);
                    player.IncreasePlayerStats(1, this.statsList);
                }
            }

            if (this.incInteractable.TryGetValue(PlayerEnum.Stats.Heal, out this.stats))
            {
                if (player.Collider.DidCollideWith(this.stats.Collider))
                {
                    this.statsList.Add(PlayerEnum.Stats.Heal);
                    this.model.DestroyObject(null, this.stats, null);
                    player.IncreasePlayerStats(1, this.statsList);
                }
            }

            if (this.incInteractable.TryGetValue(PlayerEnum.Stats.Defense, out this.stats))
            {
                if (player.Collider.DidCollideWith(this.stats.Collider))
                {
                    this.statsList.Add(PlayerEnum.Stats.Defense);
                    this.model.DestroyObject(null, this.stats, null);
                    player.IncreasePlayerStats(1, this.statsList);
                }
            }

            if (this.incInteractable.TryGetValue(PlayerEnum.Stats.AttackSpeed, out this.stats))
            {
                if (player.Collider.DidCollideWith(this.stats.Collider))
                {
                    this.statsList.Add(PlayerEnum.Stats.AttackSpeed);
                    this.model.DestroyObject(null, this.stats, null);
                    player.IncreasePlayerStats(1, this.statsList);
                }
            }

            if (this.incInteractable.TryGetValue(PlayerEnum.Stats.MovementSpeed, out this.stats))
            {
                if (player.Collider.DidCollideWith(this.stats.Collider))
                {
                    this.statsList.Add(PlayerEnum.Stats.MovementSpeed);
                    this.model.DestroyObject(null, this.stats, null);
                    player.IncreasePlayerStats(20, this.statsList);
                }
            }

            if (this.incInteractable.TryGetValue(PlayerEnum.Stats.Währung, out this.stats))
            {
                if (player.Collider.DidCollideWith(this.stats.Collider))
                {
                    this.statsList.Add(PlayerEnum.Stats.Währung);
                    this.model.DestroyObject(null, this.stats, null);
                    player.IncreasePlayerStats(200, this.statsList);
                }
            }
        }
    }
}
