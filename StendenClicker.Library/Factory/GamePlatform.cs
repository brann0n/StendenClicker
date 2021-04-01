using StendenClicker.Library.AbstractMonster;
using StendenClicker.Library.AbstractScene;
using System;
using System.Collections.Generic;
using System.Text;

namespace StendenClicker.Library.Factory
{
	public class GamePlatform
	{
		public virtual IAbstractMonster Monster { get; set; }
		public virtual IAbstractScene Scene { get; set; }
	}

	public class BossGamePlatform : GamePlatform
	{
		public new Boss Monster { get; set; }
		public new BossScene Scene { get; set; }
	}

	public class NormalGamePlatform : GamePlatform
	{
		public new Normal Monster { get; set; }
		public new NormalScene Scene { get; set; }
	}
}
