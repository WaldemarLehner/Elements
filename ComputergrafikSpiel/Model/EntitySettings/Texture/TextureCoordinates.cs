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

        public static TextureCoordinates Default => new TextureCoordinates(new Vector2(0, 1), Vector2.One, new Vector2(1, 0), Vector2.Zero);

        public Vector2 TopLeft { get; }

        public Vector2 TopRight { get; }

        public Vector2 BottomLeft { get; }

        public Vector2 BottomRight { get; }

        public bool IsXYAligned => this.XAlign && this.YAlign;

        private bool XAlign => this.TopLeft.X == this.BottomLeft.X && this.TopRight.X == this.BottomRight.X;

        private bool YAlign => this.BottomLeft.Y == this.BottomRight.Y && this.TopLeft.Y == this.TopRight.Y;

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
