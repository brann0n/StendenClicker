
using StendenClickerGame.CurrencyObjects;
using Windows.UI.Xaml.Controls;

namespace StendenClickerGame.AbstractMonster
{
	public interface IAbstractMonster
	{
		int getHealth();

		void doDamage(int damage);

		Currency getReward();

		Image getMonsterAsset();

	}

}
