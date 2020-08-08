using ComputergrafikSpiel.Model.EntitySettings.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.Character
{
    public interface ICharacter : IEntity
    {
        int MaxHealth { get; }

        float MovementSpeed { get; }

        Vector2 LastPosition { get; set; }

        bool TextureWasMirrored { get; set; }

        float BloodColorHue { get; }

        void LookAt(Vector2 vec);

        void CollisionPrevention();
    }
}
