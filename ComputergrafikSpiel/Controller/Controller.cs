using System;
using ComputergrafikSpiel.Model.Character.Player;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
using ComputergrafikSpiel.View.Interfaces;
using OpenTK;
using OpenTK.Graphics;

namespace ComputergrafikSpiel.Controller
{
    internal class Controller : GameWindow
    {
        private IInputController inputController;

        internal Controller(IView view, IModel model, int width, int height, string title)
           : base(width, height, GraphicsMode.Default, title)
        {
            this.View = view;
            this.Model = model;
            this.inputController = new Input.InputController(Input.InputControllerSettings.Default);
            this.Updateable = new PlayerHandler();
        }

        private IUpdateable Updateable { get; set; }

        private IView View { get; set; }

        private IModel Model { get; set; }

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
            this.inputController.PlayerAction();
            this.Model.Update((float)e.Time);
            this.Updateable.Update((float)e.Time);
            base.OnUpdateFrame(e);
        }
    }
}
