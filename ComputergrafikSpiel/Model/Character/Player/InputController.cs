using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model.Interfaces;
using OpenTK.Input;

namespace ComputergrafikSpiel.Model.Character.Player
{
    public class InputController
    {
        private readonly List<PlayerEnum.PlayerActions> pressedActions;
        private bool lockedInteractKey = false;
        private bool lockedDashKey = false;

        // Initialize InputController => gets a struct of Dictionary
        // Shall be called in the Constructor of Controller
        public InputController(InputControllerSettings controllersettings)
        {
            this.pressedActions = new List<PlayerEnum.PlayerActions>();
            this.MouseDefinitions = controllersettings.MouseAction;
            this.KeyboardDefinitions = controllersettings.KeyboardAction;
        }

        private Dictionary<Key, PlayerEnum.PlayerActions> KeyboardDefinitions { get; set; }

        private Dictionary<MouseButton, PlayerEnum.PlayerActions> MouseDefinitions { get; set; }

        // Check if pressed key is a allowed player action
        // Shall be called in OnUpdateFrame()
        public IEnumerable<PlayerEnum.PlayerActions> GetActions(IInputState state)
        {
            // Clear list for next input
            this.pressedActions.Clear();

            KeyboardState keyboardState = state.KeyboardState;
            MouseState mouseState = state.MouseState;

            foreach (var key in this.KeyboardDefinitions.Keys)
            {
                /// <summary>
                /// Diese If Abfragen müssen geändert werden! Es soll eine Methode implementiert werden, die gedrückte Tasten locked,
                /// damit diese keine neue Funktionen aufrufen können, bis sie wieder released werden.
                /// </summary>
                if (keyboardState.IsKeyDown(key))
                {
                    if (this.KeyboardDefinitions[key] == PlayerEnum.PlayerActions.MoveDown || this.KeyboardDefinitions[key] == PlayerEnum.PlayerActions.MoveUp || this.KeyboardDefinitions[key] == PlayerEnum.PlayerActions.MoveRight || this.KeyboardDefinitions[key] == PlayerEnum.PlayerActions.MoveLeft || this.KeyboardDefinitions[key] == PlayerEnum.PlayerActions.Run)
                    {
                        this.pressedActions.Add(this.KeyboardDefinitions[key]);
                    }
                    else if (!this.lockedInteractKey && this.KeyboardDefinitions[key] == PlayerEnum.PlayerActions.Interaction)
                    {
                        this.pressedActions.Add(this.KeyboardDefinitions[key]);
                        this.lockedInteractKey = true;
                    }
                    else if (!this.lockedDashKey && this.KeyboardDefinitions[key] == PlayerEnum.PlayerActions.Dash)
                    {
                        this.pressedActions.Add(this.KeyboardDefinitions[key]);
                        this.lockedDashKey = true;
                    }
                }

                if (keyboardState.IsKeyUp(Key.F))
                {
                    this.lockedInteractKey = false;
                }

                if (keyboardState.IsKeyUp(Key.Space))
                {
                    this.lockedDashKey = false;
                }
            }

            foreach (var button in this.MouseDefinitions.Keys)
            {
                if (mouseState.IsButtonDown(button))
                {
                    this.pressedActions.Add(this.MouseDefinitions[button]);
                }
            }

            return this.pressedActions;
        }
    }
}