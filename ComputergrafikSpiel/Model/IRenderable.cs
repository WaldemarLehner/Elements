using OpenTK;

namespace ComputergrafikSpiel.Model
{
    interface IRenderable
    {
        Vector2 position;
        Vector2 scale;

        float rotation;
        Vector2 rotationAnker;

        ITexture texture;

        public Vector2 Position { get; }
        public Vector2 Scale { get; }
        public float Rotation { get; }
        public Vector2 RotationAnker { get; }
        public ITexture Texture { get; }
    }
}