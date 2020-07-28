using ComputergrafikSpiel.Model.Interfaces;
using System;

namespace ComputergrafikSpiel.Model.Entity.Particles
{
    internal class GenericParticleEmitter : IUpdateable
    {
        private readonly float emmisionPeriod;

        private bool enabled = true;
        private float timeUntilTick;

        internal GenericParticleEmitter(EmitParticleOnceOptions opts, float emmisionPeriod)
        {
            this.Options = opts;
            this.timeUntilTick = this.emmisionPeriod = emmisionPeriod;
            this.Emit();
        }

        public EmitParticleOnceOptions Options { get; set; }

        public void Enable() => this.enabled = true;

        public void Disable() => this.enabled = false;

        public void Update(float dtime)
        {
            if (!this.enabled)
            {
                return;
            }

            this.timeUntilTick -= dtime;

            if (this.timeUntilTick <= 0)
            {
                this.timeUntilTick = this.emmisionPeriod - this.timeUntilTick;
                this.Emit();
            }
        }

        private void Emit()
        {
            StaticParticleEmmiter.EmitOnce(this.Options);
        }
    }
}
