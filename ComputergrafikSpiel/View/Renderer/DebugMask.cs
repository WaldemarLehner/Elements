using System;

namespace ComputergrafikSpiel.View.Renderer
{
    public static class DebugMask
    {
        [Flags]
        public enum Mask
        {
            TextureBoundingBox = 0b01,
            DebugData = 0b10,
            IndependentDebugData = 0b100,
        }
    }
}
