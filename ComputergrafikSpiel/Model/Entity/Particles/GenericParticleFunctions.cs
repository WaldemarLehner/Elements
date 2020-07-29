using OpenTK;

namespace ComputergrafikSpiel.Model.Entity.Particles
{
    internal static class GenericParticleFunctions
    {
        // Linear.
        internal static ParticleFunction.PositionalOffsetFunction LinearPositionalFunctionToFrom(Vector2 from, Vector2 to) => (float dTime) => from + ((to - from) * dTime);

        internal static ParticleFunction.ScaleFunction LinearScaleToZeroFunction(float initialScale) => (float dTime) => (1 - dTime) * initialScale;

        internal static ParticleFunction.ColorFunction LinearTrivialColorFunction((byte r, byte g, byte b) from, (byte r, byte g, byte b) to) =>
            (float dTime) =>
            {
                var r = (byte)(from.r + ((to.r - from.r) * dTime));
                var g = (byte)(from.g + ((to.g - from.g) * dTime));
                var b = (byte)(from.b + ((to.b - from.b) * dTime));

                return (r, g, b);
            };
    }
}
