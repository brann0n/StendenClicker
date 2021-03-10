using StendenClickerGame.CurrencyObjects;
using Windows.UI.Xaml.Controls;

namespace StendenClickerGame.AbstractMonster
{
	public abstract class AbstractMonster : IAbstractMonster
	{
		private int health;

		private int currencyAmount;

        public abstract void doDamage(int damage);
        public abstract int getHealth();
        public abstract Image getMonsterAsset();
        public abstract Currency getReward();
    }

}

