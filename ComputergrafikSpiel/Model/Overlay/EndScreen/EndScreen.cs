using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model.Character.Player;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.Overlay.EndScreen
{
    public class EndScreen : IUpdateable
    {
        private static readonly List<string> Text = new List<string>(new string[] { "Retry", "Quit" });
        private readonly List<EndScreenButton> endScreenButtons = new List<EndScreenButton>();
        private readonly List<IRenderable> renderables = new List<IRenderable>();

        internal EndScreen(IList<EndOption> options, float tileSize, Vector2 bottom, float width, float margin = 5f, Action<PlayerEnum.Stats> reset = null)
        {
            this.TileSize = tileSize;

            var entrySize = width / 5;

            for (int i = 0; i < 2; i++)
            {
                float y = bottom.Y - ((entrySize + margin) * (i + 1));
                this.endScreenButtons.Add(new EndScreenButton(this, new Vector2(bottom.X, y), options[i], new Vector2(entrySize * 8, entrySize), reset ?? ((PlayerEnum.Stats s) => Console.WriteLine("Clicked!")), Text[i]));
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
