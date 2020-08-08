using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model.Character.Player;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
using OpenTK;
using OpenTK.Graphics;

namespace ComputergrafikSpiel.Model.Overlay.UpgradeScreen
{
    internal class UpgradeScreenButton : IUpdateable
    {
        private static readonly Dictionary<PlayerEnum.Stats, ITileTexture> TextureLookup = UpgradeScreenButtonTextureLookupGenerator.Default;
        private static readonly IMappedTileFont Font = new TextureLoader().LoadFontTexture("Font/vt323", (x: 8, y: 8), FontTextureMappingHelper.Default);
        private static readonly ITileTexture BackgroundTexture = new TextureLoader().LoadTileTexture("GUI/Buttons/Button", (3, 2));
        private readonly List<GenericRenderable> backgroundTiles;
        private readonly List<GenericRenderable> foregroundTiles;
        private readonly UpgradeScreen parent;
        private readonly Action<PlayerEnum.Stats> callback;
        private readonly PlayerEnum.Stats stat;

        private bool triggered = false;
        private Vector2 size;
        private bool isHovered = false;
        private bool clickReleasedAfterCreation = false; // This is needed so that buttons dont get clicked immediatedly.
        private Vector2 centre;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpgradeScreenButton"/> class.
        /// Create a new button.
        /// </summary>
        /// <param name="parent">The UpdateScreen the button belongs to.</param>
        /// <param name="centre">The Centre of the button.</param>
        /// <param name="upgradeOption">Data for the button.</param>
        /// <param name="contentWidth">The width of the inner part of the button. This is to make all buttons the same width.</param>
        /// <param name="onClick">Callback to be triggered when the button is clicked.</param>
        /// <param name="buttonSize">Buttons size in World Coordinates.</param>
        internal UpgradeScreenButton(UpgradeScreen parent, Vector2 centre, UpgradeOption upgradeOption, Vector2 buttonSize, Action<PlayerEnum.Stats> onClick)
        {
            this.parent = parent ?? throw new ArgumentNullException(nameof(parent));
            this.centre = centre;
            this.callback = onClick ?? throw new ArgumentNullException(nameof(onClick));
            this.size = buttonSize;
            this.stat = upgradeOption.Stat;
            string mainText = UpgradeScreenButtonTextureLookupGenerator.MainText(upgradeOption);
            string priceText = upgradeOption.Price.ToString();

            // Button Setup:
            // Icon Name ValueOld + Change > ValueNew MoneyIconSmall Price
            int foregroundTileCount = 1 + mainText.Length + 1 + priceText.Length;
            var backgroundTileCount = (int)Math.Round(buttonSize.X / buttonSize.Y);
            float foregroundEntrySize = (buttonSize.Y < (buttonSize.X / foregroundTileCount)) ? buttonSize.Y : (buttonSize.X / foregroundTileCount);

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
            this.foregroundTiles = new List<GenericRenderable>(foregroundTileCount);
            for (int i = 0; i < foregroundTileCount; i++)
            {
                float leftcentreBound = this.centre.X - (this.size.X / 2f) + (this.size.Y / 2f);
                float rightcentreBound = this.centre.X + (this.size.X / 2f) - (this.size.Y / 2f);
                float x = leftcentreBound + ((rightcentreBound - leftcentreBound) * (i / (float)foregroundTileCount));
                Vector2 center = new Vector2(x, this.centre.Y);
                var scale = foregroundEntrySize / 2f * Vector2.One;

                if (i == 0)
                {
                    // Icon.
                    this.foregroundTiles.Add(new GenericRenderable
                    {
                        Scale = scale,
                        Coordinates = TextureCoordinates.Default,
                        Position = center,
                        Tex = TextureLookup[upgradeOption.Stat],
                    });
                }
                else if (i <= mainText.Length)
                {
                    char c = mainText[i - 1];
                    if (!Font.MappedPositions.ContainsKey(c))
                    {
                        continue;
                    }

                    var texCoords = Font.GetTexCoordsOfIndex(Font.MappedPositions[c]);
                    this.foregroundTiles.Add(new GenericRenderable
                    {
                        Scale = scale,
                        Coordinates = texCoords,
                        Position = center,
                        Tex = Font,
                    });
                }
                else if (i == mainText.Length + 1)
                {
                    // Cost Icon.
                    this.foregroundTiles.Add(new GenericRenderable
                    {
                        Scale = scale * .5f,
                        Coordinates = TextureCoordinates.Default,
                        Position = center,
                        Tex = TextureLookup[PlayerEnum.Stats.Money],
                    });
                }
                else
                {
                    char c = priceText[i - (mainText.Length + 2)];
                    if (!Font.MappedPositions.ContainsKey(c))
                    {
                        continue;
                    }

                    var texCoords = Font.GetTexCoordsOfIndex(Font.MappedPositions[c]);
                    this.foregroundTiles.Add(new GenericRenderable
                    {
                        Scale = scale,
                        Coordinates = texCoords,
                        Position = center,
                        Tex = Font,
                    });
                }
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
                        this.callback(this.stat);
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