using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model.Character.Player;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.View.Interfaces;
using ComputergrafikSpiel.View.Renderer.Interfaces;
using OpenTK;
using OpenTK.Input;

namespace ComputergrafikSpiel.Controller.Input
{
    public class InputController : IInputController
    {
        private readonly List<PlayerEnum.PlayerActions> pressedActions;
        private readonly MouseCursor mouseCursor;
        private IPlayer playerControl;

        // Initialize InputController => gets a struct of Dictionary
        // Shall be called in the Constructor of Controller
        public InputController(InputControllerSettings controllersettings)
        {
            this.pressedActions = new List<PlayerEnum.PlayerActions>();
            this.MouseDefinitions = controllersettings.MouseAction;
            this.KeyboardDefinitions = controllersettings.KeyboardAction;
            this.playerControl = null;
            this.mouseCursor = new MouseCursor();
        }

        private Dictionary<Key, PlayerEnum.PlayerActions> KeyboardDefinitions { get; set; }

        private Dictionary<MouseButton, PlayerEnum.PlayerActions> MouseDefinitions { get; set; }

        public void HookPlayer(IPlayer control)
        {
            this.playerControl = control;
        }

        // Check if pressed key is a allowed player action
        // Shall be called in OnUpdateFrame()
        public void PlayerAction(IRenderer renderer, Vector2 cursorNDC)
        {
            // Clear list for next input
            this.pressedActions.Clear();

            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            this.mouseCursor.Update(renderer, cursorNDC);

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
            if (this.playerControl != null)
            {
                this.playerControl.PlayerControl(this.pressedActions);
            }
        }
    }
}