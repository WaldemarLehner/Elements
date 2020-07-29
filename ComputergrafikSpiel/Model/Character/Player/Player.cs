using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.Character.Player.PlayerSystems;
using ComputergrafikSpiel.Model.Character.Weapon.Interfaces;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.Entity.Particles;
using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
using ComputergrafikSpiel.Model.Overlay.UpgradeScreen;
using OpenTK;
using OpenTK.Graphics;

namespace ComputergrafikSpiel.Model.Character.Player
{
    public class Player : IPlayer
    {
        private readonly List<PlayerEnum.PlayerActions> playerActionList;
        private readonly PlayerAttackSystem playerAttackSystem = new PlayerAttackSystem();
        private readonly PlayerMovementSystem playerMovementSystem = new PlayerMovementSystem();
        private readonly PlayerInteractionSystem playerInteractionSystem = new PlayerInteractionSystem();
        private readonly PlayerStateManager playerStateManager = new PlayerStateManager(PlayerStateOptions.Default);
        private readonly Vector2 scale;
        private readonly InputController inputController = new InputController(InputControllerSettings.Default);
        private readonly GenericParticleEmitter dirtEmitter;

        private bool run = false;
        private Vector2 directionXY = Vector2.Zero;

        public Player()
        {
            this.dirtEmitter = new GenericParticleEmitter(EmitParticleOnceOptions.Dirt, 0.1f);
            this.dirtEmitter.Disable();
            this.playerActionList = new List<PlayerEnum.PlayerActions>();
            this.Position = new Vector2(50, 65);
            this.scale = new Vector2(24, 24);
            this.Scale = this.scale;
            var collisionLayer = ColliderLayer.Layer.Bullet | ColliderLayer.Layer.Enemy | ColliderLayer.Layer.Water | ColliderLayer.Layer.Wall | ColliderLayer.Layer.Interactable | ColliderLayer.Layer.Trigger;
            this.Collider = new CircleOffsetCollider(this, new Vector2(0, -15f), 10, ColliderLayer.Layer.Player, collisionLayer);
            this.Texture = new TextureLoader().LoadTexture("PlayerWeapon");
        }

        // Define Player
        public event EventHandler CharacterDeath;

        public event EventHandler CharacterHit;

        public event EventHandler CharacterMove;

        public event EventHandler PlayerInc;

        public int MaxHealth => (int)this.playerStateManager.Current.MaxHealth;

        public int CurrentHealth => (int)this.playerStateManager.Current.Health;

        [Obsolete]
        public int Defense { get; set; } = 0;

        public float AttackSpeed => this.playerStateManager.Current.Firerate;

        public float AttackCooldown { get; } = 20;

        public float AttackCooldownCurrent { get; set; } = 0;

        public float DashCooldown { get; } = 4;

        public float DashCooldownCurrent { get; set; } = 0;

        public float MovementSpeed => 60f * this.playerStateManager.Current.MovementSpeed * this.playerMovementSystem.DashMultiplier;

        public int Money => (int)this.playerStateManager.Current.Currency;

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

        public float BloodColorHue => 0f;

        // Look wich action was handed over and call corresponding method
        public void PlayerControl()
        {
            var inputState = Scene.Scene.Current.Model.InputState;

            if (inputState == null || Scene.Scene.Current.Model.UpgradeScreen != null || !inputState.IsWindowFocused)
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
        public void TakingDamage()
        {
            bool died = false;

            this.playerStateManager.Hurt(ref died);

            if (died)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("CurrentHealth is under 0 -- Player died");
                this.OnDeath(EventArgs.Empty);
            }

            // Spawn particles
            EmitParticleOnceOptions opt = EmitParticleOnceOptions.ProjectileHit;
            opt.Count = 25;
            opt.PointOfEmmision = this.Position;
            opt.Direction = Vector2.One;
            opt.DirectionDeviation = 180;
            opt.Hue = (this.BloodColorHue, this.BloodColorHue);
            StaticParticleEmmiter.EmitOnce(opt);

        }

        public void TakeHeal()
        {
            this.playerStateManager.Heal();
        }

        public void TakeMoney()
        {
            this.playerStateManager.AddCoin(1);
        }

        public void Update(float dtime)
        {
            this.LastPosition = this.Position;
            if (Scene.Scene.Current.Model.InputState != null)
            {
                this.PlayerControl();
                this.LookAt(Scene.Scene.Current.Model.InputState.Cursor.WorldCoordinates ?? Vector2.Zero);
            }

            var rand = new Random();
            var opt = this.dirtEmitter.Options;
            opt.PointOfEmmision = this.Collider.Position + ((new Vector2((float)rand.NextDouble(), (float)rand.NextDouble()) - (Vector2.One * .5f)) * 5f);
            this.dirtEmitter.Options = opt;

            if (this.run || this.playerMovementSystem.DashMultiplier > 1)
            {
                this.dirtEmitter.Enable();
            }
            else
            {
                this.dirtEmitter.Disable();
            }

            if (this.run)
            {
                this.Position += this.directionXY * this.MovementSpeed * dtime / 4;
                this.run = false;
            }


            this.dirtEmitter.Update(dtime);

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

        public IList<UpgradeOption> GetOptions(uint level) => this.playerStateManager.GetUpgradeOptions(level);

        public void SelectOption(PlayerEnum.Stats stat, uint level)
        {
            this.playerStateManager.Apply(stat, level);
        }

        private void HandlePlayerAction(IInputState inputState, PlayerEnum.PlayerActions playerAction)
        {
            if (playerAction == PlayerEnum.PlayerActions.MoveUp || playerAction == PlayerEnum.PlayerActions.MoveDown || playerAction == PlayerEnum.PlayerActions.MoveLeft || playerAction == PlayerEnum.PlayerActions.MoveRight)
            {
                this.playerActionList.Add(playerAction);
                this.OnMove(EventArgs.Empty);
                this.playerInteractionSystem.PlayerInteraction(this);
            }
            else if (playerAction == PlayerEnum.PlayerActions.Attack)
            {
                if (this.EquipedWeapon != null && this.AttackCooldownCurrent <= 0)
                {
                    this.playerAttackSystem.PlayerAttack(inputState.Cursor.WorldCoordinates ?? Vector2.Zero);
                    this.AttackCooldownCurrent = this.AttackCooldown;
                }
            }
            else if (playerAction == PlayerEnum.PlayerActions.Interaction)
            {
                // this.playerInteractionSystem.PlayerInteraction(this);
            }
            else if (playerAction == PlayerEnum.PlayerActions.Run)
            {
                this.run = true;
            }
            else if (playerAction == PlayerEnum.PlayerActions.Dash)
            {
                if (this.DashCooldownCurrent <= 0)
                {
                    this.playerMovementSystem.PlayerDash();
                    this.DashCooldownCurrent = this.DashCooldown;
                }
            }
        }
    }
}