using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.Character.Weapon.Interfaces
{
    public interface IWeapon : IEntity
    {
        float Firerate { get; }

        int ProjectileCreationCount { get; }

        int BulletTTL { get; }

        float BulletSize { get; }

        float AttackDamage { get; }

        IColliderManager ColliderManager { get; }

        void CreateProjectile(Vector2 position, Vector2 direction);

        // possibly for later (enums for spread shot, regular shot, etc)
        // int Firemode { get; }
    }
}