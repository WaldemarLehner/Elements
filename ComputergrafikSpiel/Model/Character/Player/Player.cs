using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.Character.Player.PlayerSystems;
using ComputergrafikSpiel.Model.Character.Weapon.Interfaces;
using ComputergrafikSpiel.Model.Collider;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.Entity.Particles;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
using ComputergrafikSpiel.Model.Overlay.UpgradeScreen;
using OpenTK;
using OpenTK.Graphics;

namespace ComputergrafikSpiel.Model.Character.Player
{
    public class Player : IPlayer, IRenderableLayeredTextures
    {
        private readonly List<PlayerEnum.PlayerActions> playerActionList;
        private readonly PlayerAttackSystem playerAttackSystem = new PlayerAttackSystem();
        private readonly PlayerMovementSystem playerMovementSystem = new PlayerMovementSystem();
        private readonly PlayerInteractionSystem playerInteractionSystem = new PlayerInteractionSystem();
        private readonly PlayerStateManager playerStateManager = new PlayerStateManager(PlayerStateOptions.Default);
        private readonly Vector2 scale;
        private readonly InputController inputController = new InputController(InputControllerSettings.Default);
        private readonly GenericParticleEmitter dirtEmitter;
        private readonly PlayerAnimationSystem playerAnimationSystem = new PlayerAnimationSystem(5);

        private bool run = false;
        private Vector2 directionXY = Vector2.Zero;
        private bool updateDisabled = false;
        private bool muteButtonOnScreen = false;

        public Player()
        {
            this.dirtEmitter = new GenericParticleEmitter(EmitParticleOnceOptions.Dirt, 0.1f);
            this.dirtEmitter.Disable();
            this.playerActionList = new List<PlayerEnum.PlayerActions>();
            this.Position = new Vector2(350, 200);
            this.scale = new Vector2(24, 24);
            this.Scale = this.scale;
            var collisionLayer = ColliderLayer.Layer.Bullet | ColliderLayer.Layer.Water | ColliderLayer.Layer.Wall | ColliderLayer.Layer.Interactable | ColliderLayer.Layer.Trigger;
            this.Collider = new CircleOffsetCollider(this, new Vector2(0, -15f), 10, ColliderLayer.Layer.Player, collisionLayer);
            this.TileTexture = new TextureLoader().LoadTileTexture("PlayerTile", (5, 1));
        }

        public int MaxHealth => (int)this.playerStateManager.Current.MaxHealth;

        public int CurrentHealth => (int)this.playerStateManager.Current.Health;

        public float BulletTTL => this.playerStateManager.Current.BulletTTL;

        public int BulletDamage => (int)this.playerStateManager.Current.BulletDamage;

        public float AttackSpeed => this.playerStateManager.Current.Firerate;

        public float AttackCooldown { get; } = -100;

        public float AttackCooldownCurrent { get; set; } = 0;

        public float DashCooldown { get; } = 4;

        public float DashCooldownCurrent { get; set; } = 0;

        public float MovementSpeed => 60f * this.playerStateManager.Current.MovementSpeed * this.playerMovementSystem.DashMultiplier;

        public int Money => (int)this.playerStateManager.Current.Currency;

        public Vector2 Position { get; set; } = Vector2.Zero;

        public Vector2 Scale { get; set; } = Vector2.One * 10;

        public float Rotation { get; } = 0f;

        public Vector2 RotationAnker { get; } = Vector2.Zero;

        public ITileTexture TileTexture { get; }

        public ITexture Texture => this.TileTexture as ITexture;

        public ICollider Collider { get; set; }

        public IEnumerable<(Color4 color, Vector2[] vertices)> DebugData => new (Color4 color, Vector2[] vertices)[] { this.Collider.DebugData };

        public IWeapon EquipedWeapon { get; private set; }

        public bool TextureWasMirrored { get; set; } = false;

        public Vector2 LastPosition { get; set; }

        public (int currentHealth, int maxHealth, int currency, float bulletTTL, int bulletDamage) PlayerData => (this.CurrentHealth, this.MaxHealth, this.Money, this.BulletTTL, this.BulletDamage);

