using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ComputergrafikSpiel.Model.Character.Player;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
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
        private bool lockedInteractKey = false;
        private bool lockedAttackButton = false;
        private bool lockedDashKey = false;

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
                else if (keyboardState.IsKeyUp(Key.F))
                {
                    this.lockedInteractKey = false;
                }
                else if (keyboardState.IsKeyUp(Key.Space))
                {
                    this.lockedDashKey = false;
                }
            }

            foreach (var button in this.MouseDefinitions.Keys)
            {
                if (mouseState.IsButtonDown(button))
                {
                    if (!this.lockedAttackButton)
                    {
                        this.pressedActions.Add(this.MouseDefinitions[button]);
                    }
                }
                else if (mouseState.IsButtonUp(MouseButton.Left))
                {
                    this.lockedAttackButton = false;
                }
            }

            // Gives the Player a IReadOnlyList of pressed Actions
            if (this.playerControl != null)
            {
                if (this.pressedActions.Count != 0)
                {
                    this.playerControl.PlayerControl(this.pressedActions, this.mouseCursor);
                }
            }
        }
    }
}