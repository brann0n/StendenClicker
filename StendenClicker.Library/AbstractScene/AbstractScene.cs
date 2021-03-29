using StendenClicker.Library.Models;
using System;

namespace StendenClicker.Library.AbstractScene
{
	public abstract class AbstractScene : IAbstractScene
	{
		private int _currentMonster;
		private int _monsterCount;
		private string _background;
		private string _name;

		public int CurrentMonster { get => _currentMonster; set => _currentMonster = value; }
		public int MonsterCount { get => _monsterCount; set => _monsterCount = value; }
		public string Background { get => _background; set => _background = value; }
		public string Name { get => _name; set => _name = value; }

		public AbstractScene(PlayerState state)
		{

		}
	}

}
