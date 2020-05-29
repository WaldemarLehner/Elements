using System.Collections.Generic;
using ComputergrafikSpiel.Model.Character.Player;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;

namespace ComputergrafikSpiel.Model
{
    internal class Model : IModel
    {
        internal Model()
        {
            this.RenderablesList = new List<IRenderable>();
            this.Updateables = new List<IUpdateable>();
        }

        public IReadOnlyCollection<IRenderable> Renderables => this.RenderablesList;

        private List<IRenderable> RenderablesList { get; }

        private List<IUpdateable> Updateables { get; }

        private IPlayer Player { get; set; } = null;

        /// <summary>
        /// For the Test, this will draw a Rectangle doing a loop.
        /// </summary>
        /// <param name="dTime">Time between two Update Calls in Seconds.</param>
        public void Update(float dTime)
        {
            foreach (var entry in this.Updateables)
            {
                entry.Update(dTime);
            }
        }

        public bool CreatePlayerOnce(IInputController controller)
        {
            if (this.Player == null)
            {
                this.Player = new Player();
                controller.HookPlayer(this.Player);
                this.Updateables.Add(this.Player);
                this.RenderablesList.Add(this.Player);
                return true;
            }

            return false;
        }
    }
}
