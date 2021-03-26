using StendenClicker.Library.CurrencyObjects;
using StendenClicker.Library.PlayerControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StendenClicker.Library.AbstractMonster
{
	public abstract class AbstractMonster : IAbstractMonster
    {
        /// <summary>
        /// Total health of the monster.
        /// </summary>
		public int Health { get; set; }

        /// <summary>
        /// Total damage done to health of monster.
        /// </summary>
        public int Damage { get; set; }
        public double DamageFactor { get; set; } = 1;
		public ulong CurrencyAmount { get; set; }
        public string Sprite { get; set; }
        public string Name { get; set; }
        public int MonsterLevel { get; set; }

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

		public virtual int GetHealthPercentage()
		{
            return (int)(((double)GetHealth() / (double)Health) * (double)100);
		}

    }

}

