using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.Character.Player.PlayerSystems;
using ComputergrafikSpiel.Model.Character.Weapon.Interfaces;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
using OpenTK;
using OpenTK.Graphics;

namespace ComputergrafikSpiel.Model.Character.Player
{
    public class Player : IPlayer
    {
        private readonly List<PlayerEnum.PlayerActions> playerActionList;
        private readonly PlayerAttackSystem playerAttackSystem;
        private readonly PlayerMovementSystem playerMovementSystem;
        private readonly PlayerInteractionSystem playerInteractionSystem;
        private readonly Vector2 scale;
        private readonly InputController inputController;

        private bool run = false;
        private Vector2 directionXY = Vector2.Zero;

        public Player()
        {
            this.CurrentHealth = this.MaxHealth;
            this.playerActionList = new List<PlayerEnum.PlayerActions>();
            this.Position = new Vector2(50, 65);
            this.scale = new Vector2(24, 24);
            this.Scale = this.scale;
            var collisionLayer = ColliderLayer.Layer.Bullet | ColliderLayer.Layer.Enemy | ColliderLayer.Layer.Water | ColliderLayer.Layer.Wall | ColliderLayer.Layer.Interactable | ColliderLayer.Layer.Trigger;
            this.Collider = new CircleOffsetCollider(this, new Vector2(0, -15f), 10, ColliderLayer.Layer.Player, collisionLayer);
            this.playerAttackSystem = new PlayerAttackSystem();
            this.playerMovementSystem = new PlayerMovementSystem();
            this.playerInteractionSystem = new PlayerInteractionSystem();
            this.Texture = new TextureLoader().LoadTexture("PlayerWeapon");
            this.AttackCooldownCurrent = 0;
            this.DashCooldownCurrent = 0;
            this.inputController = new InputController(InputControllerSettings.Default);
        }

        // Define Player
        public event EventHandler CharacterDeath;

        public event EventHandler CharacterHit;

        public event EventHandler CharacterMove;

        public event EventHandler PlayerInc;

        public int MaxHealth { get; set; } = 5;

        public int CurrentHealth { get; set; }

        public int Defense { get; set; } = 0;

        public float AttackSpeed { get; set; } = 1;

        public float AttackCooldown { get; } = 100;

        public float AttackCooldownCurrent { get; set; }

        public float DashCooldown { get; } = 4;

        public float DashCooldownCurrent { get; set; }

        public float MovementSpeed { get; set; } = 100;

        public int Money { get; set; } = 0;

        public Vector2 Position { get; set; } = Vector2.Zero;

        public Vector2 Scale { get; set; } = Vector2.One * 10;

        public float Rotation { get; } = 0f;

        public Vector2 RotationAnker { get; } = Vector2.Zero;

        public ITexture Texture { get; }

        public ICollider Collider { get; set; }

        public IEnumerable<(Color4 color, Vector2[] vertices)> DebugData => new (Color4 color, Vector2[] vertices)[] { this.Collider.DebugData };

        public IWeapon EquipedWeapon { get; private set; }

        public bool TextureWasMirrored { get; set; } = false;

        public Vector2 LastPosition { get; set; }

        public (int currentHealth, int maxHealth, int currency) PlayerData => (this.CurrentHealth, this.MaxHealth, this.Money);

        // Look wich action was handed over and call corresponding method
        public void PlayerControl()
        {
            var inputState = Scene.Scene.Current.Model.InputState;

            if (inputState == null)
            {
                return;
            }

            IEnumerable<PlayerEnum.PlayerActions> actions = this.inputController.GetActions(inputState);

            foreach (PlayerEnum.PlayerActions playerAction in actions)
            {
                this.HandlePlayerAction(inputState, playerAction);
            }

            this.directionXY = this.playerMovementSystem.SetPlayerDirection(this.playerActionList);
            this.playerActionList.Clear();
        }

        // Needs EventHandler from Npc who hits player
        public void TakingDamage(int damage)
        {
            if (damage <= 0)
            {
                throw new View.Exceptions.ArgumentNotPositiveIntegerGreaterZeroException(nameof(damage));
            }

            if (this.Defense < damage)
            {
                damage -= this.Defense;
                this.CurrentHealth -= damage;
                this.OnHit(EventArgs.Empty);
            }

            if (this.CurrentHealth <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("CurrentHealth is under 0 -- Player died");
                this.OnDeath(EventArgs.Empty);
            }
        }

