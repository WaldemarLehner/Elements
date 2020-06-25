using ComputergrafikSpiel.Model.EntitySettings.Texture.ConstructorHelpers;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture
{
    public class Texture : ITexture
    {
        public Texture(TextureContructor contructor)
        {
            this.Width = contructor.Width;
            this.Height = contructor.Height;
            this.FilePath = contructor.FilePath;
        }

        public int Width { get; }

        public int Height { get; }

        public string FilePath { get; }

        public TextureCoordinates TextureCoordinates => new TextureCoordinates(new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 0));

        public void Update(float dtime)
        {
        }
    }
}
