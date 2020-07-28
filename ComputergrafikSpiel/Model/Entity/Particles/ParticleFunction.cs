using System;
using ComputergrafikSpiel.Model.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.Entity.Particles
{
    internal class ParticleFunction : IUpdateable
    {
        private readonly ScaleFunction scaleFunction;
        private readonly PositionalOffsetFunction positionalFunction;
        private readonly ColorFunction colorFunction;
        private readonly float ttl;

        private float dTime;

        public ParticleFunction(ScaleFunction scaleFunction, PositionalOffsetFunction positionalFunction, ColorFunction colorFunction, float ttl)
        {
            this.scaleFunction = scaleFunction ?? throw new ArgumentNullException(nameof(scaleFunction));
            this.positionalFunction = positionalFunction ?? throw new ArgumentNullException(nameof(positionalFunction));
            this.colorFunction = colorFunction ?? throw new ArgumentNullException(nameof(colorFunction));
            this.ttl = ttl;
            this.Finished = false;
            this.dTime = 0f;
        }

        internal delegate float ScaleFunction(float normalizedTime);

        internal delegate Vector2 PositionalOffsetFunction(float normalizedTime);

        internal delegate (byte r, byte g, byte b) ColorFunction(float normalizedTime);

        internal float Scale => this.scaleFunction(this.NormalizedDTime);

        internal (byte r, byte g, byte b) Color => this.colorFunction(this.NormalizedDTime);

        internal Vector2 Offset => this.positionalFunction(this.NormalizedDTime);

        internal bool Finished { get; private set; }

        private float NormalizedDTime => this.dTime / this.ttl;

        public void Update(float dtime)
        {
            this.dTime += dtime;
            if (this.dTime > this.ttl)
            {
                this.Finished = true;
            }
        }
    }
}
