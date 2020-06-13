using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Text;
using ComputergrafikSpiel.Model.Character.Player;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.Character.Weapon;
using ComputergrafikSpiel.Model.Character.Weapon.Interfaces;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.Entity;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model
{
    internal class Model : IModel
    {
        // temporary
        private IColliderManager colliderManager;
        private IWeapon weapon;

        internal Model()
        {
            this.RenderablesList = new List<IRenderable>();
            this.Updateables = new List<IUpdateable>();
            this.Interactable = new Dictionary<PlayerEnum.Stats, IEntity>();
            this.colliderManager = new ColliderManager(32);
        }

        public IEnumerable<IRenderable> Renderables => this.RenderablesList;

        public (float top, float bottom, float left, float right) CurrentSceneBounds => (500, 0, 0, 800);

        public IEnumerable<IUiRenderable> UiRenderables { get; } = new List<IUiRenderable>();

        private List<IRenderable> RenderablesList { get; } = new List<IRenderable>();

        private List<IUpdateable> Updateables { get; } = new List<IUpdateable>();

        private IPlayer Player { get; set; } = null;

        private IEntity IncMovementSpeed { get; set; } = null;

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
                this.weapon = new Weapon(3, 1, 20, 2, this.colliderManager, 1, this);
                this.Player = new Player(this.Interactable, this.colliderManager, this.weapon);
                controller.HookPlayer(this.Player);
                this.Updateables.Add(this.Player);
                this.RenderablesList.Add(this.Player);
                return true;
            }

            return false;
        }

        public bool CreateTestInteractable()
        {
            if (this.IncMovementSpeed == null)
            {
                this.IncMovementSpeed = new TestInteractable();
                this.Interactable.Add(PlayerEnum.Stats.MovementSpeed, this.IncMovementSpeed);
                this.Updateables.Add(this.IncMovementSpeed);
                this.RenderablesList.Add(this.IncMovementSpeed);
                return true;
            }

            return false;
        }

        public void CreateProjectile(int projectileCreationCount, Vector2 position, Vector2 direction, int bulletTTL, float bulletSize, IColliderManager colliderManager)
        {
            for (int i = 0; i <= projectileCreationCount; i++)
            {
                Projectile projectile = new Projectile(position, direction, bulletTTL, bulletSize, colliderManager);
                this.Updateables.Add(projectile);
                this.RenderablesList.Add(projectile);
            }
        }
    }
}
