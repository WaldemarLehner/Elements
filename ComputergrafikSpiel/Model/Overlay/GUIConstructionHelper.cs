using System.Collections.Generic;
using System.Windows.Forms;
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
        private static readonly ITexture HeartFull = new TextureLoader().LoadTexture("GUI/Heart");
        private static readonly ITexture HeartEmpty = new TextureLoader().LoadTexture("GUI/HeartEmpty");

        internal static IEnumerable<IRenderable> GenerateGuiHealthIndicator(IWorldScene sceneDefinition, IPlayer player)
        {
            List<IRenderable> healthEntries = new List<IRenderable>();
            var (currentHealth, maxHealth, _) = player.PlayerData;

            // Get bounds of GUI Area of Scene.
            float left = sceneDefinition.WorldSceneBounds.left;
            float right = sceneDefinition.WorldSceneBounds.right;
            float top = sceneDefinition.SceneDefinition.TileSize;
            float bottom = 0f;
            float centerHeight = (top + bottom) * .5f;
            {
                top = (top - centerHeight) * .5f;
                bottom = (centerHeight - bottom) * .5f;
            }

            float heartSize = (right - left) / maxHealth;
            if (heartSize > top - bottom)
            {
                heartSize = top - left;
            }

            for (int i = 0; i < maxHealth; i++)
            {
                float xCenter = right - sceneDefinition.SceneDefinition.TileSize + ((i + 0.5f) * heartSize);
                var entry = new GenericGUIRenderable()
                {
                    Scale = heartSize / 2 * Vector2.One,
                    Position = new Vector2(xCenter, centerHeight),
                    Texture = (i < maxHealth - currentHealth ) ? HeartEmpty : HeartFull,
                };
                healthEntries.Add(entry);
            }

            return healthEntries;
        }

        internal class GenericGUIRenderable : IRenderable
        {
            public ITexture Texture { get; set; }

            public IEnumerable<(Color4 color, Vector2[] vertices)> DebugData => null;

            public Vector2 Position { get; set; }

            public float Rotation => 0f;

            public Vector2 RotationAnker => this.Position;

            public Vector2 Scale { get; set; }
        }
    }
}
