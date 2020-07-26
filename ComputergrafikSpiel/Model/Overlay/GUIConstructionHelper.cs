using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ComputergrafikSpiel.Controller.Input;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using ComputergrafikSpiel.Model.World;
using OpenTK;
using OpenTK.Graphics;
using MouseCursor = ComputergrafikSpiel.Controller.Input.MouseCursor;

namespace ComputergrafikSpiel.Model.Overlay
{
    internal static class GUIConstructionHelper
    {
        private static readonly IMappedTileFont Font = new TextureLoader().LoadFontTexture("Font/vt323", (x: 8, y: 8), FontTextureMappingHelper.Default);
        private static readonly ITileTexture Heart = new TextureLoader().LoadTileTexture("GUI/Heart", (x: 2, y: 1));
        // private static readonly ITileTexture Crosshair = new TextureLoader().LoadTileTexture("GUI/Crosshair_Cursor", (x: 10, y: 10));
        private static readonly ITileTexture Gameover = new TextureLoader().LoadTileTexture("GUI/gameover", (x: 10, y: 10));

        internal static IEnumerable<IRenderable> GenerateGuiIndicator(IWorldScene sceneDefinition, IPlayer player)
        {
            var coinData = GUIConstructionHelper.GenerateCoinCount(sceneDefinition, player);
            var healthbar = GUIConstructionHelper.GenerateHealthBar(sceneDefinition, player);
            // var crosshair = GUIConstructionHelper.GenerateCrosshair(sceneDefinition, player);
            var gameover = GUIConstructionHelper.GenerateGameover(sceneDefinition, player);

            var renderables = new List<IRenderable>();
            renderables.AddRange(coinData);
            renderables.AddRange(healthbar);
            // renderables.AddRange(crosshair);
            renderables.AddRange(gameover);
            return renderables;
        }

        private static List<IRenderable> GenerateCoinCount(IWorldScene sceneDefinition, IPlayer player)
        {
            // Get bounds of GUI Area of Scene.
            float left = sceneDefinition.WorldSceneBounds.right - (sceneDefinition.SceneDefinition.TileSize * 3);
            float right = sceneDefinition.WorldSceneBounds.right;
            float top = sceneDefinition.WorldSceneBounds.top;
            float bottom = top - sceneDefinition.SceneDefinition.TileSize;

            float width = right - left;
            float height = top - bottom;

            string moneyCount = player.PlayerData.currency.ToString() + "$";
            int renderablesCount = moneyCount.Length;
            float itemSize = (width / renderablesCount) < height ? width / renderablesCount : height;

            List<IRenderable> renderables = new List<IRenderable>();

            for (int i = 0; i < renderablesCount; i++)
            {
                var tex = Font;
                int texIndex;
                {
                    var (x, y) = tex.GetTileOfKey(moneyCount[i]);
                    if (x == -1 && y == -1)
                    {
                        continue;
                    }

                    texIndex = (y * tex.XRows) + x;
                }

                var position = new Vector2(left + ((i + .5f) * itemSize), (top + bottom) / 2f);
                var scale = Vector2.One * itemSize / 2f;
                var entry = new GenericGUIRenderable()
                {
                    Scale = scale,
                    Position = position,
                    Texture = tex,
                    Coordinates = tex.GetTexCoordsOfIndex(texIndex),
                };
                renderables.Add(entry);
            }

            return renderables;
        }

