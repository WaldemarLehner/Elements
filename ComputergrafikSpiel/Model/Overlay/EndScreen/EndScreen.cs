using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputergrafikSpiel.Model.Overlay.EndScreen
{
    public class EndScreen : IUpdateable
    {
        private readonly List<EndScreenButton> endScreenButtons = new List<EndScreenButton>();
        private readonly List<IRenderable> renderables = new List<IRenderable>();

        internal EndScreen(float tileSize, Vector2 top, float width, float margin = 5f)
        {
            this.TileSize = tileSize;

            var entrySize = width / 5;

            for (int i = 0; i < 2; i++)
            {
                float y = top.Y - ((entrySize + margin) * (i + 1));
                this.endScreenButtons.Add(new EndScreenButton(this, new Vector2(top.X, y), new Vector2(entrySize * 8, entrySize)));
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
