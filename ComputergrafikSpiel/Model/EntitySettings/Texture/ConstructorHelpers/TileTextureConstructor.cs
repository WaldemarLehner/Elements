using ComputergrafikSpiel.Model.EntitySettings.Texture.ConstructorHelpers.Interfaces;
using ComputergrafikSpiel.View.Exceptions;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture.ConstructorHelpers
{
    internal class TileTextureConstructor : ITileTextureContructor
    {
        internal TileTextureConstructor(int xRows, int yRows)
        {
            if (xRows <= 0)
            {
                throw new ArgumentNotPositiveIntegerGreaterZeroException(nameof(xRows));
            }

            if (yRows <= 0)
            {
                throw new ArgumentNotPositiveIntegerGreaterZeroException(nameof(yRows));
            }

            this.XRows = xRows;
            this.YRows = yRows;
        }

        public int XRows { get; }

        public int YRows { get; }
    }
}
