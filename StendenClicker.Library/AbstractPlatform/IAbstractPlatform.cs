using StendenClicker.Library.AbstractMonster;
using StendenClicker.Library.AbstractScene;

namespace StendenClicker.Library.AbstractPlatform
{
	public interface IAbstractPlatform
	{
		IAbstractMonster GetMonster();

		IAbstractScene GetScene();
	}
}
