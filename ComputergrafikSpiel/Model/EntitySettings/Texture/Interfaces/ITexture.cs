using System;
using ComputergrafikSpiel.Model.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces
{
    internal interface ITexture : IUpdateable
    {
        int Width { get; }

        int Height { get; }

        string FilePath { get; }

        /// <summary>
        /// Gets the Coordinates for said texture, in case a spezialisation of the ITexture outputs just a subset of the entire texture.
        /// <see href="https://www.learnopengles.com/wordpress/wp-content/uploads/2011/09/texture-coordinates.png"> Graphical Representation </see>.
        /// </summary>
        (Vector2 TL, Vector2 TR, Vector2 BR, Vector2 BL) TextureCoordinates { get; }
    }
}