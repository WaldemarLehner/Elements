using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model;
using OpenTK.Input;

namespace ComputergrafikSpiel.Controller
{
    public struct InputControllerSettings
    {
        public Dictionary<Key, PlayerActionEnum.PlayerActions> KeyboardAction;
        public Dictionary<MouseButton, PlayerActionEnum.PlayerActions> MouseAction;

        public static InputControllerSettings Default => GenerateDefault();

        private static InputControllerSettings GenerateDefault()
        {
            return new InputControllerSettings
            {
                KeyboardAction = new Dictionary<Key, PlayerActionEnum.PlayerActions>
                {
                    [Key.W] = PlayerActionEnum.PlayerActions.MoveUp,
                    [Key.A] = PlayerActionEnum.PlayerActions.MoveLeft,
                    [Key.S] = PlayerActionEnum.PlayerActions.MoveDown,
                    [Key.D] = PlayerActionEnum.PlayerActions.MoveRight,
                },
                MouseAction = new Dictionary<MouseButton, PlayerActionEnum.PlayerActions>
                {
                    [MouseButton.Left] = PlayerActionEnum.PlayerActions.Attack,
                    [MouseButton.Right] = PlayerActionEnum.PlayerActions.Dash,
                },
            };
        }
    }
}
