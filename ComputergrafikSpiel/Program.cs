using ComputergrafikSpiel.Model;
using ComputergrafikSpiel.Model.Interfaces;
using ComputergrafikSpiel.View.Interfaces;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ComputergrafikSpiel
{
    public class Program
    {
        /// <summary>
        /// This is the entry point. This is where the program starts.
        /// </summary>
        public static void Main()
        {
            // Check if Godmode ist set.

            SetSettings();
            IModel model = new Model.Model();
            IView view = new View.View(model);
            Controller.Controller controller = new Controller.Controller(view, model, 800, 600, "Elements");
            controller.Run(60f, 0f);
        }

        private static void SetSettings()
        {
            // Get args, skip the Application path
            var args = Environment.GetCommandLineArgs().Skip(1);
            if (args.Any(e => e.ToLower().Contains("god")))
            {
                GlobalSettings.GodMode = true;
                Console.WriteLine("############ GODMODE ACTIVE ############ ");
            }
        }
    }
}
