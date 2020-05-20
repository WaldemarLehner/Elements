using System.Collections.Generic;
using ComputergrafikSpiel.Model.Character.Player;
using OpenTK.Input;

namespace ComputergrafikSpiel.Controller
{
    public class InputController : IInputController
    {
        private IPlayerControl playerControl;
        private List<PlayerEnum.PlayerActions> pressedActions;

        // Initialize InputController => gets a struct of Dictionary
        // Shall be called in the Constructor of Controller
        public InputController(InputControllerSettings controllersettings)
        {
            this.pressedActions = new List<PlayerEnum.PlayerActions>();
            this.MouseDefinitions = controllersettings.MouseAction;
            this.KeyboardDefinitions = controllersettings.KeyboardAction;
            this.playerControl = new Player();
        }

        private Dictionary<Key, PlayerEnum.PlayerActions> KeyboardDefinitions { get; set; }

        private Dictionary<MouseButton, PlayerEnum.PlayerActions> MouseDefinitions { get; set; }

        // Check if pressed key is a allowed player action
        // Shall be called in OnUpdateFrame()
        public void PlayerAction()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            foreach (var key in this.KeyboardDefinitions.Keys)
            {
                if (keyboardState.IsKeyDown(key))
                {
                    this.pressedActions.Add(this.KeyboardDefinitions[key]);
                }
            }

            foreach (var button in this.MouseDefinitions.Keys)
            {
                if (mouseState.IsButtonDown(button))
                {
                    this.pressedActions.Add(this.MouseDefinitions[button]);
                }
            }

            // Gives the Player a IReadOnlyList of pressed Actions
            this.playerControl.PlayerControl(this.pressedActions);

            // Clear list for next input
            this.pressedActions.Clear();
        }
    }
}