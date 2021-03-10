using System;

namespace StendenClicker.Library.AbstractScene
{
	public abstract class AbstractScene : IAbstractScene
	{
		protected int currentMonster;

		protected int monsterCount;

		protected Image background;

        public abstract Image getBackground();
        public abstract int getCurrentMonster();
        public abstract int getMonsterCount();
    }

}
