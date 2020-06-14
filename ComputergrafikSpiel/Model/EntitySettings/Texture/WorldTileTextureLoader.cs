using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ComputergrafikSpiel.Model.EntitySettings.Texture.ConstructorHelpers;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using ComputergrafikSpiel.Model.World;
using SixLabors.ImageSharp;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture
{
    public class WorldTileTextureLoader : ITextureLoader
    {
        private static readonly Dictionary<TileDefinitions.Type, string> NameLookUp = new Dictionary<TileDefinitions.Type, string>()
        {
            { TileDefinitions.Type.Dirt,  "Ground/EarthTileSet" },
            { TileDefinitions.Type.WallTrim, "Wall/WallTipTileSet" },
            { TileDefinitions.Type.Water, "Ground/WaterTileSet" },
            { TileDefinitions.Type.Grass, "Ground/Grass" },
            { TileDefinitions.Type.Wall, "Wall/Wall" },
        };

        public ITileTexture LoadTexture(TileDefinitions.Type type)
        {
            string name = GetTextureNameByType(type);

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
                Console.WriteLine("Error: im Kontruktor {0}", e);
                return null;
            }

            TileTextureConstructor tileTextureConstructor = new TileTextureConstructor(5, 4);

            TileTexture texture = new TileTexture(constructor, tileTextureConstructor);
            return texture;
        }

        public ITexture LoadTexture(string name)
        {
            KeyValuePair<TileDefinitions.Type, string> type = WorldTileTextureLoader.NameLookUp.Where(x => x.Value == name).First();
            return this.LoadTexture(type.Key) as ITexture;
        }

        private static string GetTextureNameByType(TileDefinitions.Type type) => WorldTileTextureLoader.NameLookUp[type];
    }
}
