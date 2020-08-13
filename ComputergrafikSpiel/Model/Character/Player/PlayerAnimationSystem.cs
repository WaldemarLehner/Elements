using ComputergrafikSpiel.Model.Interfaces;

namespace ComputergrafikSpiel.Model.Character.Player
{
    internal class PlayerAnimationSystem : IUpdateable
    {
        private readonly int frames;
        private readonly float timePerFrame;
        private float timeUntilNextFrame = 0;
        private bool moving = false;

        internal PlayerAnimationSystem(int frames, float timePerFrame = .3f)
        {
            this.frames = frames;
            this.timePerFrame = timePerFrame;
            this.timeUntilNextFrame = timePerFrame;
        }

        public int CurrentFrame { get; private set; } = 0;

        public void Update(float dtime)
        {
            if (!this.moving)
            {
                return;
            }

            this.timeUntilNextFrame -= dtime;
            if (this.timeUntilNextFrame <= 0)
            {
                this.SetNextMovingFrame();
                this.timeUntilNextFrame += this.timePerFrame;
            }
        }

        internal void UpdateIsMoving(bool moving)
        {
            this.moving = moving;
            if (!moving)
            {
                this.CurrentFrame = 0;
            }
        }

        private void SetNextMovingFrame()
        {
            this.CurrentFrame = (this.CurrentFrame + 1) % this.frames;
            if (this.CurrentFrame == 0)
            {
                this.CurrentFrame++;
            }
        }
    }
}