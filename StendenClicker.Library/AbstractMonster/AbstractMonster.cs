using StendenClicker.Library.CurrencyObjects;
using StendenClicker.Library.PlayerControls;
using System;
using System.Collections.Generic;

namespace StendenClicker.Library.AbstractMonster
{
	public abstract class AbstractMonster : IAbstractMonster
	{
		protected int Health { get; set; }
        protected int Damage { get; set; }
        protected double DamageFactor { get; set; } = 1;
		protected ulong CurrencyAmount { get; set; }
        protected string Sprite { get; set; }
        protected string Name { get; set; }

        public virtual void DoDamage(int damage)
        {
            Damage += (int)(DamageFactor * damage);
        }

        public virtual int GetHealth()
        {
            return Health - Damage;
        }

        public virtual bool IsDefeated()
        {
            return 0 >= GetHealth();
        }

        public virtual Image GetMonsterAsset()
        {
            return new Image(Sprite);
        }

        public abstract PlayerCurrency GetReward();
    }

}
