using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;
using ComputergrafikSpiel.Model.World;
using System;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture
{
    public class WorldTileTextureLoader : ITextureLoader
    {
        public WorldTileTextureLoader()
        {

        }

        public ITileTexture LoadTexture(WorldTile.Type type)
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

            Texture texture = new Texture(constructor);
            return texture;
        }

        private static string GetTextureNameByType(WorldTile.Type type)
        {
            switch (type)
            {
                case WorldTile.Type.Dirt: return "Ground/"
            }
        }
    }
}
