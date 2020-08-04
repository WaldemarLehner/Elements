using System;
using static ComputergrafikSpiel.Model.Character.Player.PlayerStateOptionsDefaultFunctions;

namespace ComputergrafikSpiel.Model.Character.Player
{
    internal class PlayerStateOptions
    {
        public PlayerStateOptions(ValueUpgradeDelegate movementSpeedFunction, ValueUpgradeDelegate firerateFunction, ValueUpgradeDelegate bulletTTLFunction, ValueUpgradeDelegate bulletDamageFunction, MaxHeartUpgradeCostDelegate extraHeartPriceFunction, PrizeMoneyCalculation prizeMoneyFunction)
        {
            this.MovementSpeedFunction = movementSpeedFunction ?? throw new ArgumentNullException(nameof(movementSpeedFunction));
            this.FirerateFunction = firerateFunction ?? throw new ArgumentNullException(nameof(firerateFunction));
            this.BulletTTLFunction = bulletTTLFunction ?? throw new ArgumentNullException(nameof(bulletTTLFunction));
            this.BulletDamageFunction = bulletDamageFunction ?? throw new ArgumentNullException(nameof(bulletDamageFunction));
            this.ExtraHeartPriceFunction = extraHeartPriceFunction ?? throw new ArgumentNullException(nameof(extraHeartPriceFunction));
            this.PrizeMoneyFunction = prizeMoneyFunction ?? throw new ArgumentNullException(nameof(prizeMoneyFunction));
        }

        internal delegate (float improvement, uint cost) ValueUpgradeDelegate(uint upgradeLevel);

        internal delegate uint MaxHeartUpgradeCostDelegate(uint upgradeLevel);

        internal delegate uint PrizeMoneyCalculation(uint chamber);

        internal static PlayerStateOptions Default => new PlayerStateOptions(SpeedCostAndValue(), FirerateCostAndValue(), BulletTTLCostAndValue(), BulletDamageCostAndValue(), MaxHeartCost(), MoneyReward());

        internal ValueUpgradeDelegate MovementSpeedFunction { get; set; }

        internal ValueUpgradeDelegate FirerateFunction { get; set; }

        internal ValueUpgradeDelegate BulletTTLFunction { get; set; }

        internal ValueUpgradeDelegate BulletDamageFunction { get; set; }

        internal MaxHeartUpgradeCostDelegate ExtraHeartPriceFunction { get; set; }

        internal PrizeMoneyCalculation PrizeMoneyFunction { get; set; }
    }
}