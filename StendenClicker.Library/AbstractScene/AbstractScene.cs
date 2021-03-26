using System;

namespace StendenClicker.Library.AbstractScene
{
	public abstract class AbstractScene : IAbstractScene
	{
		public int CurrentMonster { get; set; }
		
		public int MonsterCount { get; set; }
		
		public string Background { get; set; }
		
		public string Name { get; set; }

		public abstract Image getBackground();
        public abstract int getCurrentMonster();
        public abstract int getMonsterCount();
		public abstract string getName();
    }

}
