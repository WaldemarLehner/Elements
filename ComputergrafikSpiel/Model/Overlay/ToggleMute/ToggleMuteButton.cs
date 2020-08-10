using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model.Character.Player;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
using ComputergrafikSpiel.Model.Scene;
using OpenTK;
using OpenTK.Graphics;

namespace ComputergrafikSpiel.Model.Overlay.ToggleMute
{
    internal class ToggleMuteButton : IUpdateable
    {
        private static readonly Dictionary<PlayerEnum.Stats, ITileTexture> TextureLookup = GenerateDefault();
        private static readonly IMappedTileFont Font = new TextureLoader().LoadFontTexture("Font/vt323", (x: 8, y: 8), FontTextureMappingHelper.Default);
        private static readonly ITileTexture BackgroundTexture = new TextureLoader().LoadTileTexture("GUI/Buttons/Button", (3, 2));
        private readonly List<GenericRenderable> backgroundTiles;
        private readonly List<GenericRenderable> foregroundTiles;
        private readonly ToggleMute parent;
        private bool triggered = false;
        private Vector2 size;
        private PlayerEnum.Stats toggleitem;
        private bool isHovered = false;
        private bool clickReleasedAfterCreation = false; // This is needed so that buttons dont get clicked immediatedly.
        private Vector2 centre;

        internal ToggleMuteButton(ToggleMute parent, Vector2 centre, Vector2 buttonSize, PlayerEnum.Stats toggleitem)
        {
            this.parent = parent ?? throw new ArgumentNullException(nameof(parent));
            this.centre = centre;
            this.size = buttonSize;
            this.toggleitem = toggleitem;

            // Button Setup:
            var backgroundTileCount = (int)Math.Round(buttonSize.X / buttonSize.Y);
            float foregroundEntrySize = (buttonSize.Y < buttonSize.X) ? buttonSize.Y : buttonSize.X;

            this.backgroundTiles = new List<GenericRenderable>(backgroundTileCount);

            // Background Tiles
            for (int i = 0; i < backgroundTileCount; i++)
            {
                float x = this.centre.X - (this.size.X / 2f) + ((i + .5f) * this.size.Y);

                Vector2 center = new Vector2(x, this.centre.Y);
                TextureCoordinates coords;
                if (i == 0)
                {
                    coords = BackgroundTexture.GetTexCoordsOfIndex(0);
                }
                else if (i == backgroundTileCount - 1)
                {
                    coords = BackgroundTexture.GetTexCoordsOfIndex(2);
                }
                else
                {
                    coords = BackgroundTexture.GetTexCoordsOfIndex(1);
                }

                this.backgroundTiles.Add(new GenericRenderable
                {
                    Coordinates = coords,
                    Position = center,
                    Scale = Vector2.One * buttonSize.Y / 2f,
                    Tex = BackgroundTexture,
                });
            }

            // Data
            this.foregroundTiles = new List<GenericRenderable>(1);
            for (int i = 0; i < 1; i++)
            {
                float leftcentreBound = this.centre.X - (this.size.X / 3.5f) + this.size.Y;
                float rightcentreBound = this.centre.X + (this.size.X / 1.5f) - (this.size.Y / 0.3f);
                float x = leftcentreBound + ((rightcentreBound - leftcentreBound) * i);
                Vector2 center = new Vector2(x, this.centre.Y);
                var scale = foregroundEntrySize / 8f * Vector2.One;

                /*char c = text[i];

                if (!Font.MappedPositions.ContainsKey(c))
                {
                    continue;
                }*/

                var texCoords = TextureLookup[toggleitem].GetTexCoordsOfIndex(0);
                this.foregroundTiles.Add(new GenericRenderable
                {
                    Scale = scale,
                    Coordinates = texCoords,
                    Position = center,
                    Tex = TextureLookup[toggleitem],
                });
            }
        }

        public IEnumerable<IRenderable> Foreground => this.foregroundTiles;

        public IEnumerable<IRenderable> Background => this.backgroundTiles;

        public (float top, float bottom, float left, float right) Bounds
        {
            get
            {
                var top = this.centre.Y + (this.size.Y / 2f);
                var bottom = this.centre.Y - (this.size.Y / 2f);
                var left = this.centre.X - (this.size.X / 2f);
                var right = this.centre.X + (this.size.X / 2f);
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
                            // Scene.Scene.Current.Model.ToggleMute = null;
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