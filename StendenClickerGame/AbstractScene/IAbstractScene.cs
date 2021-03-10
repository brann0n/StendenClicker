using System;
using Windows.UI.Xaml.Controls;

namespace StendenClickerGame.AbstractScene
{
	public interface IAbstractScene
	{
		Image getBackground();

		int getMonsterCount();

		int getCurrentMonster();
	}
}
