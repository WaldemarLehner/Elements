using System;
using Spectrum;

namespace ComputergrafikSpiel.Model.Entity.Particles
{
    internal static class StaticParticleEmmiter
    {
        internal static void EmitOnce(EmitParticleOnceOptions opt)
        {
            var rand = new Random();

            for (int i = 0; i < opt.Count; i++)
            {
                CreateParticleAndPushToScene(i);
            }

            void CreateParticleAndPushToScene(int index)
            {
                var direction = opt.Direction.Deviate(opt.DirectionDeviation, rand);
                var speed = opt.Speed.Deviate(opt.SpeedDeviation, rand);
                var ttl = opt.TTL.Deviate(opt.TTLDeviation, rand).Clamp(0, null);
                var startHue = opt.Hue.Item1.Deviate(opt.HueDeviation, rand) % 360;
                if (startHue < 0)
                {
                    startHue += 360;
                }

                var endHue = opt.Hue.Item2.Deviate(opt.HueDeviation, rand) % 360;
                if (endHue < 0)
                {
                    endHue += 360;
                }

                var scale = opt.StartSize.Deviate(opt.StartSizeDeviation, rand).Clamp(0, null) / 2f;
                var startSaturation = opt.Saturation.Item1.Deviate(opt.SaturationDeviation, rand).Clamp(0, 1);
                var startValue = opt.Value.Item1.Deviate(opt.ValueDeviation, rand).Clamp(0, 1);

                var endSaturation = opt.Saturation.Item1.Deviate(opt.SaturationDeviation, rand).Clamp(0, 1);
                var endValue = opt.Value.Item1.Deviate(opt.ValueDeviation, rand).Clamp(0, 1);

                var startRGB = new Color.HSV(startHue, startSaturation, startValue).ToRGB();
                var endRGB = new Color.HSV(endHue, endSaturation, endValue).ToRGB();

                var startRGBTuple = (startRGB.R, startRGB.G, startRGB.B);
                var endRGBTuple = (endRGB.R, endRGB.G, endRGB.B);

                ParticleFunction particleFunctions = new ParticleFunction(
                    GenericParticleFunctions.LinearScaleToZeroFunction(scale),
                    GenericParticleFunctions.LinearPositionalFunctionToFrom(opt.PointOfEmmision, opt.PointOfEmmision + (direction.Normalized() * speed * ttl)),
                    GenericParticleFunctions.LinearTrivialColorFunction(startRGBTuple, endRGBTuple),
                    ttl);
                var particle = new GenericParticle(particleFunctions);
                Scene.Scene.Current?.SpawnParticle(particle);
            }
        }
    }
}
