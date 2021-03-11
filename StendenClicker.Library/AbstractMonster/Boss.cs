
using StendenClicker.Library.CurrencyObjects;
using System.Collections.Generic;
using System;
using System.Linq;
using StendenClicker.Library.PlayerControls;

namespace StendenClicker.Library.AbstractMonster
{
    public class Boss : AbstractMonster
    {
        private static readonly Dictionary<string, string> Bosses;
        private static readonly int InternalBossCount = 7;

        public Boss(int levelNr)
        {
            //the first 7 bosses need to be in order, then they can be randomized
            int bossNumber = (levelNr / 5) - 1;

            Random r = new Random();
            if (bossNumber >= InternalBossCount)
            {
                //this means all hero's are unlocked, now you can randomize the boss sprites
                bossNumber = r.Next(1, InternalBossCount);
            }

            var item = Bosses.ToArray()[bossNumber];
            Sprite = item.Value; //hack code because this dictionary is going away
            Name = item.Key;

            //health of the boss is 200 times its own boss number
            Health = 200 * bossNumber;

            //currency is 3 ec per boss and a large amount of spark coins
            CurrencyAmount = (ulong)Math.Pow(levelNr, 3);
        }

        public override PlayerCurrency GetReward()
        {
            return new PlayerCurrency { EuropeanCredit = 3, SparkCoin = CurrencyAmount};
        }
    }

}
