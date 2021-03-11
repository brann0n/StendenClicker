using StendenClicker.Library.AbstractMonster;
using StendenClicker.Library.AbstractScene;
using StendenClicker.Library.Models;

namespace StendenClicker.Library.AbstractPlatform
{
    public class NormalLevel : AbstractPlatform
    {
        public NormalLevel(PlayerState state) : base(state) { }

        public override IAbstractMonster getMonster()
        {
            return new Normal(CurrentPlayerState.LevelsDefeated);
        }

        public override IAbstractScene getScene()
        {
            return new NormalScene();
        }
    }

}
