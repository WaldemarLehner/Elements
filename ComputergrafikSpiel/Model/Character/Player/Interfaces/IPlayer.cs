using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model.Character.Weapon.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.Character.Player.Interfaces
{
    public interface IPlayer : ICharacter
    {
        event EventHandler PlayerInc;

        IWeapon EquipedWeapon { get; }

        (int currentHealth, int maxHealth, int currency) PlayerData { get; }

        // Receives a enum list of pressed player actions -> MoveUp, MoveDown, MoveLeft, MoveRight, Dash, Attack, Interaction
        void PlayerControl(List<PlayerEnum.PlayerActions> pressedActions, Vector2 mouseControlerCooridantes);

        void OnInc(EventArgs e);

        void TakingDamage(int damage);

        void IncreasePlayerStats(int incNumber, PlayerEnum.Stats incstats);

        void Equip(Weapon.Weapon weapon);
    }
}