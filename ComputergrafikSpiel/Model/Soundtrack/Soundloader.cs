using WMPLib;

namespace ComputergrafikSpiel.Model.Soundtrack
{
    public class Soundloader
    {
        private readonly WindowsMediaPlayer mediaPlayer = new WindowsMediaPlayer();

        public Soundloader()
        {
            this.mediaPlayer.settings.setMode("loop", true);
        }

        public void StartSafeMusic()
        {
            this.mediaPlayer.URL = "./Content/Soundtrack/Spanish_Waltz.mp3";
            this.mediaPlayer.controls.play();
        }

        // Soundtrack der Dungeons
        public void StartDungeon1Music()
        {
            this.mediaPlayer.URL = "./Content/Soundtrack/Dungeons/Jumper.mp3";
            this.mediaPlayer.controls.play();
        }

        public void StartDungeon2Music()
        {
            this.mediaPlayer.URL = "./Content/Soundtrack/Dungeons/Rage_of_the_Champions.mp3";
            this.mediaPlayer.controls.play();
        }

        public void StartDungeon3Music()
        {
            this.mediaPlayer.URL = "./Content/Soundtrack/Dungeons/Space_Pirates.mp3";
            this.mediaPlayer.controls.play();
        }

        public void StartDungeon4Music()
        {
            this.mediaPlayer.URL = "./Content/Soundtrack/Dungeons/Dark_Skies.mp3";
            this.mediaPlayer.controls.play();
        }

        // Soundtrack der Bosskammern
        public void StartDungeon1BossMusic()
        {
            this.mediaPlayer.URL = "./Content/Soundtrack/Bosses/Race_Around_the_World.mp3";
            this.mediaPlayer.controls.play();
        }

        public void StartDungeon2BossMusic()
        {
            this.mediaPlayer.URL = "./Content/Soundtrack/Bosses/Castle_in_the_Sky.mp3";
            this.mediaPlayer.controls.play();
        }

        public void StartDungeon3BossMusic()
        {
            this.mediaPlayer.URL = "./Content/Soundtrack/Bosses/Vain_Star.mp3";
            this.mediaPlayer.controls.play();
        }

        public void StartDungeon4BossMusic()
        {
            this.mediaPlayer.URL = "./Content/Soundtrack/Bosses/Till_Death.mp3";
            this.mediaPlayer.controls.play();
        }

        public void StartGameoverMusic()
        {
            this.mediaPlayer.URL = "./Content/Soundtrack/Orange_Kiss.mp3";
            this.mediaPlayer.controls.play();
        }

        public void MuteMusik()
        {
            this.mediaPlayer.settings.mute = true;
        }

        public void UnmuteMusik()
        {
            this.mediaPlayer.settings.mute = false;
        }

        public void StopMusik()
        {
            this.mediaPlayer.controls.stop();
        }
    }
}
