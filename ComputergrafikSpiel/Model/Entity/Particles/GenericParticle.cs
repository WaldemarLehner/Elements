using System.Collections.Generic;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using ComputergrafikSpiel.Model.Interfaces;
using OpenTK;
using OpenTK.Graphics;

namespace ComputergrafikSpiel.Model.Entity.Particles
{
    internal class GenericParticle : IParticle
    {
        private readonly ParticleFunction function;

        internal GenericParticle(ParticleFunction functions)
        {
            this.function = functions;
        }

        public (byte r, byte g, byte b) Color => this.function.Color;

        public ITexture Texture => null;

        public IEnumerable<(Color4 color, Vector2[] vertices)> DebugData => null;

        public Vector2 Position => this.function.Offset;

        public float Rotation => 0f;

        public Vector2 RotationAnker => this.Position;

        public Vector2 Scale => this.function.Scale * Vector2.One;

        public void Update(float dtime)
        {
            this.function.Update(dtime);
            if (this.function.Finished)
            {
                Scene.Scene.Current.RemoveParticle(this);
            }
        }
    }
}
