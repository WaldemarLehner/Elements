using System;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
using ComputergrafikSpiel.View.Interfaces;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;

namespace ComputergrafikSpiel.Controller
{
    internal class Controller : GameWindow
    {
        private Vector2 cursorNDC = Vector2.Zero;

        internal Controller(IView view, IModel model, int width, int height, string title)
           : base(width, height, GraphicsMode.Default, title)
        {
            this.View = view;
            this.Model = model;
            this.InputController = new Input.InputController(Input.InputControllerSettings.Default);
            // Wird später von der Szene geladen, bei Rundenende
            this.Model.CreateRoundEndInteractables();
            this.Model.CreatePlayerOnce(this.InputController);
            this.Model.CreateEnemy();
        }

        public IInputController InputController { get; private set; }

        internal IView View { get; private set; }

        internal IModel Model { get; private set; }

        // OnResize soll den Trigger an View weiterleiten
        protected override void OnResize(EventArgs e)
        {
            this.View.Resize(this.Width, this.Height);
            base.OnResize(e);
        }

        // OnRenderFrame soll den Trigger an View weiterleiten
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            this.View.Render();
            this.SwapBuffers();
            base.OnRenderFrame(e);
        }

        // OnUpdateFrame soll den Trigger an Model und Input Controller weiterleiten
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            this.InputController.PlayerAction(this.View.Renderer, this.cursorNDC);
            this.Model.Update((float)e.Time);
            base.OnUpdateFrame(e);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            this.cursorNDC = new Vector2(
                x: (e.X - (this.Width / 2f)) / (this.Width / 2f),
                y: (e.Y - (this.Height / 2f)) / (this.Height / 2f));
            base.OnMouseMove(e);
        }
    }
}
