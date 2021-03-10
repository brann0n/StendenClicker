
using StendenClicker.Library.CurrencyObjects;

namespace StendenClicker.Library.AbstractMonster
{
	public interface IAbstractMonster
	{
		int getHealth();

		void doDamage(int damage);

		Currency getReward();

		Image getMonsterAsset();

	}

}
