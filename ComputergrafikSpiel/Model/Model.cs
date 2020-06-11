using System.Collections.Generic;
using ComputergrafikSpiel.Model.Character.Player;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.Entity;
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
            this.Interactable = new Dictionary<PlayerEnum.Stats, IEntity>();
        }

        public IEnumerable<IRenderable> Renderables => this.RenderablesList;

        public (float top, float bottom, float left, float right) CurrentSceneBounds => (500, 0, 0, 800);

        public IEnumerable<IUiRenderable> UiRenderables { get; } = new List<IUiRenderable>();

        private List<IRenderable> RenderablesList { get; } = new List<IRenderable>();

        private List<IUpdateable> Updateables { get; } = new List<IUpdateable>();

        private IPlayer Player { get; set; } = null;

        private IEntity IncMaxHealth { get; set; } = null;

        private IEntity IncHeal { get; set; } = null;

        private IEntity IncDefense { get; set; } = null;

        private IEntity IncAttackSpeed { get; set; } = null;

        private IEntity IncMovementSpeed { get; set; } = null;

        private IEntity IncWährung { get; set; } = null;

        private Dictionary<PlayerEnum.Stats, IEntity> Interactable { get; set; } = null;

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
                this.Player = new Player(this.Interactable);
                controller.HookPlayer(this.Player);
                this.Updateables.Add(this.Player);
                this.RenderablesList.Add(this.Player);
                return true;
            }

            return false;
        }

        public bool CreateTestInteractable()
        {
            // MaxHealth Interactable
            this.IncMaxHealth = new CreateInteractable(PlayerEnum.Stats.MaxHealth, 200, 100);
            this.Interactable.Add(PlayerEnum.Stats.MaxHealth, this.IncMaxHealth);
            this.Updateables.Add(this.IncMaxHealth);
            this.RenderablesList.Add(this.IncMaxHealth);

            // Heal Interactable
            this.IncHeal = new CreateInteractable(PlayerEnum.Stats.Heal, 300, 100);
            this.Interactable.Add(PlayerEnum.Stats.Heal, this.IncHeal);
            this.Updateables.Add(this.IncHeal);
            this.RenderablesList.Add(this.IncHeal);

            // Defense Interactable
            this.IncDefense = new CreateInteractable(PlayerEnum.Stats.Defense, 400, 100);
            this.Interactable.Add(PlayerEnum.Stats.Defense, this.IncDefense);
            this.Updateables.Add(this.IncDefense);
            this.RenderablesList.Add(this.IncDefense);

            // AttackSpeed Interactable
            this.IncAttackSpeed = new CreateInteractable(PlayerEnum.Stats.AttackSpeed, 500, 100);
            this.Interactable.Add(PlayerEnum.Stats.AttackSpeed, this.IncAttackSpeed);
            this.Updateables.Add(this.IncAttackSpeed);
            this.RenderablesList.Add(this.IncAttackSpeed);

            // MovementSpeed Interactable
            this.IncMovementSpeed = new CreateInteractable(PlayerEnum.Stats.MovementSpeed, 600, 100);
            this.Interactable.Add(PlayerEnum.Stats.MovementSpeed, this.IncMovementSpeed);
            this.Updateables.Add(this.IncMovementSpeed);
            this.RenderablesList.Add(this.IncMovementSpeed);

            // Währung Interactable
            this.IncWährung = new CreateInteractable(PlayerEnum.Stats.Währung, 700, 100);
            this.Interactable.Add(PlayerEnum.Stats.Währung, this.IncWährung);
            this.Updateables.Add(this.IncWährung);
            this.RenderablesList.Add(this.IncWährung);

            return false;
        }
    }
}
