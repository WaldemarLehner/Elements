using System;
using static ComputergrafikSpiel.Model.Character.Player.PlayerStateOptionsDefaultFunctions;

namespace ComputergrafikSpiel.Model.Character.Player
{
    internal class PlayerStateOptions
    {
        public PlayerStateOptions(ValueUpgradeDelegate movementSpeedFunction, ValueUpgradeDelegate firerateFunction, ValueUpgradeDelegate projectileTTLFunction, ValueUpgradeDelegate attackDamageFunction, MaxHeartUpgradeCostDelegate extraHeartPriceFunction, PrizeMoneyCalculation prizeMoneyFunction)
        {
            this.MovementSpeedFunction = movementSpeedFunction ?? throw new ArgumentNullException(nameof(movementSpeedFunction));
            this.FirerateFunction = firerateFunction ?? throw new ArgumentNullException(nameof(firerateFunction));
            this.ExtraHeartPriceFunction = extraHeartPriceFunction ?? throw new ArgumentNullException(nameof(extraHeartPriceFunction));
            this.PrizeMoneyFunction = prizeMoneyFunction ?? throw new ArgumentNullException(nameof(prizeMoneyFunction));
            this.ProjectileTTLFunction = projectileTTLFunction ?? throw new ArgumentNullException(nameof(projectileTTLFunction));
            this.AttackDamageFunction = attackDamageFunction ?? throw new ArgumentNullException(nameof(attackDamageFunction));
        }

        internal delegate (float improvement, uint cost) ValueUpgradeDelegate(uint upgradeLevel);

        internal delegate uint MaxHeartUpgradeCostDelegate(uint upgradeLevel);

        internal delegate uint PrizeMoneyCalculation(uint chamber);

        internal static PlayerStateOptions Default => new PlayerStateOptions(SpeedCostAndValue(), FirerateCostAndValue(), WeaponTTLCostAndValue(), WeaponDamageCostAndValue(), MaxHeartCost(), MoneyReward());

        internal ValueUpgradeDelegate MovementSpeedFunction { get; set; }

        internal ValueUpgradeDelegate FirerateFunction { get; set; }

        internal ValueUpgradeDelegate ProjectileTTLFunction { get; set; }

        internal ValueUpgradeDelegate AttackDamageFunction { get; set; }

        internal MaxHeartUpgradeCostDelegate ExtraHeartPriceFunction { get; set; }

        internal PrizeMoneyCalculation PrizeMoneyFunction { get; set; }
    }
}