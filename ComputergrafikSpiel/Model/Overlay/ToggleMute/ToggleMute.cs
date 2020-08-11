using System.Collections.Generic;
using ComputergrafikSpiel.Model.Character.Player;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;

namespace ComputergrafikSpiel.Model.Overlay.ToggleMute
{
    public class ToggleMute : IUpdateable
    {
        // private readonly PlayerEnum.Stats[] toggleItems = new PlayerEnum.Stats[2] { PlayerEnum.Stats.Mute, PlayerEnum.Stats.Unmute };
        // private static readonly string[] Text = new string[2] { "mute", "unmute" };
        private readonly ToggleMuteButton toggleMuteButton;
        private readonly List<IRenderable> renderables = new List<IRenderable>();

        internal ToggleMute(PlayerEnum.Stats toggleItem)
        {
            this.toggleMuteButton = new ToggleMuteButton(this, toggleItem);
        }

        public bool NeedsUpdate { get; set; } = true;

        public IEnumerable<IRenderable> Renderables => this.renderables;

        public void Update(float dtime)
        {
            this.toggleMuteButton.Update(dtime);

            if (!this.NeedsUpdate)
            {
                return;
            }

            this.renderables.Clear();
            this.renderables.AddRange(this.toggleMuteButton.Background);
            this.renderables.AddRange(this.toggleMuteButton.Foreground);
        }
    }
}
