using System;

namespace ComputergrafikSpiel.Model.World
{
    public class SpawnMask
    {
        [Flags]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "Self-Explanatory")]
        public enum Mask
        {
            Disallow = 0 << 0,
            AllowObstacle = 1 << 0,
            AllowNPC = 1 << 1,
            AllowInteractable = 1 << 2,
        }
    }
}
