
using StendenClicker.Library.CurrencyObjects;
using System.Collections.Generic;

namespace StendenClicker.Library.AbstractMonster
{
	public class Normal : AbstractMonster
	{
		private static readonly Dictionary<string, string> monsters;

		private Currency currency;

        public override void doDamage(int damage)
        {
            throw new System.NotImplementedException();
        }

        public override int getHealth()
        {
            throw new System.NotImplementedException();
        }

        public override Image getMonsterAsset()
        {
            throw new System.NotImplementedException();
        }

        public override Currency getReward()
        {
            throw new System.NotImplementedException();
        }
    }

}
