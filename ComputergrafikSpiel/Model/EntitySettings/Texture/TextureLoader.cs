using System;
using System.Collections.Generic;
using System.IO;
using ComputergrafikSpiel.Model.EntitySettings.Texture.ConstructorHelpers;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using SixLabors.ImageSharp;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture
{
    internal class TextureLoader : ITextureLoader
    {
        public ITexture LoadTexture(string name)
        {
            // Leerer Texture name
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name), "Error: Name can not be empty.");
            }

            // Endung .png wird hinzugefügt
            name += ".png";

            var pathToTexture = Path.Combine("./Content/Images/", name);
            Image currentTexture = Image.Load(pathToTexture);
            TextureContructor constructor;

            try
            {
                constructor = new TextureContructor(currentTexture.Width, currentTexture.Height, pathToTexture);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: im Kontruktor {e}");
                return null;
            }

            Texture texture = new Texture(constructor);
            return texture;
        }

        internal ITileTexture LoadTileTexture(string name, (int x, int y) tileCount)
        {
            // Leerer Texture name
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name), "Error: Name can not be empty.");
            }

            // Endung .png wird hinzugefügt
            name += ".png";

            var pathToTexture = Path.Combine("./Content/Images/", name);
            Image currentTexture = Image.Load(pathToTexture);
            TextureContructor constructor;
            try
            {
                constructor = new TextureContructor(currentTexture.Width, currentTexture.Height, pathToTexture);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: im Kontruktor {e}");
                return null;
            }

            TileTextureConstructor tileTextureConstructor = new TileTextureConstructor(tileCount.x, tileCount.y);
            return new TileTexture(constructor, tileTextureConstructor);
        }

        internal IMappedTileFont LoadFontTexture(string name, (int x, int y) tileCount, Dictionary<char, int> mappings)
        {
            // Leerer Texture name
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name), "Error: Name can not be empty.");
            }

            // Endung .png wird hinzugefügt
            name += ".png";

            var pathToTexture = Path.Combine("./Content/Images/", name);
            Image currentTexture = Image.Load(pathToTexture);
            TextureContructor constructor;
            try
            {
                constructor = new TextureContructor(currentTexture.Width, currentTexture.Height, pathToTexture);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: im Kontruktor {e}");
                return null;
            }

            TileTextureConstructor tileTextureConstructor = new TileTextureConstructor(tileCount.x, tileCount.y);
            var collection = new List<(char, int)>();

            foreach (var entry in mappings)
            {
                collection.Add((entry.Key, entry.Value));
            }

            return new FontTexture(constructor, tileTextureConstructor, collection);
        }
    }
}
