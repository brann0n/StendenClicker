
using StendenClicker.Library.CurrencyObjects;
using StendenClicker.Library.PlayerControls;
using System.Collections.Generic;

namespace StendenClicker.Library.AbstractMonster
{
	public interface IAbstractMonster
	{
		int GetHealth();

		void DoDamage(int damage);

		PlayerCurrency GetReward();

		Image GetMonsterAsset();

		bool IsDefeated();
	}

}
