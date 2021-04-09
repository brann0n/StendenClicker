using StendenClicker.Library.AbstractMonster;
using StendenClicker.Library.AbstractScene;
using StendenClicker.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StendenClicker.Library.AbstractPlatform
{
	public abstract class AbstractPlatform : IAbstractPlatform
	{
		public PlayerState CurrentPlayerState { get; set; }

		public AbstractPlatform(PlayerState state)
		{
			CurrentPlayerState = state;
		}

		public abstract IAbstractMonster getMonster();

		public abstract IAbstractScene getScene();
	}
}
