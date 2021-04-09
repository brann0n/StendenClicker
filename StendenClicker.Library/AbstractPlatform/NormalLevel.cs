using StendenClicker.Library.AbstractMonster;
using StendenClicker.Library.AbstractScene;
using StendenClicker.Library.Models;

namespace StendenClicker.Library.AbstractPlatform
{
	public class NormalLevel : AbstractPlatform
	{
		public NormalLevel(PlayerState state) : base(state) { }

		public override IAbstractMonster GetMonster()
		{
			return new Normal(CurrentPlayerState);
		}

		public override IAbstractScene GetScene()
		{
			return new NormalScene(CurrentPlayerState);
		}
	}
}
