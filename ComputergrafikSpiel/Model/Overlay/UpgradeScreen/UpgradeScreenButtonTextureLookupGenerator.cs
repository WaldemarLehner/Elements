using System.Collections.Generic;
using ComputergrafikSpiel.Model.Character.Player;
using ComputergrafikSpiel.Model.EntitySettings.Texture;
using ComputergrafikSpiel.Model.EntitySettings.Texture.Interfaces;

namespace ComputergrafikSpiel.Model.Overlay.UpgradeScreen
{
    internal static class UpgradeScreenButtonTextureLookupGenerator
    {
        internal static Dictionary<PlayerEnum.Stats, ITileTexture> Default => GenerateDefault();

        internal static string MainText(UpgradeOption upgrade)
        {
            var nameText = GetName(upgrade.Stat);
            var upgradeText = GetUpgradeText(upgrade.ValueBefore, upgrade.Improvement, upgrade.ValueAfter);
            return $"{nameText} {upgradeText}";
        }

        private static object GetUpgradeText(float valueBefore, float improvement, float valueAfter)
        {
            return $"{valueBefore.Truncate()}+{improvement.Truncate()}>{valueAfter.Truncate()}";
        }

        private static string Truncate(this float value) => string.Format("{0:0.00}", value);

        private static string GetName(PlayerEnum.Stats s)
        {
            switch (s)
            {
                case PlayerEnum.Stats.AttackSpeed: return "firerate";
                case PlayerEnum.Stats.MaxHealth: return "max health";
                case PlayerEnum.Stats.Money: return "money bonus";
                case PlayerEnum.Stats.MovementSpeed: return "movement";
                default: return "error";
            }
        }

        private static Dictionary<PlayerEnum.Stats, ITileTexture> GenerateDefault()
        {
            const string root = "GUI/Upgrades/";
            var texLoader = new TextureLoader();
            return new Dictionary<PlayerEnum.Stats, ITileTexture>
            {
                [PlayerEnum.Stats.AttackSpeed] = texLoader.LoadTileTexture(root + "FireRate", (1, 1)),
                [PlayerEnum.Stats.Money] = texLoader.LoadTileTexture(root + "Coin", (1, 1)),
                [PlayerEnum.Stats.MaxHealth] = texLoader.LoadTileTexture(root + "ExtraHeart", (1, 1)),
                [PlayerEnum.Stats.MovementSpeed] = texLoader.LoadTileTexture(root + "MovementSpeed", (1, 1)),
            };
        }
    }
}
