using System;
using ComputergrafikSpiel.Model.Character.Weapon.Interfaces;
using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.Character
{
    public interface ICharacter : IEntity
    {
        event EventHandler CharacterDeath;

        event EventHandler CharacterHit;

        event EventHandler CharacterMove;

        int MaxHealth { get; }

        float MovementSpeed { get; }

        int Defense { get; }

        Vector2 LastPosition { get; set; }

        bool TextureWasMirrored { get; set; }

        void OnDeath(EventArgs e);

        void OnHit(EventArgs e);

        void OnMove(EventArgs e);

        void LookAt(Vector2 vec);

        void CollisionPrevention();
    }
}
