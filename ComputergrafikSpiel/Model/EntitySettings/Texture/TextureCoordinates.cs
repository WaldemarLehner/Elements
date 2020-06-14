using OpenTK;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture
{
    public struct TextureCoordinates
    {
        public TextureCoordinates(Vector2 tl, Vector2 tr, Vector2 br, Vector2 bl)
        {
            this.TopLeft = tl;
            this.TopRight = tr;
            this.BottomLeft = bl;
            this.BottomRight = br;
        }

        public static TextureCoordinates Error => GetErrorTexCoords();

        public Vector2 TopLeft { get; }

        public Vector2 TopRight { get; }

        public Vector2 BottomLeft { get; }

        public Vector2 BottomRight { get; }

        public (Vector2 TL, Vector2 TR, Vector2 BR, Vector2 BL) AsTuple()
        {
            return (this.TopLeft, this.TopRight, this.BottomRight, this.BottomLeft);
        }

        private static TextureCoordinates GetErrorTexCoords()
        {
            Vector2 negVec = Vector2.One * -1;
            return new TextureCoordinates(negVec, negVec, negVec, negVec);
        }
    }
}
