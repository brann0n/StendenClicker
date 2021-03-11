
using StendenClicker.Library.CurrencyObjects;
using StendenClicker.Library.PlayerControls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StendenClicker.Library.AbstractMonster
{
	public class Normal : AbstractMonster
	{
        //dictionary mapped with monster name and sprite location
		private static readonly Dictionary<string, string> monsters;
        private static readonly int InternalMonsterCount = 11;

        public Normal(int levelNr)
        {
            int bossNumber = (levelNr / 5) - 1;
            Random r = new Random();
            int monsterIndex = r.Next(1, InternalMonsterCount);

            var item = monsters.ToArray()[monsterIndex];
            Sprite = item.Value; //hack code because this dictionary is going away
            Name = item.Key;

            //health and currency calculations.
            Health = 100 * bossNumber;
            CurrencyAmount = (ulong)Math.Pow(levelNr, 2);
        }

        public override PlayerCurrency GetReward()
        {
            return new PlayerCurrency { EuropeanCredit = 0, SparkCoin = CurrencyAmount };
        }
    }

}
