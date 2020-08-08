using OpenTK;

namespace ComputergrafikSpiel.Model.Entity.Particles
{
    internal static class EmitParticleOnceOptionsHelper
    {
        public static EmitParticleOnceOptions PlayerWeaponMuzzle => GenerateWeaponMuzzleParticleOptions();

        public static EmitParticleOnceOptions ProjectileHit => GenerateProjectileHitParticleOptions();

        public static EmitParticleOnceOptions BulletSmoke => GenerateBulletSmokeOptions();

        public static EmitParticleOnceOptions Dirt => GenerateDirtDustOption();

        private static EmitParticleOnceOptions GenerateDirtDustOption()
        {
            return new EmitParticleOnceOptions
            {
                Count = 3,
                Speed = 1,
                TTL = 2f,
                TTLDeviation = .2f,
                Direction = new Vector2(0, 1),
                DirectionDeviation = 180f,
                PointOfEmmision = Vector2.Zero, // TO be overridden on return
                Hue = (37f, 37f),
                HueDeviation = 2f,
                Saturation = (.95f, .5f),
                SaturationDeviation = .1f,
                Value = (.45f, .3f),
                ValueDeviation = .05f,
                SpeedDeviation = 10f,
                StartSize = 3f,
                StartSizeDeviation = 1f,
            };
        }

        private static EmitParticleOnceOptions GenerateBulletSmokeOptions()
        {
            return new EmitParticleOnceOptions
            {
                Count = 5,
                Speed = 10,
                TTL = 1f,
                TTLDeviation = .2f,
                Direction = Vector2.Zero, // To be overridden on return
                DirectionDeviation = 20f,
                PointOfEmmision = Vector2.Zero, // TO be overridden on return
                Hue = (37f, 20f),
                HueDeviation = 10f,
                Saturation = (.95f, .5f),
                SaturationDeviation = .1f,
                Value = (1f, .1f),
                ValueDeviation = .1f,
                SpeedDeviation = 10f,
                StartSize = 5f,
                StartSizeDeviation = 1f,
            };
        }

        private static EmitParticleOnceOptions GenerateProjectileHitParticleOptions()
        {
            return new EmitParticleOnceOptions
            {
                Count = 10,
                Speed = 60,
                TTL = 1f,
                TTLDeviation = .5f,
                Direction = Vector2.Zero, // To be overridden on return
                DirectionDeviation = 20f,
                PointOfEmmision = Vector2.Zero, // TO be overridden on return
                Hue = (0f, 0f),
                HueDeviation = 20f,
                Saturation = (.95f, .5f),
                SaturationDeviation = .1f,
                Value = (1f, .5f),
                ValueDeviation = .1f,
                SpeedDeviation = 2f,
                StartSize = 5f,
                StartSizeDeviation = 2f,
            };
        }

        private static EmitParticleOnceOptions GenerateWeaponMuzzleParticleOptions()
        {
            return new EmitParticleOnceOptions
            {
                Count = 5,
                Speed = 50,
                TTL = 1f,
                TTLDeviation = .2f,
                Direction = Vector2.Zero, // To be overridden on return
                DirectionDeviation = 40f,
                PointOfEmmision = Vector2.Zero, // TO be overridden on return
                Hue = (0f, 0f),
                HueDeviation = 0f,
                Saturation = (0f, 0f),
                SaturationDeviation = 0f,
                Value = (.9f, 1f),
                ValueDeviation = .1f,
                SpeedDeviation = 2f,
                StartSize = 10f,
                StartSizeDeviation = 5f,
            };
        }
    }
}