        private static List<IRenderable> GenerateHealthBar(IWorldScene sceneDefinition, IPlayer player)
        {
            List<IRenderable> healthEntries = new List<IRenderable>();
            var (currentHealth, maxHealth, _) = player.PlayerData;

            // Get bounds of GUI Area of Scene.
            float left = sceneDefinition.WorldSceneBounds.left;
            float right = sceneDefinition.WorldSceneBounds.right - (sceneDefinition.SceneDefinition.TileSize * 3);
            float top = sceneDefinition.WorldSceneBounds.top;
            float bottom = top - sceneDefinition.SceneDefinition.TileSize;

            float heartSize = (right - left) / maxHealth;
            if (heartSize < 0)
            {
                heartSize *= -1;
            }

            if (heartSize > top - bottom)
            {
                heartSize = top - bottom; ///////////
            }

            for (int i = 0; i < maxHealth; i++)
            {
                float xCenter = left + ((i + .5f) * heartSize);
                var texCoords = Heart.GetTexCoordsOfIndex((i >= currentHealth) ? 0 : 1); // Get the required texture.

                var entry = new GenericGUIRenderable()
                {
                    Scale = Vector2.One * .5f * heartSize,
                    Position = new Vector2(xCenter, .5f * (top + bottom)),
                    Texture = Heart,
                    Coordinates = texCoords,
                };
                healthEntries.Add(entry);
            }

            return healthEntries;
        }

        /*private static List<IRenderable> GenerateCrosshair(IWorldScene sceneDefinition, IPlayer player)
        {
            List<IRenderable> crosshairEntries = new List<IRenderable>();
            MouseCursor mouseCursor = new MouseCursor();
            var (currentHealth, maxHealth, _) = player.PlayerData;

            // Get bounds of GUI Area of Scene.
            float left = sceneDefinition.WorldSceneBounds.left;
            float right = sceneDefinition.WorldSceneBounds.right - (sceneDefinition.SceneDefinition.TileSize * 3);
            float top = sceneDefinition.WorldSceneBounds.top;
            float bottom = top - sceneDefinition.SceneDefinition.TileSize;

            float heartSize = (right - left) / maxHealth;
            if (heartSize < 0)
            {
                heartSize *= -1;
            }

            if (heartSize > top - bottom)
            {
                heartSize = top - bottom; ///////////
            }

            for (int i = 0; i < 1; i++)
            {
                float xCenter = (left + right) / 2;
                var texCoords = Heart.GetTexCoordsOfIndex(2); // Get the required texture.

                var entry = new GenericGUIRenderable()
                {
                    Scale = Vector2.One * .5f * heartSize,
                    Position = new Vector2(xCenter, .5f * (top + bottom)),
                    Texture = Crosshair,
                    Coordinates = texCoords,
                };
                crosshairEntries.Add(entry);
            }

            return crosshairEntries;
        }*/

        private static List<IRenderable> GenerateGameover(IWorldScene sceneDefinition, IPlayer player)
        {
            List<IRenderable> healthEntries = new List<IRenderable>();
            var (currentHealth, maxHealth, _) = player.PlayerData;

            // Get bounds of GUI Area of Scene.
            float left = sceneDefinition.WorldSceneBounds.left;
            float right = sceneDefinition.WorldSceneBounds.right;
            float top = sceneDefinition.WorldSceneBounds.top;
            float bottom = sceneDefinition.WorldSceneBounds.bottom;

            float gameoverSize = (right - left) / 4;

            for (int i = 0; i < 1; i++)
            {
                float xCenter = (left + right) / 2;
                float yCenter = (top + bottom) / 2;
                var texCoords = Heart.GetTexCoordsOfIndex(0); // Get the required texture.

                var entry = new GenericGUIRenderable()
                {
                    Scale = Vector2.One * .5f * gameoverSize,
                    Position = new Vector2(xCenter, yCenter),
                    Texture = Gameover,
                    Coordinates = texCoords,
                };
                healthEntries.Add(entry);
            }

            return healthEntries;
        }

        private class GenericGUIRenderable : IRenderableLayeredTextures
        {
            public ITexture Texture { get; set; }

            public IEnumerable<(Color4 color, Vector2[] vertices)> DebugData => null;

            public Vector2 Position { get; set; }

            public float Rotation => 0f;

            public Vector2 RotationAnker => this.Position;

            public Vector2 Scale { get; set; }

            internal TextureCoordinates Coordinates { get; set; }

            (IEnumerable<TextureCoordinates>, ITileTexture) IRenderableLayeredTextures.Texture => (new TextureCoordinates[] { this.Coordinates }, this.Texture as ITileTexture);
        }
    }
}
