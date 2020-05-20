using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model.Character.Player;
using OpenTK.Input;

namespace ComputergrafikSpiel.Controller.Input
{
    public struct InputControllerSettings
    {
        public Dictionary<Key, PlayerEnum.PlayerActions> KeyboardAction;
        public Dictionary<MouseButton, PlayerEnum.PlayerActions> MouseAction;

        public static InputControllerSettings Default => GenerateDefault();

        private static InputControllerSettings GenerateDefault()
        {
            return new InputControllerSettings
            {
                KeyboardAction = new Dictionary<Key, PlayerEnum.PlayerActions>
                {
                    [Key.W] = PlayerEnum.PlayerActions.MoveUp,
                    [Key.A] = PlayerEnum.PlayerActions.MoveLeft,
                    [Key.S] = PlayerEnum.PlayerActions.MoveDown,
                    [Key.D] = PlayerEnum.PlayerActions.MoveRight,
                    [Key.ShiftLeft] = PlayerEnum.PlayerActions.Dash,
                    [Key.F] = PlayerEnum.PlayerActions.Interaction,
                },
                MouseAction = new Dictionary<MouseButton, PlayerEnum.PlayerActions>
                {
                    [MouseButton.Left] = PlayerEnum.PlayerActions.Attack,
                },
            };
        }
    }
}
