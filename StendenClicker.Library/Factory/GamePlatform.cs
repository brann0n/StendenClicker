using StendenClicker.Library.AbstractMonster;
using StendenClicker.Library.AbstractScene;
using System;
using System.Collections.Generic;
using System.Text;

namespace StendenClicker.Library.Factory
{
	public class GamePlatform
	{
		public IAbstractMonster Monster { get; set; }	
		public IAbstractScene Scene { get; set; }
	}
}
