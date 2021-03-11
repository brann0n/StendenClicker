

using StendenClicker.Library.AbstractMonster;
using StendenClicker.Library.AbstractScene;
using StendenClicker.Library.Models;

namespace StendenClicker.Library.AbstractPlatform
{
    public class BossLevel : AbstractPlatform
    {
        public BossLevel(PlayerState state) : base(state) { }

        public override IAbstractMonster getMonster()
        {
            return new Boss(CurrentPlayerState.LevelsDefeated + 1);
        }

        public override IAbstractScene getScene()
        {
            return new BossScene();
        }
    }

}
