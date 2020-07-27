using System;
using System.Media;

namespace ComputergrafikSpiel.Model.Soundtrack
{
    public class Soundloader : ISoundloader
    {
        private SoundPlayer soundplayer;
        public bool battleMusicOn = false;

        public Soundloader()
        {
        }

        public void StartMainThemeMusic()
        {
            this.soundplayer = new SoundPlayer("./Content/Soundtrack/main_theme.wav");
            this.soundplayer.PlayLooping();
        }

        public void StartSafeMusic()
        {
            this.soundplayer = new SoundPlayer("./Content/Soundtrack/safe_zone.wav");
            this.soundplayer.PlayLooping();
        }

        public void StartBattleMusic()
        {
            if (this.battleMusicOn == true)
            {
                return;
            }

            this.battleMusicOn = true;
            this.soundplayer = new SoundPlayer("./Content/Soundtrack/battle_has_begun.wav");
            this.soundplayer.PlayLooping();
        }

        public void StartBossMusic()
        {
            this.soundplayer = new SoundPlayer("./Content/Soundtrack/boss_fight.wav");
            this.soundplayer.PlayLooping();
        }

        public void StopMusik()
        {
            this.soundplayer.Stop();
        }

    }
}
