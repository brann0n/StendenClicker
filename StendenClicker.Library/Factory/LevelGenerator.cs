using StendenClicker.Library.AbstractPlatform;
using StendenClicker.Library.Models;
using StendenClicker.Library.PlayerControls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StendenClicker.Library.Factory
{
	public class LevelGenerator
	{
		public const int LevelsUntilBoss = 5;

		public IAbstractPlatform BuildLevel(List<Player> players)
		{
			PlayerState state = CalculateState(players);
			if (CalculateIfBoss(state))
			{
				return new BossLevel(state);
			}
			else
			{
				return new NormalLevel(state);
			}
		}

		private bool CalculateIfBoss(PlayerState state)
		{
			return (state.LevelsDefeated % LevelsUntilBoss == 0) && state.LevelsDefeated != 0;
		}

		private PlayerState CalculateState(List<Player> players)
		{
			if(players.Where(p => Player.IsPlayerObjectEmpty(p)).Count() != 0)
			{
				//this means there are empty player objects
				throw new Exception("Empty player objects passed into CalculateState function.");
			}

			PlayerState state;
			if (players.Count == 1)
			{
				//get the singleplayer state
				state = players.First().State;
			}
			else
			{
				//define these stats of the current lobby
				int HighestLevelsDefeated = players.Max(n => n.State.LevelsDefeated);
				int LowestLevelsDefeated = players.Min(n => n.State.LevelsDefeated);

				int HighestMonstersDefeated = players.Max(n => n.State.MonstersDefeated);
				int LowestMonstersDefeated = players.Min(n => n.State.MonstersDefeated);

				double AverageLevelsDefeated = players.Average(n => n.State.LevelsDefeated);
				double AverageMonstersDefeated = players.Average(n => n.State.MonstersDefeated);

				//create new playerstate from above variables
				state = new PlayerState { LevelsDefeated = LowestLevelsDefeated, MonstersDefeated = LowestMonstersDefeated };
			}
			return state;
		}
    }

}

