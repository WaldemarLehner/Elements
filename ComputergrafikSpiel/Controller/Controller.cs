using System;
using ComputergrafikSpiel.Model;
using ComputergrafikSpiel.View;
using OpenTK;
using OpenTK.Graphics;

namespace ComputergrafikSpiel.Controller
{
    internal class Controller : GameWindow
    {
        internal Controller(IView view, IModel model, int width, int height, string title)
           : base(width, height, GraphicsMode.Default, title)
        {
            this.View = view;
            this.Model = model;
            this.InputController = new InputController(InputControllerSettings.Default);
        }

        private IView View { get; set; }

        private IModel Model { get; set; }

        private InputController InputController { get; set; }

        // OnResize soll den Trigger an View weiterleiten
        protected override void OnResize(EventArgs e)
        {
            this.View.Resize(this.Width, this.Height);
            base.OnResize(e);
        }

        // OnRenderFrame soll den Trigger an View weiterleiten
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            // Context.SwapBuffers();
            this.View.Render();
            base.OnRenderFrame(e);
        }

        // OnUpdateFrame soll den Trigger an Model und Input Controller weiterleiten
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            this.InputController.PlayerAction();
            this.Model.Update((float)e.Time);
            base.OnUpdateFrame(e);
        }
    }
}
