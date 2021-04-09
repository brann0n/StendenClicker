using StendenClicker.Library.PlayerControls;

namespace StendenClicker.Library.AbstractMonster
{
	public interface IAbstractMonster
	{
		int GetHealth();

		void DoDamage(int damage);

		PlayerCurrency GetReward();

		bool IsDefeated();

		int GetHealthPercentage();
	}
}