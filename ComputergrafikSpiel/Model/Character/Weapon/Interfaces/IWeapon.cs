using System.Collections.Generic;
using ComputergrafikSpiel.Model.Character.NPC.Interfaces;
using ComputergrafikSpiel.Model.Collider.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.Character.Weapon.Interfaces
{
    public interface IWeapon : IEntity
    {
        float Firerate { get; }

        int ProjectileCreationCount { get; }

        float BulletTTL { get; }

        float BulletSize { get; }

        int AttackDamage { get; }

        IColliderManager ColliderManager { get; }

        void Shoot(Vector2 position, Vector2 direction, ICollection<INonPlayerCharacter> enemyList);

        // void CreateProjectile(Vector2 position, Vector2 direction);

        // possibly for later (enums for spread shot, regular shot, etc)
        // int Firemode { get; }
    }
}