        public float BloodColorHue => 0f;

        public bool Invulnerable { get; set; } = false;

        private TextureCoordinates CurrentTexCoords => TexturePointerCalculationHelper.GetCurrentTextureCoordinates(this.TileTexture, this.playerAnimationSystem.CurrentFrame) ?? TextureCoordinates.Error;

        (IEnumerable<TextureCoordinates>, ITileTexture) IRenderableLayeredTextures.Texture => (new TextureCoordinates[] { this.CurrentTexCoords }, this.TileTexture);

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
            if (this.directionXY.Length == 0f)
            {
                this.playerAnimationSystem.UpdateIsMoving(false);
            }
            else
            {
                this.playerAnimationSystem.UpdateIsMoving(true);
            }
            this.playerActionList.Clear();
        }

        public void TakingDamage(int damage)
        {
            if (this.updateDisabled == true)
            {
                return;
            }

            if (!this.Invulnerable)
            {
                this.playerStateManager.Hurt(damage);

                // Spawn particles
                EmitParticleOnceOptions opt = EmitParticleOnceOptions.ProjectileHit;
                opt.Count = 25;
                opt.PointOfEmmision = this.Position;
                opt.Direction = Vector2.One;
                opt.DirectionDeviation = 180;
                opt.Hue = (this.BloodColorHue, this.BloodColorHue);
                StaticParticleEmmiter.EmitOnce(opt);
            }
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
            // Setzt Mutebutton einmalig auf den Screen zu Beginn
            if (!this.muteButtonOnScreen)
            {
                (Scene.Scene.Current.Model as Model).TriggerToggleMuteButton();
                this.muteButtonOnScreen = true;
            }

            if (this.updateDisabled)
            {
                return;
            }

            if (this.CurrentHealth <= 0 || (Scene.Scene.Current.NpcList.Count == 0 && ((Scene.Scene.Current.Model as Model).SceneManager.CurrentStageLevel == 1)))
            {
                Scene.Scene.Current.Model.SceneManager.Play.StartGameoverMusic();
                (Scene.Scene.Current.Model as Model).TriggerEndscreenButtons();
                this.updateDisabled = true;
            }


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

            this.playerAnimationSystem.Update(dtime);

            Scene.Scene.Current.ColliderManager.HandleTriggerCollisions(this);
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

                var colliderPosition = CollisionPushbackHelper.PushbackCollider(this, collision);
                var colliderOffset = this.Position - this.Collider.Position;

                this.Position = colliderPosition + colliderOffset;

                if (Scene.Scene.Current.ColliderManager.GetCollisions(this).Count > 0)
                {
                    // Fall back. The Pushback pushed the player onto another collider. We go back to the last position instead to be safe.
                    this.Position = this.LastPosition;
                }

                return;
            }
        }

        public IList<UpgradeOption> GetOptions(uint level) => this.playerStateManager.GetUpgradeOptions(level);

        public void SelectOption(PlayerEnum.Stats stat, uint level)
        {
            this.playerStateManager.Apply(stat, level);
        }

        public void ChangePosition()
        {
            // Change Position to Left Door
            this.Position = new Vector2(55, 272);
        }

        private void HandlePlayerAction(IInputState inputState, PlayerEnum.PlayerActions playerAction)
        {
            if (playerAction == PlayerEnum.PlayerActions.MoveUp || playerAction == PlayerEnum.PlayerActions.MoveDown || playerAction == PlayerEnum.PlayerActions.MoveLeft || playerAction == PlayerEnum.PlayerActions.MoveRight)
            {
                this.playerActionList.Add(playerAction);
                this.playerInteractionSystem.PlayerInteraction(this);
            }
            else if (playerAction == PlayerEnum.PlayerActions.Attack)
            {
                if (this.EquipedWeapon != null && this.AttackCooldownCurrent <= 0)
                {
                    this.playerAttackSystem.PlayerAttack(Scene.Scene.Current?.Model?.InputState?.Cursor?.WorldCoordinates ?? Vector2.Zero);
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