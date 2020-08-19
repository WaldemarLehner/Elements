using WMPLib;

// Soundtrack CC License: https://creativecommons.org/2008/09/22/castle-crashers-soundtrack-released-under-cc-license/
namespace ComputergrafikSpiel.Model.Soundtrack
{
    public class Soundloader
    {
        private readonly WindowsMediaPlayer mediaPlayer;

        public Soundloader()
        {
            try
            {
                this.mediaPlayer = new WindowsMediaPlayer();
                this.mediaPlayer?.settings.setMode("loop", true);
            }
            catch
            {
                this.mediaPlayer = null;
            }
        }

        public void StartSafeMusic() => this.PlayMusic("./Content/Soundtrack/Creepy_Frog.mp3");

        // Soundtrack der Dungeons
        public void StartDungeon1Music() => this.PlayMusic("./Content/Soundtrack/Dungeons/Factory.mp3");

        public void StartDungeon2Music() => this.PlayMusic("./Content/Soundtrack/Dungeons/Rage_of_the_Champions.mp3");

        public void StartDungeon3Music() => this.PlayMusic("./Content/Soundtrack/Dungeons/Space_Pirates.mp3");

        public void StartDungeon4Music() => this.PlayMusic("./Content/Soundtrack/Dungeons/Dark_Skies.mp3");

        // Soundtrack der Bosskammern
        public void StartDungeon1BossMusic() => this.PlayMusic("./Content/Soundtrack/Bosses/Race_Around_the_World.mp3");

        public void StartDungeon2BossMusic() => this.PlayMusic("./Content/Soundtrack/Bosses/Castle_in_the_Sky.mp3");

        public void StartDungeon3BossMusic() => this.PlayMusic("./Content/Soundtrack/Bosses/Vain_Star.mp3");

        public void StartDungeon4BossMusic() => this.PlayMusic("./Content/Soundtrack/Bosses/Till_Death.mp");

        public void StartGameoverMusic() => this.PlayMusic("./Content/Soundtrack/Orange_Kiss.mp3");

        public void MuteMusic()
        {
            this.mediaPlayer.settings.mute = true;
        }

        public void UnmuteMusic()
        {
            this.mediaPlayer.settings.mute = false;
        }

        public void StopMusic()
        {
            this.mediaPlayer.controls.stop();
        }

        private void PlayMusic(string songPath)
        {
            if (this.mediaPlayer == null)
            {
                return;
            }

            this.mediaPlayer.URL = songPath;
            this.mediaPlayer.controls.play();
        }
    }
}
