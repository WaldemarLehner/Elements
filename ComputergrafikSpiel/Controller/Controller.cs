using System;
using ComputergrafikSpiel.Model;
using ComputergrafikSpiel.View;
using OpenTK;
using OpenTK.Graphics;

namespace ComputergrafikSpiel.Controller
{
    internal class Controller : GameWindow

    {

        private IView View { get; set; }

        private IUpdateModel Model { get; set; }

        private IModel IModel { get; set; }

        //private IModel IModel { get; set; }

        private InputController InpController { get; set; }

        internal Controller(IView view, IModel imodel, int width, int height, string title)
            : base(width, height, GraphicsMode.Default, title)
        {
            this.View = view;
            this.IModel = imodel;
        }

        //OnResize soll den Trigger an View weiterleiten
        protected override void OnResize(EventArgs e)
        {
            this.View.Resize(this.Width, this.Height);
            base.OnResize(e);
        }

        //OnRenderFrame soll den Trigger an View weiterleiten
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            //Context.SwapBuffers();
            this.View.Render();
            base.OnRenderFrame(e);
        }

        //OnUpdateFrame soll den Trigger an Model und Input Controller weiterleiten
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            this.InpController.PlayerAction();
            this.Model.Update((float)e.Time);   //fragen ob notwendig
            base.OnUpdateFrame(e);
        }



    }

}
