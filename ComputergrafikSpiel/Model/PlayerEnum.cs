namespace ComputergrafikSpiel.Model
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
            Dash,
            Attack,
            Interaction,
        }

        public enum Stats
        {
            MaxHealth,
            Defense,
            MovementSpeed,
        }
    }
}