        public void IncreasePlayerStats(int incNumber, PlayerEnum.Stats stats)
        {
            if (incNumber <= 0)
            {
                throw new View.Exceptions.ArgumentNotPositiveIntegerGreaterZeroException(nameof(incNumber));
            }

            /*Interactable MaxHealth: Bei Rundenende kann das Maximalleben um eins erhöht werden
            -> Überprüfung das bisheriges Leben auch auf Max ist*/
            if (stats == PlayerEnum.Stats.MaxHealth)
            {
                    this.MaxHealth += incNumber;
            }

            // Leben wird um eins erhöht
            else if (stats == PlayerEnum.Stats.Heal && this.CurrentHealth < this.MaxHealth)
            {
                    this.CurrentHealth += incNumber;
            }

            // Defense wird erhöht
            else if (stats == PlayerEnum.Stats.Defense)
            {
                this.Defense += incNumber;
            }

            // AttackSpeed wird erhöht
            else if (stats == PlayerEnum.Stats.AttackSpeed)
            {
                this.AttackSpeed += incNumber;
            }

            // MovementSpeed wird erhöht
            else if (stats == PlayerEnum.Stats.MovementSpeed)
            {
                this.MovementSpeed += incNumber;
            }

            // Währung wird an der Stelle gespawnt, an der der Gegner gestorben ist
            else if (stats == PlayerEnum.Stats.Money)
            {
                this.Money += incNumber;
            }

            this.OnInc(EventArgs.Empty);
        }

        public void Update(float dtime)
        {
            this.LastPosition = this.Position;
            if (Scene.Scene.Current.Model.InputState != null)
            {
                this.PlayerControl();
                this.LookAt(Scene.Scene.Current.Model.InputState.Cursor.WorldCoordinates ?? Vector2.Zero);
            }

            if (this.run)
            {
                this.Position += this.directionXY * this.MovementSpeed * dtime / 2;
                this.run = false;

                // Dient nur zu Testzwecken
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("Maximales Leben: {0} Aktuelles Leben: {1} Verteidigung: {2}  Angriffsgeschwindigkeit: {3}  Bewegungsgeschwindigkeit: {4}  Währung(Coins): {5}\n", this.MaxHealth, this.CurrentHealth, this.Defense, this.AttackSpeed, this.MovementSpeed, this.Money);
            }

            this.Position += this.directionXY * this.MovementSpeed * dtime;

            this.directionXY = Vector2.Zero;

            this.CollisionPrevention();

            this.AttackCooldownCurrent -= dtime + this.AttackSpeed;

            this.DashCooldownCurrent -= dtime;

            Scene.Scene.Current.ColliderManager.HandleTriggerCollisions(this);
        }

        public void OnInc(EventArgs e)
        {
            this.PlayerInc?.Invoke(this, e);
        }

        public void OnDeath(EventArgs e)
        {
            this.CharacterDeath?.Invoke(this, e);
        }

        public void OnHit(EventArgs e)
        {
            this.CharacterHit?.Invoke(this, e);
        }

        public void OnMove(EventArgs e)
        {
            this.CharacterMove?.Invoke(this, e);
        }

        public void Equip(Weapon.Weapon weapon)
        {
            this.EquipedWeapon = weapon;
        }

        public void LookAt(Vector2 vec) => this.Scale = (this.Position.X > vec.X) ? this.Scale = this.scale * new Vector2(-1, 1) : this.scale;

        public void CollisionPrevention()
        {
            IReadOnlyCollection<ICollidable> collisions = Scene.Scene.Current.ColliderManager.GetCollisions(this);

            foreach (ICollidable collision in collisions)
            {
                if (collision.Collider.OwnLayer == ColliderLayer.Layer.Interactable || collision.Collider.OwnLayer == ColliderLayer.Layer.Bullet)
                {
                    continue;
                }

                this.Position = this.LastPosition;
                return;
            }
        }

        private void HandlePlayerAction(IInputState inputState, PlayerEnum.PlayerActions playerAction)
        {
            if (playerAction == PlayerEnum.PlayerActions.MoveUp || playerAction == PlayerEnum.PlayerActions.MoveDown || playerAction == PlayerEnum.PlayerActions.MoveLeft || playerAction == PlayerEnum.PlayerActions.MoveRight)
            {
                this.playerActionList.Add(playerAction);
                this.OnMove(EventArgs.Empty);
            }
            else if (playerAction == PlayerEnum.PlayerActions.Attack)
            {
                if (this.EquipedWeapon != null && this.AttackCooldownCurrent <= 0 && !Scene.Scene.Current.LockPlayerAttack)
                {
                    this.playerAttackSystem.PlayerAttack(inputState.Cursor.WorldCoordinates ?? Vector2.Zero);
                    this.AttackCooldownCurrent = this.AttackCooldown;
                }
            }
            else if (playerAction == PlayerEnum.PlayerActions.Interaction)
            {
                this.playerInteractionSystem.PlayerInteraction(this);
            }
            else if (playerAction == PlayerEnum.PlayerActions.Run)
            {
                this.run = true;
            }
            else if (playerAction == PlayerEnum.PlayerActions.Dash)
            {
                if (this.DashCooldownCurrent <= 0)
                {
                    this.playerMovementSystem.PlayerDash(this);
                    this.DashCooldownCurrent = this.DashCooldown;
                }
            }
        }
    }
}