using System;
using System.Collections.Generic;
using ComputergrafikSpiel.Model.Overlay.EndScreen;
using ComputergrafikSpiel.Model.Overlay.UpgradeScreen;

namespace ComputergrafikSpiel.Model.Character.Player
{
    internal class PlayerStateManager : IPlayerStateManager
    {
        private readonly PlayerStateOptions options;
        private PlayerState current;
        private (ushort maxhealth, ushort movement, ushort firerate, ushort bulletttl, ushort bulletdamage) currentLevel;

        internal PlayerStateManager(PlayerStateOptions opt)
        {
            this.current = new PlayerState(0, 5, 5, 1, 1f, .6f, 5);
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

            {
                var (improvement, cost) = this.options.BulletTTLFunction(this.currentLevel.bulletttl);
                if (cost <= this.current.Currency)
                {
                    options.Add(new UpgradeOption(PlayerEnum.Stats.BulletTTL, this.current.BulletTTL, this.current.BulletTTL + improvement, cost));
                }
            }

            {
                var (improvement, cost) = this.options.BulletDamageFunction(this.currentLevel.bulletdamage);
                if (cost <= this.current.Currency)
                {
                    options.Add(new UpgradeOption(PlayerEnum.Stats.BulletDamage, this.current.BulletDamage, this.current.BulletDamage + improvement, cost));
                }
            }

            var firerate = this.options.FirerateFunction(this.currentLevel.firerate);
            if (firerate.cost <= this.current.Currency)
            {
                options.Add(new UpgradeOption(PlayerEnum.Stats.AttackSpeed, this.current.Firerate, this.current.Firerate + firerate.improvement, firerate.cost));
            }

            var money = this.options.PrizeMoneyFunction(level);
            options.Add(new UpgradeOption(PlayerEnum.Stats.Money, this.current.Currency, this.current.Currency + money, 0));

            return options;
        }

        public IList<EndOption> GetEndOptions(uint level)
        {
            List<EndOption> options = new List<EndOption>();
            var heartPrice = this.options.ExtraHeartPriceFunction(this.currentLevel.maxhealth);
            if (heartPrice <= this.current.Currency)
            {
                options.Add(new EndOption(PlayerEnum.Stats.MaxHealth, this.current.MaxHealth, this.current.MaxHealth + 1, heartPrice));
            }

            {
                var (improvement, cost) = this.options.MovementSpeedFunction(this.currentLevel.movement);
                if (cost <= this.current.Currency)
                {
                    options.Add(new EndOption(PlayerEnum.Stats.MovementSpeed, this.current.MovementSpeed, this.current.MovementSpeed + improvement, cost));
                }
            }

            var firerate = this.options.FirerateFunction(this.currentLevel.firerate);
            if (firerate.cost <= this.current.Currency)
            {
                options.Add(new EndOption(PlayerEnum.Stats.AttackSpeed, this.current.Firerate, this.current.Firerate + firerate.improvement, firerate.cost));
            }

            var money = this.options.PrizeMoneyFunction(level);
            options.Add(new EndOption(PlayerEnum.Stats.Money, this.current.Currency, this.current.Currency + money, 0));

            return options;
        }

        internal bool Reset()
        {
            Console.WriteLine("got resetted");
            return true;
        }

        internal bool Apply(PlayerEnum.Stats stat, uint currentChamber)
        {
            if (stat != PlayerEnum.Stats.AttackSpeed && stat != PlayerEnum.Stats.BulletTTL && stat != PlayerEnum.Stats.BulletDamage && stat != PlayerEnum.Stats.MaxHealth && stat != PlayerEnum.Stats.MovementSpeed && stat != PlayerEnum.Stats.Money)
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
                case PlayerEnum.Stats.BulletTTL:
                    {
                        var (improvement, cost) = this.options.BulletTTLFunction(this.currentLevel.bulletttl++);
                        this.current.BulletTTL += improvement;
                        this.current.Currency -= cost;
                    }

                    return true;
                case PlayerEnum.Stats.BulletDamage:
                    {
                        var (improvement, cost) = this.options.BulletDamageFunction(this.currentLevel.bulletdamage++);
                        this.current.BulletDamage += (int)improvement;
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
                case PlayerEnum.Stats.BulletTTL:
                    return this.current.Currency - this.options.BulletTTLFunction(this.currentLevel.bulletttl).cost >= 0;
                case PlayerEnum.Stats.BulletDamage:
                    return this.current.Currency - this.options.BulletDamageFunction(this.currentLevel.bulletdamage).cost >= 0;
                case PlayerEnum.Stats.MaxHealth:
                    return this.current.Currency - this.options.PrizeMoneyFunction(chamber) >= 0;
                case PlayerEnum.Stats.MovementSpeed:
                    return this.current.Currency - this.options.MovementSpeedFunction(this.currentLevel.movement).cost >= 0;
                case PlayerEnum.Stats.Money:
                    return true;
                default:
                    return false;
            }
        }
    }
}
