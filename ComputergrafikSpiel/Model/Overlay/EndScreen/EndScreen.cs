using System.Collections.Generic;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.Overlay.EndScreen
{
    public class EndScreen : IUpdateable
    {
        private static readonly string[] Text = new string[2] { "retry", "quit" };
        private readonly List<EndScreenButton> endScreenButtons = new List<EndScreenButton>();
        private readonly List<IRenderable> renderables = new List<IRenderable>();

        internal EndScreen(float tileSize, Vector2 center, float width, float margin = 5f)
        {
            this.TileSize = tileSize;

            var entrySize = width / 5;

            for (int i = 0; i < 2; i++)
            {
                float y = center.Y - ((entrySize + margin) * (i + 1));
                this.endScreenButtons.Add(new EndScreenButton(this, new Vector2(center.X, y), new Vector2(entrySize * 8, entrySize), Text[i]));
            }
        }

        public float TileSize { get; }

        public bool NeedsUpdate { get; set; } = true;

        public IEnumerable<IRenderable> Renderables => this.renderables;

        public void Update(float dtime)
        {
            foreach (var entry in this.endScreenButtons)
            {
                entry.Update(dtime);
            }

            if (!this.NeedsUpdate)
            {
                return;
            }

            this.renderables.Clear();
            foreach (var button in this.endScreenButtons)
            {
                this.renderables.AddRange(button.Background);
                this.renderables.AddRange(button.Foreground);
            }
        }
    }
}
