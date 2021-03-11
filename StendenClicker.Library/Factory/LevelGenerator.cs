using StendenClicker.Library.AbstractPlatform;
using StendenClicker.Library.Models;
using StendenClicker.Library.PlayerControls;
using System.Collections.Generic;
using System.Linq;

namespace StendenClicker.Library.Factory
{
    public class LevelGenerator
    {

        public IAbstractPlatform buildLevel(List<Player> players)
        {
            //singleplayer
            if (players.Count == 1)
            {
                IAbstractPlatform level;
                PlayerState state = players.First().State;
                if (CalculateIfBoss(state))
                {
                    level = new BossLevel(state);                    
                }
                else
                {
                    level = new NormalLevel(state);
                }

                return level;
            }
            else
            {
                //multiplayer
                //TODO: get average and lowest numbers and do same as above
            }
            return null;
        }

        private bool CalculateIfBoss(PlayerState state)
        {
            return (state.LevelsDefeated % 5 == 0) && state.LevelsDefeated != 0;
        }
    }

}

