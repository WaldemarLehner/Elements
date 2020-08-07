using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model.Character.Weapon.Interfaces;
using ComputergrafikSpiel.Model.Overlay.EndScreen;
using ComputergrafikSpiel.Model.Overlay.UpgradeScreen;

namespace ComputergrafikSpiel.Model.Character.Player.Interfaces
{
    public interface IPlayer : ICharacter
    {
        event EventHandler PlayerInc;

        IWeapon EquipedWeapon { get; }

        (int currentHealth, int maxHealth, int currency, float bulletTTL, int bulletDamage) PlayerData { get; }

        void TakingDamage();

        void TakeHeal();

        void TakeMoney();

        // Receives a enum list of pressed player actions -> MoveUp, MoveDown, MoveLeft, MoveRight, Dash, Attack, Interaction
        void PlayerControl();

        void OnInc(EventArgs e);

        void Equip(Weapon.Weapon weapon);

        IList<UpgradeOption> GetOptions(uint currentLevel);

        void SelectOption(PlayerEnum.Stats stat, uint level);

        void ChangePosition();
    }
}