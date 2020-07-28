using System;
using ComputergrafikSpiel.Controller.Input;
using ComputergrafikSpiel.Model.Character.Player;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
using ComputergrafikSpiel.Model.Scene;
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
            this.CursorVisible = false;
            var player = new Player();
            Scene.CreatePlayer(player);
        }

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
            this.Model.Update((float)e.Time);
            this.Model.UpdateInput(IInputStateGenerationHelper.GenerateInputState(this.View.Renderer, this.cursorNDC, this.Focused));
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
