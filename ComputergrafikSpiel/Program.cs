using System;
using System.IO;
using ComputergrafikSpiel.Model.Interfaces;
using ComputergrafikSpiel.View.Interfaces;

namespace ComputergrafikSpiel
{
    public class Program
    {
        /// <summary>
        /// This is the entry point. This is where the program starts.
        /// </summary>
        public static void Main()
        {
            IModel model = new Model.Model();
            IView view = new View.View(model.Renderables);
            Controller.Controller controller = new Controller.Controller(view, model, 200, 200, "Test");
            controller.Run(60f, 0f);
        }
    }
}
