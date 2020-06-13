using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.Character.Weapon.Interfaces;
using OpenTK;
using System;
using System.Windows.Forms;

namespace ComputergrafikSpiel.Model.Character.Player.PlayerSystems
{
    internal class PlayerAttackSystem
    {
        public void PlayerAttack(IPlayer player, IWeapon weapon, Controller.Input.MouseCursor mouseCursor)
        {
            // Need Weapon to handle Collider and Damage. NPC to determine if attack hit or not

            // use weapon object in here to spawn projectiles
            Vector2 direction = Vector2.Normalize(Vector2.Subtract((Vector2)mouseCursor.WorldCoordinates, player.Position));

            // could be changed to bulletspeed instead of a set number
            direction = Vector2.Multiply(direction, 300);
            weapon.Shoot(player.Position, direction);
        }
    }
}
