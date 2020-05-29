using System;
using ComputergrafikSpiel.Model.Interfaces;
using OpenTK;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces
{
    internal interface ITexture : IUpdateable
    {
        int Width { get; set; }

        int Height { get; set; }

        string FilePath { get; set; }

        /// <summary>
        /// Gets the Coordinates for said texture, in case a spezialisation of the ITexture outputs just a subset of the entire texture.
        /// <see href="https://www.learnopengles.com/wordpress/wp-content/uploads/2011/09/texture-coordinates.png"> Graphical Representation </see>.
        /// </summary>
        Tuple<Vector2, Vector2, Vector2, Vector2> TextureCoordinates { get; }
    }
}