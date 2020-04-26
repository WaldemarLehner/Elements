using OpenTK;

namespace ComputergrafikSpiel.Model
{
    interface IRenderable
    {
        public Vector2 Position { get; }
        public Vector2 Scale { get; }
        public float Rotation { get; }
        public Vector2 RotationAnker { get; }
        public ITexture Texture { get; }
    }
}