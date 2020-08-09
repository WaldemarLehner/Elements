using OpenTK;

namespace ComputergrafikSpiel.Model.Character.Player.PlayerSystems
{
    internal class PlayerAttackSystem
    {
        public void PlayerAttack(Vector2 mouseCursorCooridnates)
        {
            Vector2 playerPosition = new Vector2(Scene.Scene.Player.Position.X, Scene.Scene.Player.Position.Y - 10.5f); // Lowered position to spawn bullet on pistol level
            Vector2 direction = Vector2.Normalize(Vector2.Subtract(mouseCursorCooridnates, playerPosition));

            direction = Vector2.Multiply(direction, 300);
            Scene.Scene.Player.EquipedWeapon.CreateProjectile(direction);
        }
    }
}
