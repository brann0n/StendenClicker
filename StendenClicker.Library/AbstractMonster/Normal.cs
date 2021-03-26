
using StendenClicker.Library.CurrencyObjects;
using StendenClicker.Library.Models;
using StendenClicker.Library.PlayerControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StendenClicker.Library.AbstractMonster
{
	public class Normal : AbstractMonster
	{
        //dictionary mapped with monster name and sprite location
        private static List<Models.DatabaseModels.Monster> Monsters;
        private static int InternalMonsterCount { get { return Monsters.Count; } }

        private static int previousId { get; set; } = 0;


        public static async Task Initialize()
		{
            var response = await RestHelper.GetRequestAsync("api/Assets/monsters");
            Monsters = RestHelper.ConvertJsonToObject<List<Models.DatabaseModels.Monster>>(response.Content);
            if (Monsters != null)
            {
                await LocalPlayerData .SaveLocalData(Monsters, "monsters-asset-data.json");
            }
            else
            {
                Monsters = await LocalPlayerData.LoadLocalData<List<Models.DatabaseModels.Monster>>("monsters-asset-data.json");
            }
        }

        public Normal(PlayerState state)
        {
            Random r = new Random();
            int monsterIndex = r.Next(1, InternalMonsterCount + 1);

            while(monsterIndex == previousId)
			{
                monsterIndex = r.Next(1, InternalMonsterCount + 1);
            }

            previousId = monsterIndex;

            var item = Monsters.FirstOrDefault(n => n.MonsterId == monsterIndex);

            if (item == null) throw new Exception("No monsters were loaded, make sure you have an internet connection.");
            Sprite = item.MonsterAsset.Base64Image; 
            Name = item.MonsterName;

            MonsterLevel = state.MonstersDefeated + 1;
            //health and currency calculations.
            Health = item.BaseHealth * this.MonsterLevel;
            CurrencyAmount = (ulong)Math.Pow(state.LevelsDefeated, 2);
           
        }

        public override PlayerCurrency GetReward()
        {
            return new PlayerCurrency { EuropeanCredit = 0, SparkCoin = CurrencyAmount };
        }
    }

}
