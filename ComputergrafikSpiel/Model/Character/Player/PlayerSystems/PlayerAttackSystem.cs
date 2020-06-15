using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ComputergrafikSpiel.Model.Character.NPC.Interfaces;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.Character.Weapon.Interfaces;
using OpenTK;
using OpenTK.Input;

namespace ComputergrafikSpiel.Model.Character.Player.PlayerSystems
{
    internal class PlayerAttackSystem
    {
        public void PlayerAttack(IPlayer player, IWeapon weapon, Vector2 mouseCursorCooridnates, ICollection<INonPlayerCharacter> enemyList)
        {
            Vector2 direction = Vector2.Normalize(Vector2.Subtract(mouseCursorCooridnates, player.Position));

            // could be changed to bulletspeed instead of a set number?
            direction = Vector2.Multiply(direction, 300);
            weapon.Shoot(player.Position, direction, enemyList);
        }
    }
}
