
using StendenClicker.Library.CurrencyObjects;
using StendenClicker.Library.PlayerControls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StendenClicker.Library.AbstractMonster
{
	public class Normal : AbstractMonster
	{
        //dictionary mapped with monster name and sprite location
        private static readonly List<Models.DatabaseModels.Monster> Monsters;
        private static int InternalMonsterCount { get { return Monsters.Count; } }

        static Normal()
        {
            var response = RestHelper.GetRequestAsync("api/Assets/monsters").GetAwaiter().GetResult();
            Monsters = RestHelper.ConvertJsonToObject<List<Models.DatabaseModels.Monster>>(response.Content);
            if (Monsters != null)
            {
                LocalPlayerData.SaveLocalData(Monsters, "monsters-asset-data.json");
            }
            else
            {
                Monsters = LocalPlayerData.LoadLocalData<List<Models.DatabaseModels.Monster>>("monsters-asset-data.json").GetAwaiter().GetResult();
            }
        }
        public Normal(int levelNr)
        {
            int bossNumber = (levelNr / 5) - 1;
            Random r = new Random();
            int monsterIndex = r.Next(1, InternalMonsterCount);

            var item = Monsters.FirstOrDefault(n => n.MonsterId == monsterIndex);

            if (item == null) throw new Exception("No monsters were loaded, make sure you have an internet connection.");
            Sprite = item.MonsterAsset.Base64Image; 
            Name = item.MonsterName;


            //health and currency calculations.
            Health = 100 * bossNumber;
            CurrencyAmount = (ulong)Math.Pow(levelNr, 2);
        }

        public override PlayerCurrency GetReward()
        {
            return new PlayerCurrency { EuropeanCredit = 0, SparkCoin = CurrencyAmount };
        }
    }

}
