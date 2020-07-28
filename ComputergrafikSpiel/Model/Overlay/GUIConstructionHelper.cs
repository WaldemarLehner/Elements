using System.Collections.Generic;
using ComputergrafikSpiel.Model.Character.Player.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using ComputergrafikSpiel.Model.World;
using OpenTK;
using OpenTK.Graphics;

namespace ComputergrafikSpiel.Model.Overlay
{
    internal static class GUIConstructionHelper
    {
        private static readonly ITileTexture Heart = new TextureLoader().LoadTileTexture("GUI/Heart", (x: 2, y: 1));
        private static readonly IMappedTileFont Font = new TextureLoader().LoadFontTexture("Font/vt323", (x: 8, y: 8), FontTextureMappingHelper.Default);

        internal static IEnumerable<IRenderable> GenerateGuiIndicator(IWorldScene sceneDefinition, IPlayer player)
        {
            var coinData = GUIConstructionHelper.GenerateCoinCount(sceneDefinition, player);
            var healthbar = GUIConstructionHelper.GenerateHealthBar(sceneDefinition, player);

            var renderables = new List<IRenderable>();
            renderables.AddRange(coinData);
            renderables.AddRange(healthbar);
            return renderables;
        }

        private static List<IRenderable> GenerateCoinCount(IWorldScene sceneDefinition, IPlayer player)
        {
            // Get bounds of GUI Area of Scene.
            (float top, float bottom, float left, _, float width, float height) = GenerateCoinGUIBounds(sceneDefinition.SceneDefinition.TileSize, sceneDefinition.WorldSceneBounds);

            string moneyCount = player.PlayerData.currency.ToString() + "$";
            int renderablesCount = moneyCount.Length;
            float itemSize = (width / renderablesCount) < height ? width / renderablesCount : height;

            List<IRenderable> renderables = new List<IRenderable>();

            for (int i = 0; i < renderablesCount; i++)
            {
                var tex = Font;
                int? texIndex = GetTexIndex(tex, moneyCount[i]);
                if (texIndex == null)
                {
                    continue;
                }

                var entry = new GenericGUIRenderable()
                {
                    Scale = Vector2.One * itemSize / 2f,
                    Position = new Vector2(left + ((i + .5f) * itemSize), (top + bottom) / 2f),
                    Texture = tex,
                    Coordinates = tex.GetTexCoordsOfIndex((int)texIndex),
                };
                renderables.Add(entry);
            }

            return renderables;
        }

        private static (float top, float bottom, float left, float right, float width, float height) GenerateCoinGUIBounds(int tileSize, (float top, float bottom, float left, float right) worldSceneBounds)
        {
            float left = worldSceneBounds.right - (tileSize * 3);
            float right = worldSceneBounds.right;
            float top = worldSceneBounds.top;
            float bottom = top - tileSize;
            float width = right - left;
            float height = top - bottom;

            return (top, bottom, left, right, width, height);
        }

        private static int? GetTexIndex(IMappedTileFont tex, char v)
        {
            var (x, y) = tex.GetTileOfKey(v);
            if (x == -1 && y == -1)
            {
                return null;
            }

            return (y * tex.XRows) + x;
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
                heartSize = top - bottom;
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

        private class GenericGUIRenderable : IRenderableLayeredTextures
        {
            public ITexture Texture { get; set; }

            public IEnumerable<(Color4 color, Vector2[] vertices)> DebugData => null;

            public Vector2 Position { get; set; }

            public float Rotation => 0f;

            public Vector2 RotationAnker => this.Position;

            public Vector2 Scale { get; set; }

            (IEnumerable<TextureCoordinates>, ITileTexture) IRenderableLayeredTextures.Texture => (new TextureCoordinates[] { this.Coordinates }, this.Texture as ITileTexture);

            internal TextureCoordinates Coordinates { get; set; }
        }
    }
}
