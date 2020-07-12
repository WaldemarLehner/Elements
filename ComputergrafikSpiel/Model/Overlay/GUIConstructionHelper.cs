using System.Collections.Generic;
using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.Overlay
{
    internal static class GUIConstructionHelper
    {
        private static readonly ITexture HeartFull = new TextureLoader().LoadTexture("debugGrid16x16_directional");
        private static readonly ITexture HeartEmpty = new TextureLoader().LoadTexture("debugGrid16x16");

        internal static IGUIElement[] GenerateGuiHealthIndicator(int currentHealth, int maxHealth)
        {
            List<IGUIElement> healthEntries = new List<IGUIElement>();
            int fullCount = currentHealth;
            float itemWidth = maxHealth / .7f;

            // Clamp vertical value.
            if (itemWidth > .1f)
            {
                itemWidth = .1f;
            }

            for (int i = 0; i < maxHealth; i++)
            {
                healthEntries.Add(new GUIElement
                {
                    Offset = new Vector2(-((maxHealth / 2f) - i - 1) * itemWidth, 0),
                    Size = (itemWidth * .5f, null),
                    AspectRatio = 1f,
                    Texture = (i < fullCount) ? HeartFull : HeartEmpty,
                });
            }

            return healthEntries.ToArray();
        }

        internal class GUIElement : IGUIElement
        {
            public ITexture Texture { get; set; }

            public Vector2 Offset { get; set; }

            public (float? width, float? height) Size { get; set; }

            public float AspectRatio { get; set; }
        }
    }
}
