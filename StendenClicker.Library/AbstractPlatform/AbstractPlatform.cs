using StendenClicker.Library.AbstractMonster;
using StendenClicker.Library.AbstractScene;
using StendenClicker.Library.Models;

namespace StendenClicker.Library.AbstractPlatform
{
	public abstract class AbstractPlatform : IAbstractPlatform
	{
		public PlayerState CurrentPlayerState { get; set; }

		public AbstractPlatform(PlayerState state)
		{
			CurrentPlayerState = state;
		}

		public abstract IAbstractMonster GetMonster();

		public abstract IAbstractScene GetScene();
	}
}
