using OpenTK;

namespace ComputergrafikSpiel.Model.Entity.Particles
{
    internal struct EmitParticleOnceOptions
    {
        public static EmitParticleOnceOptions PlayerWeaponMuzzle => EmitParticleOnceOptionsHelper.PlayerWeaponMuzzle;

        public static EmitParticleOnceOptions ProjectileHit => EmitParticleOnceOptionsHelper.ProjectileHit;

        public static EmitParticleOnceOptions BulletSmoke => EmitParticleOnceOptionsHelper.BulletSmoke;

        public static EmitParticleOnceOptions Dirt => EmitParticleOnceOptionsHelper.Dirt;

        internal uint Count { get; set; }

        internal Vector2 Direction { get; set; }

        internal Vector2 PointOfEmmision { get; set; }

        internal float DirectionDeviation { get; set; }

        internal float StartSize { get; set; }

        internal float StartSizeDeviation { get; set; }

        internal (float, float) Hue { get; set; }

        internal float HueDeviation { get; set; }

        internal (float, float) Saturation { get; set; }

        internal float SaturationDeviation { get; set; }

        internal (float, float) Value { get; set; }

        internal float ValueDeviation { get; set; }

        internal float Speed { get; set; }

        internal float SpeedDeviation { get; set; }

        internal float TTL { get; set; }

        internal float TTLDeviation { get; set; }
    }
}
