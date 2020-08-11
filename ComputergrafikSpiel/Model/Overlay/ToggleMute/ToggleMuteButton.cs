using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model.Character.Player;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
using OpenTK;
using OpenTK.Graphics;

namespace ComputergrafikSpiel.Model.Overlay.ToggleMute
{
    internal class ToggleMuteButton : IUpdateable
    {
        private static readonly Dictionary<PlayerEnum.Stats, ITileTexture> TextureLookup = GenerateDefault();
        private static readonly ITileTexture BackgroundTexture = new TextureLoader().LoadTileTexture("GUI/Buttons/SquareButton", (1, 2));
        private readonly List<GenericRenderable> backgroundTiles;
        private readonly List<GenericRenderable> foregroundTiles;
        private readonly ToggleMute parent;
        private readonly PlayerEnum.Stats toggleitem;
        private bool triggered = false;
        private Vector2 size;
        private bool isHovered = false;
        private bool clickReleasedAfterCreation = false; // This is needed so that buttons dont get clicked immediatedly.
        private Vector2 location;

        internal ToggleMuteButton(ToggleMute parent, PlayerEnum.Stats toggleitem)
        {
            this.parent = parent ?? throw new ArgumentNullException(nameof(parent));

            var (top, _, left, right) = Scene.Scene.Current.World.WorldSceneBounds; // bottom is ignored, as it is never needed
            this.location = new Vector2(right - 20, top - 20);

            float buttonSize = (right - left) * .025f;
            this.size = new Vector2(buttonSize, buttonSize);

            this.toggleitem = toggleitem;

            // Button Setup:
            var backgroundTileCount = (int)Math.Round(this.size.X / this.size.Y);

            this.backgroundTiles = new List<GenericRenderable>(backgroundTileCount);
            TextureCoordinates coords = BackgroundTexture.GetTexCoordsOfIndex(0);

            this.backgroundTiles.Add(new GenericRenderable
            {
                Coordinates = coords,
                Position = this.location,
                Scale = this.size,
                Tex = BackgroundTexture,
            });

            // Data
            this.foregroundTiles = new List<GenericRenderable>(1);

            var texCoords = TextureLookup[toggleitem].GetTexCoordsOfIndex(0);
            this.foregroundTiles.Add(new GenericRenderable
            {
                Scale = this.size / 2,
                Coordinates = texCoords,
                Position = this.location,
                Tex = TextureLookup[toggleitem],
            });
        }

        public IEnumerable<IRenderable> Foreground => this.foregroundTiles;

        public IEnumerable<IRenderable> Background => this.backgroundTiles;

        public (float top, float bottom, float left, float right) Bounds
        {
            get
            {
                var top = this.location.Y + this.size.Y;
                var bottom = this.location.Y - this.size.Y;
                var left = this.location.X - this.size.X;
                var right = this.location.X + this.size.X;
                return (top, bottom, left, right);
            }
        }

        public void Update(float dtime)
        {
            var inputState = Scene.Scene.Current.Model.InputState;

            if (inputState.MouseState.LeftButton == OpenTK.Input.ButtonState.Released)
            {
                this.clickReleasedAfterCreation = true;
            }

            if (this.IsInBounds(inputState))
            {
                if (!this.isHovered)
                {
                    this.ChangeStyle(true);
                }

                if (inputState.MouseState.LeftButton == OpenTK.Input.ButtonState.Pressed && this.clickReleasedAfterCreation)
                {
                    if (this.triggered == false)
                    {
                        this.triggered = true;

                        if (this.toggleitem.Equals(PlayerEnum.Stats.Mute))
                        {
                            Console.WriteLine("Pressed MUTE");
                            Scene.Scene.Current.Model.SceneManager.Play.MuteMusik();
                        }
                        else if (this.toggleitem.Equals(PlayerEnum.Stats.Unmute))
                        {
                            Console.WriteLine("Pressed UNMUTE");
                            Scene.Scene.Current.Model.SceneManager.Play.UnmuteMusik();
                        }

                        (Scene.Scene.Current.Model as Model).TriggerToggleMuteButton();
                    }
                }
            }
            else
            {
                if (this.isHovered)
                {
                    this.ChangeStyle(false);
                }
            }
        }

        private static Dictionary<PlayerEnum.Stats, ITileTexture> GenerateDefault()
        {
            const string root = "GUI/ToggleMute/";
            var texLoader = new TextureLoader();
            return new Dictionary<PlayerEnum.Stats, ITileTexture>
            {
                [PlayerEnum.Stats.Mute] = texLoader.LoadTileTexture(root + "Bell_mute", (1, 1)),
                [PlayerEnum.Stats.Unmute] = texLoader.LoadTileTexture(root + "Bell_unmute", (1, 1)),
            };
        }

        private void ChangeStyle(bool active)
        {
            this.parent.NeedsUpdate = true;
            this.isHovered = active;
            Vector2 shift = new Vector2(0, .5f);
            if (active)
            {
                shift *= -1;
            }

            foreach (var entry in this.backgroundTiles)
            {
                var old = entry.Coordinates;
                entry.Coordinates = new TextureCoordinates(old.TopLeft + shift, old.TopRight + shift, old.BottomRight + shift, old.BottomLeft + shift);
            }
        }

        private bool IsInBounds(IInputState inputState)
        {
            var (top, bottom, left, right) = this.Bounds;
            var mouseCoords = inputState.Cursor.WorldCoordinates ?? Vector2.Zero;

            return left <= mouseCoords.X && right >= mouseCoords.X && top >= mouseCoords.Y && bottom <= mouseCoords.Y;
        }

        private class GenericRenderable : IRenderableLayeredTextures
        {
            public IEnumerable<(Color4 color, Vector2[] vertices)> DebugData => null;

            public Vector2 Position { get; set; }

            public float Rotation => 0f;

            public Vector2 RotationAnker => this.Position;

            public Vector2 Scale { get; set; }

            public TextureCoordinates Coordinates { get; set; }

            public ITileTexture Tex { get; set; }

            public (IEnumerable<TextureCoordinates>, ITileTexture) Texture => (new TextureCoordinates[] { this.Coordinates }, this.Tex);

            ITexture IRenderable.Texture => this.Texture.Item2;
        }
    }
}