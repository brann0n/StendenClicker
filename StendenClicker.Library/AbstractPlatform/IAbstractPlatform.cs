using StendenClicker.Library.AbstractMonster;
using StendenClicker.Library.AbstractScene;
using StendenClicker.Library.Models;

namespace StendenClicker.Library.AbstractPlatform
{
	public interface IAbstractPlatform
	{
		IAbstractMonster getMonster();

		IAbstractScene getScene();
	}
}
