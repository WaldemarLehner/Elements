using System;

namespace ComputergrafikSpiel.Model.Character.Player
{
    internal static class PlayerStateOptionsDefaultFunctions
    {
        internal static PlayerStateOptions.ValueUpgradeDelegate SpeedCostAndValue()
        {
            /// <see cref="https://www.desmos.com/calculator/bylquxrt04"/>
            return (uint x) =>
            {
                uint price = (x < 5) ? (uint)Math.Ceiling((x * x) - Math.Pow(x, 1.7f)) : 10;
                float valueIncrease = (float)(.2f - (Math.Log(x + 1, 3) * .1f));
                if (valueIncrease < .05f)
                {
                    valueIncrease = .05f;
                }

                return (valueIncrease, price);
            };
        }

        /// <summary>
        /// Function to determine price and upgrade for a given Upgrade Level for the fire rate.
        /// </summary>
        /// <returns>Price and Upgrade.</returns>
        internal static PlayerStateOptions.ValueUpgradeDelegate FirerateCostAndValue()
        {
            return (uint x) =>
            {
                uint price = (x < 5) ? (uint)Math.Ceiling((x * x) - Math.Pow(x, 1.7f)) : 10;
                float valueIncrease = (float)(.5f - (Math.Log(x + 1, 3) * .2f));
                if (valueIncrease < .05f)
                {
                    valueIncrease = .05f;
                }

                return (valueIncrease, price);
            };
        }

        internal static PlayerStateOptions.ValueUpgradeDelegate BulletTTLCostAndValue()
        {
            return (uint x) =>
            {
                uint price = (x < 5) ? (uint)Math.Ceiling((x * x) - Math.Pow(x, 1.7f)) : 10;
                float valueIncrease = .1f;

                return (valueIncrease, price);
            };
        }

        internal static PlayerStateOptions.ValueUpgradeDelegate BulletDamageCostAndValue()
        {
            return (uint x) =>
            {
                uint price = (x < 5) ? (uint)Math.Ceiling((x * x) - Math.Pow(x, 1.7f)) : 10;
                float valueIncrease = 1;

                return (valueIncrease, price);
            };
        }

        /// <summary>
        /// Returns quantity of prize reward per level <see href="https://www.desmos.com/calculator/a4bqkvnuzt"/>.
        /// </summary>
        /// <returns>Quantity of Money.</returns>
        internal static PlayerStateOptions.PrizeMoneyCalculation MoneyReward()
        {
            return (uint x) =>
            {
                return (x < 40) ? (uint)Math.Ceiling((x / 4f) - Math.Pow(1.2, x / 3f) + 3f) : 1;
            };
        }

        internal static PlayerStateOptions.MaxHeartUpgradeCostDelegate MaxHeartCost()
        {
            return (uint x) =>
            {
                return (uint)Math.Ceiling(3f + (0.5f * (Math.Pow(1.3f, x) + x)));
            };
        }
    }
}
