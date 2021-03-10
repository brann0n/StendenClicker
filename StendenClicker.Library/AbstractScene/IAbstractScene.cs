using System;

namespace StendenClicker.Library.AbstractScene
{
	public interface IAbstractScene
	{
		Image getBackground();

		int getMonsterCount();

		int getCurrentMonster();
	}
}
