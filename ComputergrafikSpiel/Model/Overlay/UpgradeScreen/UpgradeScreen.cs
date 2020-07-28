using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model.Character.Player;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.Overlay.UpgradeScreen
{
    public class UpgradeScreen : IUpdateable
    {
        private readonly List<UpgradeScreenButton> upgradeScreenButtons = new List<UpgradeScreenButton>();
        private readonly List<IRenderable> renderables = new List<IRenderable>();

        internal UpgradeScreen(IList<UpgradeOption> options, float tileSize, Vector2 top, float width, float margin = 5f, Action<PlayerEnum.Stats> callback = null)
        {
            this.TileSize = tileSize;

            var entrySize = width / 5;

            for (int i = 0; i < options.Count; i++)
            {
                float y = top.Y - ((entrySize + margin) * (i + 1));
                this.upgradeScreenButtons.Add(new UpgradeScreenButton(this, new Vector2(top.X, y), options[i], new Vector2(entrySize * 8, entrySize), callback ?? ((PlayerEnum.Stats s) => Console.WriteLine("Clicked!"))));
            }
        }

        public float TileSize { get; }

        public bool NeedsUpdate { get; set; } = true;

        public IEnumerable<IRenderable> Renderables => this.renderables;

        public void Update(float dtime)
        {
            foreach (var entry in this.upgradeScreenButtons)
            {
                entry.Update(dtime);
            }

            if (!this.NeedsUpdate)
            {
                return;
            }

            this.renderables.Clear();
            foreach (var button in this.upgradeScreenButtons)
            {
                this.renderables.AddRange(button.Background);
                this.renderables.AddRange(button.Foreground);
            }
        }
    }
}
