using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model.Overlay.UpgradeScreen;

namespace ComputergrafikSpiel.Model.Character.Player
{
    internal class PlayerStateManager : IPlayerStateManager
    {
        private readonly PlayerStateOptions options;
        private PlayerState current;
        private (ushort maxhealth, ushort movement, ushort firerate, ushort weaponttl, ushort weapondamage) currentLevel;

        internal PlayerStateManager(PlayerStateOptions opt)
        {
            this.current = new PlayerState(0, 5, 5, 1, 1f, 5, .6f);
            this.options = opt ?? throw new ArgumentNullException(nameof(opt));
            this.currentLevel = (0, 0, 0, 0, 0);
        }

        public IPlayerState Current => this.current;

        public bool Heal()
        {
            if (this.current.Health < this.current.MaxHealth)
            {
                this.current.Health++;
                return true;
            }

            return false;
        }

        public void AddCoin(uint count)
        {
            this.current.Currency += count;
        }

        public void Hurt(ref bool died)
        {
            if (this.current.Health > 0)
            {
                this.current.Health--;
            }

            if (this.current.Health == 0)
            {
                died = true;
            }
            else
            {
                died = false;
            }
        }

        public IList<UpgradeOption> GetUpgradeOptions(uint level)
        {
            List<UpgradeOption> options = new List<UpgradeOption>();
            var heartPrice = this.options.ExtraHeartPriceFunction(this.currentLevel.maxhealth);
            if (heartPrice <= this.current.Currency)
            {
                options.Add(new UpgradeOption(PlayerEnum.Stats.MaxHealth, this.current.MaxHealth, this.current.MaxHealth + 1, heartPrice));
            }

            {
                var (improvement, cost) = this.options.MovementSpeedFunction(this.currentLevel.movement);
                if (cost <= this.current.Currency)
                {
                    options.Add(new UpgradeOption(PlayerEnum.Stats.MovementSpeed, this.current.MovementSpeed, this.current.MovementSpeed + improvement, cost));
                }
            }

            var firerate = this.options.FirerateFunction(this.currentLevel.firerate);
            if (firerate.cost <= this.current.Currency)
            {
                options.Add(new UpgradeOption(PlayerEnum.Stats.AttackSpeed, this.current.Firerate, this.current.Firerate + firerate.improvement, firerate.cost));
            }

            var weaponttl = this.options.ProjectileTTLFunction(this.currentLevel.weaponttl);
            if (weaponttl.cost <= this.current.BulletTTL)
            {
                options.Add(new UpgradeOption(PlayerEnum.Stats.WeaponTTL, this.current.BulletTTL, this.current.BulletTTL + weaponttl.improvement, weaponttl.cost));
            }

            var weapondamage = this.options.AttackDamageFunction(this.currentLevel.weapondamage);
            if (weapondamage.cost <= this.current.AttackDamage)
            {
                options.Add(new UpgradeOption(PlayerEnum.Stats.WeaponDamage, this.current.AttackDamage, this.current.AttackDamage + weapondamage.improvement, weapondamage.cost));
            }

            var money = this.options.PrizeMoneyFunction(level);
            options.Add(new UpgradeOption(PlayerEnum.Stats.Money, this.current.Currency, this.current.Currency + money, 0));

            return options;
        }

        internal bool Apply(PlayerEnum.Stats stat, uint currentChamber)
        {
            if (stat != PlayerEnum.Stats.AttackSpeed && stat != PlayerEnum.Stats.MaxHealth && stat != PlayerEnum.Stats.MovementSpeed && stat != PlayerEnum.Stats.Money && stat != PlayerEnum.Stats.WeaponTTL && stat != PlayerEnum.Stats.WeaponDamage)
            {
                return false;
            }

            if (!this.CanBuy(stat, currentChamber))
            {
                return false;
            }

            return this.Pick(stat, currentChamber);
        }

        private bool Pick(PlayerEnum.Stats stat, uint chamber)
        {
            switch (stat)
            {
                case PlayerEnum.Stats.AttackSpeed:
                    {
                        var (improvement, cost) = this.options.FirerateFunction(this.currentLevel.firerate++);
                        this.current.Firerate += improvement;
                        this.current.Currency -= cost;
                    }

                    return true;

                case PlayerEnum.Stats.WeaponTTL:
                    {
                        var (improvement, cost) = this.options.ProjectileTTLFunction(this.currentLevel.weaponttl++);
                        this.current.BulletTTL += improvement;
                        this.current.Currency -= cost;
                    }

                    return true;
                case PlayerEnum.Stats.WeaponDamage:
                    {
                        var (improvement, cost) = this.options.AttackDamageFunction(this.currentLevel.weapondamage++);
                        this.current.AttackDamage += improvement;
                        this.current.Currency -= cost;
                    }

                    return true;
                case PlayerEnum.Stats.MovementSpeed:
                    {
                        var (improvement, cost) = this.options.MovementSpeedFunction(this.currentLevel.movement++);
                        this.current.MovementSpeed += improvement;
                        this.current.Currency -= cost;
                    }

                    return true;
                case PlayerEnum.Stats.Money:
                    var money = this.options.PrizeMoneyFunction(chamber);
                    this.current.Currency += money;
                    return true;

                case PlayerEnum.Stats.MaxHealth:
                    var healthPrice = this.options.ExtraHeartPriceFunction(this.currentLevel.maxhealth++);
                    this.current.MaxHealth++;
                    this.current.Currency -= healthPrice;
                    return true;
                default:
                    return false;
            }
        }

        private bool CanBuy(PlayerEnum.Stats stat, uint chamber)
        {
            switch (stat)
            {
                case PlayerEnum.Stats.AttackSpeed:
                    return this.current.Currency - this.options.FirerateFunction(this.currentLevel.firerate).cost >= 0;
                case PlayerEnum.Stats.MaxHealth:
                    return this.current.Currency - this.options.PrizeMoneyFunction(chamber) >= 0;
                case PlayerEnum.Stats.MovementSpeed:
                    return this.current.Currency - this.options.MovementSpeedFunction(this.currentLevel.movement).cost >= 0;
                case PlayerEnum.Stats.WeaponTTL:
                    return this.current.Currency - this.options.ProjectileTTLFunction(this.currentLevel.weaponttl).cost >= 0;
                case PlayerEnum.Stats.WeaponDamage:
                    return this.current.Currency - this.options.AttackDamageFunction(this.currentLevel.weapondamage).cost >= 0;
                case PlayerEnum.Stats.Money:
                    return true;
                default:
                    return false;
            }
        }
    }
}
