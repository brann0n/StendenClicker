using StendenClicker.Library.AbstractPlatform;
using StendenClicker.Library.AbstractScene;
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

		public GamePlatform BuildLevel(List<Player> players)
		{
			PlayerState state = CalculateState(players);

			IAbstractPlatform pl;

			if (CalculateIfBoss(state))
			{
				pl = new BossLevel(state); 
			}
			else
			{
				pl = new NormalLevel(state);				
			}

			return new GamePlatform()
			{
				Monster = (AbstractMonster.AbstractMonster)pl.getMonster(),
				//Scene = new NormalScene(true) {MonsterCount = 5, CurrentMonster = 1 }
				Scene = (AbstractScene.AbstractScene)pl.getScene()
			};
		}

		private bool CalculateIfBoss(PlayerState state)
		{
			bool CurrentBossIsDefeated = state.MonstersDefeated / LevelsUntilBoss == state.BossesDefeated;
			bool ShouldNextLevelBeBoss = state.MonstersDefeated % LevelsUntilBoss == 0;

			return ShouldNextLevelBeBoss && !CurrentBossIsDefeated && state.MonstersDefeated != 0;
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
				int HighestMonstersDefeated = players.Max(n => n.State.MonstersDefeated);
				int LowestMonstersDefeated = players.Min(n => n.State.MonstersDefeated);
				double AverageMonstersDefeated = players.Average(n => n.State.MonstersDefeated);

				int HighestBossesDefeated = players.Max(n => n.State.BossesDefeated);
				int LowestBossesDefeated = players.Min(n => n.State.BossesDefeated);
				double AverageBossesDefeated = players.Average(n => n.State.BossesDefeated);

				//create new playerstate from above variables
				state = new PlayerState { MonstersDefeated = (int)AverageMonstersDefeated, BossesDefeated = (int)AverageBossesDefeated };
			}
			return state;
		}
    }

}

