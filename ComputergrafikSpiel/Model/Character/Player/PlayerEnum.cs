namespace ComputergrafikSpiel.Model.Character.Player
{
    public class PlayerEnum
    {
        // Player Actions as Enums => Look @ InputControllerSettings for corresponding keys
        public enum PlayerActions
        {
            MoveUp,
            MoveDown,
            MoveLeft,
            MoveRight,
            Run,
            Attack,
            Interaction,
            Dash,
        }

        public enum Stats
        {
            MaxHealth,
            Defense,
            MovementSpeed,
        }

        public enum Weapon
        {
            Bow,
            Sword,
        }
    }
}
