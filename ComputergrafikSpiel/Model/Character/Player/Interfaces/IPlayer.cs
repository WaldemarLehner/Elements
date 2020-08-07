using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model.Character.Weapon.Interfaces;
using ComputergrafikSpiel.Model.Overlay.UpgradeScreen;

namespace ComputergrafikSpiel.Model.Character.Player.Interfaces
{
    public interface IPlayer : ICharacter
    {
        IWeapon EquipedWeapon { get; }

        bool Invulnerable { get; set; }

        (int currentHealth, int maxHealth, int currency, float bulletTTL, int bulletDamage) PlayerData { get; }

        void TakingDamage(int damage);

        void TakeHeal();

        void TakeMoney();

        // Receives a enum list of pressed player actions -> MoveUp, MoveDown, MoveLeft, MoveRight, Dash, Attack, Interaction
        void PlayerControl();

        void Equip(Weapon.Weapon weapon);

        IList<UpgradeOption> GetOptions(uint currentLevel);

        void SelectOption(PlayerEnum.Stats stat, uint level);

        void ChangePosition();
    }
}