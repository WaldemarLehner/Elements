using System.Collections.Generic;
using OpenTK;
using OpenTK.Input;

namespace ComputergrafikSpiel.Controller
{
    public class InputController
    {
        private static List<Key> pressedKey;
        private static List<MouseButton> pressedMouse;

        // Initialize "ControllerListener" -> puts every pressed key/button in a array-list and remove released key/button out of it.
        // Shall be called once at the beginning
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
            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            foreach (Key key in pressedKey)
            {
                if (keyboard.IsKeyDown(Key.W).Equals(key))
                {
                    //TODO: Key W
                }
                else if (keyboard.IsKeyDown(Key.A).Equals(key))
                {
                    //TODO: Key A
                }
                else if (keyboard.IsKeyDown(Key.S).Equals(key))
                {
                    //TODO: Key S
                }
                else if (keyboard.IsKeyDown(Key.D).Equals(key))
                {
                    //TODO: Key D
                }
                else if (keyboard.IsKeyDown(Key.ShiftLeft).Equals(key))
                {
                    //TODO: Key ShiftLeft
                }
            }

            foreach (MouseButton mouseButton in pressedMouse)
            {
                if (mouse.IsButtonDown(MouseButton.Left).Equals(mouseButton))
                {
                    //TODO: Left Mouse Button
                }
            }
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