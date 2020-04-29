using System.Collections.Generic;
using ComputergrafikSpiel.Model;
using OpenTK;
using OpenTK.Input;

namespace ComputergrafikSpiel.Controller
{
    public class InputController
    {
        private static List<Key> pressedKey;
        private static List<MouseButton> pressedMouse;
        private static List<PlayerActions> pressedAction;
        private readonly IPlayerControl playerControl;

        public enum PlayerActions
        {
            Up,
            Down,
            Left,
            Right,
            Dash,
            Attack,
        }

        // Initialize "ControllerListener" -> puts every pressed key/button in a array-list and remove released key/button out of it.
        // Shall be called in Controller Constructor
        public static void ControllerListener(GameWindow game)
        {
            pressedKey = new List<Key>();
            pressedMouse = new List<MouseButton>();

            game.KeyDown += GameKeyDown;
            game.KeyUp += GameKeyUp;
            game.MouseDown += GameMouseDown;
            game.MouseUp += GameMouseUp;
        }

        // Check if pressed key is a allowed player action
        // Shall be called in OnUpdateFrame()
        public void PlayerAction()
        {
            pressedAction = new List<PlayerActions>();

            // Compare allowed keys with pressedKey list and add action to pressedAction list
            if (pressedKey.Contains(OpenTK.Input.Key.W))
                {
                    pressedAction.Add(PlayerActions.Up);
                }
                else if (pressedKey.Contains(OpenTK.Input.Key.A))
                {
                    pressedAction.Add(PlayerActions.Left);
                }
                else if (pressedKey.Contains(OpenTK.Input.Key.S))
                {
                    pressedAction.Add(PlayerActions.Down);
                }
                else if (pressedKey.Contains(OpenTK.Input.Key.D))
                {
                    pressedAction.Add(PlayerActions.Right);
                }
                else if (pressedKey.Contains(OpenTK.Input.Key.ShiftLeft))
                {
                    pressedAction.Add(PlayerActions.Dash);
                }

            // Compare allowed MouseButton with pressedMouse list and add action to pressedAction list
            if (pressedMouse.Contains(OpenTK.Input.MouseButton.Left))
                {
                   pressedAction.Add(PlayerActions.Attack);
                }

            // Give Interface pressedAction list as IReadOnly
            this.playerControl.PlayerControl(pressedAction);
        }

        // Add pressed key in "List<Key> pressedKey" array-list.
        private static void GameKeyDown(object s, KeyboardKeyEventArgs pressed)
        {
            if (!pressedKey.Contains(pressed.Key))
            {
                pressedKey.Add(pressed.Key);
            }
        }

        // Remove released key out of "List<Key> pressedKey" array-list.
        private static void GameKeyUp(object s, KeyboardKeyEventArgs pressed)
        {
            while (pressedKey.Contains(pressed.Key))
            {
                pressedKey.Remove(pressed.Key);
            }
        }

        // Add pressed button in "List<MouseButton> pressedMouse" array-list.
        private static void GameMouseDown(object s, MouseButtonEventArgs pressed)
        {
            if (!pressedMouse.Contains(pressed.Button))
            {
                pressedMouse.Add(pressed.Button);
            }
        }

        // Remove released button out of "List<MouseButton> pressedMouse" array-list.
        private static void GameMouseUp(object s, MouseButtonEventArgs pressed)
        {
            while (pressedMouse.Contains(pressed.Button))
            {
                pressedMouse.Remove(pressed.Button);
            }
        }
    }
}