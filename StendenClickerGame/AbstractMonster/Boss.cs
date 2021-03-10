
using StendenClickerGame.CurrencyObjects;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace StendenClickerGame.AbstractMonster
{
	public class Boss : AbstractMonster
	{
		private static readonly Dictionary<string, string> bosses;

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
