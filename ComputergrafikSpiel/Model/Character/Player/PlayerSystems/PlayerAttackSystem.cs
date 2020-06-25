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
        public void PlayerAttack(Vector2 mouseCursorCooridnates)
        {
            Vector2 direction = Vector2.Normalize(Vector2.Subtract(mouseCursorCooridnates, Scene.Scene.Player.Position));

            direction = Vector2.Multiply(direction, 300);
            Scene.Scene.Player.EquipedWeapon.CreateProjectile(direction);
        }
    }
}
