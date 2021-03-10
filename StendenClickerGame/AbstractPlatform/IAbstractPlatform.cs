

using StendenClickerGame.AbstractMonster;
using StendenClickerGame.AbstractScene;

namespace StendenClickerGame.AbstractPlatform
{
	public interface IAbstractPlatform
	{
		IAbstractMonster getMonster();

		IAbstractScene getScene();

	}

}
