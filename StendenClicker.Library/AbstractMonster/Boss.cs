using StendenClicker.Library.Models;
using StendenClicker.Library.PlayerControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StendenClicker.Library.AbstractMonster
{
	public class Boss : AbstractMonster
	{
		private static List<Models.DatabaseModels.Boss> Bosses;
		private static int InternalBossCount { get { return Bosses == null ? 0 : Bosses.Count; } }

		public Boss()
		{

		}

		public static async Task Initialize()
		{
			var response = await RestHelper.GetRequestAsync("api/Assets/bosses");
			Bosses = RestHelper.ConvertJsonToObject<List<Models.DatabaseModels.Boss>>(response.Content);
			if (Bosses != null && Bosses?.Count != 0)
			{
				await LocalPlayerData.SaveLocalData(Bosses, "bosses-asset-data.json");
			}
			else
			{
				Bosses = await LocalPlayerData.LoadLocalData<List<Models.DatabaseModels.Boss>>("bosses-asset-data.json");
			}
		}

		public Boss(PlayerState state)
		{
			//the first 7 bosses need to be in order, then they can be randomized
			int bossNumber = (state.MonstersDefeated / 5);

			Random r = new Random();
			if (bossNumber >= InternalBossCount && InternalBossCount != 0)
			{
				//this means all hero's are unlocked, now you can randomize the boss sprites
				bossNumber = r.Next(1, InternalBossCount + 1);
			}

			var item = Bosses.FirstOrDefault(n => n.BossId == bossNumber);
			if (item == null) throw new Exception("No bosses were loaded, make sure you have an internet connection.");
			Sprite = item.BossAsset.Base64Image;
			Name = item.BossName;

			MonsterLevel = state.MonstersDefeated + 1;

			//health of the boss is 200 times its own boss number
			Health = item.BaseHealth * this.MonsterLevel;

			//currency is 3 ec per boss and a large amount of spark coins
			CurrencyAmount = (ulong)Math.Pow(state.LevelsDefeated, 2);
		}

		public override PlayerCurrency GetReward()
		{
			if (CurrencyAmount > 25)
				return new PlayerCurrency { EuropeanCredit = 3, SparkCoin = 25, Factor = CurrencyAmount / 25 };
			else
				return new PlayerCurrency { EuropeanCredit = 3, SparkCoin = CurrencyAmount };
		}
	}
}
