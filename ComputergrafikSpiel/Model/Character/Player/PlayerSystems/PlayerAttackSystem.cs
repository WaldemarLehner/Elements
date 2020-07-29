using OpenTK;

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
