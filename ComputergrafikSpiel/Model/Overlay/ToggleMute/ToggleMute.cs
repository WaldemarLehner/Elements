using System.Collections.Generic;
using ComputergrafikSpiel.Model.Character.Player;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.Overlay.ToggleMute
{
    public class ToggleMute : IUpdateable
    {
        private readonly PlayerEnum.Stats[] toggleItems = new PlayerEnum.Stats[2] { PlayerEnum.Stats.Mute, PlayerEnum.Stats.Unmute };
        // private static readonly string[] Text = new string[2] { "mute", "unmute" };
        private readonly List<ToggleMuteButton> endScreenButtons = new List<ToggleMuteButton>();
        private readonly List<IRenderable> renderables = new List<IRenderable>();

        internal ToggleMute(float tileSize, Vector2 center, float width, float margin = 5f)
        {
            this.TileSize = tileSize;

            var entrySize = width / 5;

            for (int i = 0; i < 1; i++)
            {
                float y = center.Y - ((entrySize + margin) * (i + 2));
                this.endScreenButtons.Add(new ToggleMuteButton(this, new Vector2(center.X, y), new Vector2(entrySize * 8, entrySize), this.toggleItems[i]));
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
