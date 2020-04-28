using System.Collections.Generic;
using OpenTK;
using OpenTK.Input;

namespace ComputergrafikSpiel.Controller
{
    public class InputController
    {
        private static List<Key> pressedKey;
        private static List<MouseButton> pressedMouse;
        private static List<AllowedInput> allowedAction;

        private enum AllowedInput
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
            allowedAction = new List<AllowedInput>();

            // Compare allowed keys with pressedKey list and add action to allowedAction list
            if (pressedKey.Contains(OpenTK.Input.Key.W))
                {
                    allowedAction.Add(AllowedInput.Up);
                }
                else if (pressedKey.Contains(OpenTK.Input.Key.A))
                {
                    allowedAction.Add(AllowedInput.Left);
                }
                else if (pressedKey.Contains(OpenTK.Input.Key.S))
                {
                    allowedAction.Add(AllowedInput.Down);
                }
                else if (pressedKey.Contains(OpenTK.Input.Key.D))
                {
                    allowedAction.Add(AllowedInput.Right);
                }
                else if (pressedKey.Contains(OpenTK.Input.Key.ShiftLeft))
                {
                    allowedAction.Add(AllowedInput.Dash);
                }

            // Compare allowed MouseButton with pressedMouse list and add action to allowedAction list
            if (pressedMouse.Contains(OpenTK.Input.MouseButton.Left))
                {
                    allowedAction.Add(AllowedInput.Attack);
                }

            // TODO give Interface a IReadOnlyCollection with pressed keys from enum List allowedAction
        }

        // Add pressed Key in "List<Key> pressedKey" array-list.
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