using System;

namespace ComputergrafikSpiel.Model.Collider
{
    public class ColliderLayer
    {
        [Flags]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:Enumeration items should be documented", Justification = "not needed")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:Code should not contain multiple whitespace in a row", Justification = "helps visibility")]
        public enum Layer
        {
            Empty =  0b00000000,
            Player = 0b00000001,
            Enemy =  0b00000010,
            Bullet = 0b00000100,
            Wall =   0b00001000,
            Water =  0b00010000,
            Interactable = 0b00100000,
            Trigger = 0b01000000,
        }

        /// <summary>
        /// Checks flags of source Collider and destination Collider. If they can Collide, true is returned.
        /// </summary>
        /// <param name="source">source Collider's Layer Flags.</param>
        /// <param name="destination">destination Collider's Layer Flags.</param>
        /// <returns>If Colliders can Collide.</returns>
        public static bool CanCollide(Layer source, Layer destination)
        {
            return (source & destination) > 0;
        }
    }
}
