namespace ComputergrafikSpiel.Model.Character.Player
{
    public class PlayerEnum
    {
        // Player Actions as Enums => Look @ InputControllerSettings for corresponding keys
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "self-explanatory")]
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "self-explanatory")]
        public enum Stats
        {
            MaxHealth,
            Heal,
            Defense,
            AttackSpeed,
            MovementSpeed,
            Money,
            BulletTTL,
            BulletDamage,
            Reset,
            Quit,
        }
    }
}
