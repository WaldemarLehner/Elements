using ComputergrafikSpiel.Model.Character.Player;

namespace ComputergrafikSpiel.Model.Overlay.UpgradeScreen
{
    public struct UpgradeOption
    {
        public UpgradeOption(PlayerEnum.Stats stat, float valueBefore, float valueAfter, uint price)
        {
            this.Stat = stat;
            this.ValueBefore = valueBefore;
            this.ValueAfter = valueAfter;
            this.Price = price;
        }

        internal PlayerEnum.Stats Stat { get; }

        internal float ValueBefore { get; }

        internal float ValueAfter { get; }

        internal float Improvement => this.ValueAfter - this.ValueBefore;

        internal uint Price { get; }
    }
}
