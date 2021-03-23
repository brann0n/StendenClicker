using System;

namespace StendenClicker.Library.AbstractScene
{
	public abstract class AbstractScene : IAbstractScene
	{
		protected int CurrentMonster;

		protected int MonsterCount;

		protected string Background;

		protected string Name;

		public abstract Image getBackground();
        public abstract int getCurrentMonster();
        public abstract int getMonsterCount();
		public abstract string getName();
    }

}
