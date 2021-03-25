
using StendenClicker.Library.CurrencyObjects;
using System.Collections.Generic;
using System;
using System.Linq;
using StendenClicker.Library.PlayerControls;
using System.Threading.Tasks;

namespace StendenClicker.Library.AbstractMonster
{
    public class Boss : AbstractMonster
    {
        private static List<Models.DatabaseModels.Boss> Bosses;
        private static int InternalBossCount { get { return Bosses.Count; } }

        static Boss()
		{
            
		}

        public static async Task Initialize()
		{
            var response = await RestHelper.GetRequestAsync("api/Assets/bosses");
            Bosses = RestHelper.ConvertJsonToObject<List<Models.DatabaseModels.Boss>>(response.Content);
            if (Bosses != null)
            {
                await LocalPlayerData.SaveLocalData(Bosses, "bosses-asset-data.json");
            }
            else
            {
                Bosses = await LocalPlayerData.LoadLocalData<List<Models.DatabaseModels.Boss>>("bosses-asset-data.json");
            }
        }

        public Boss(int levelNr)
        {
            //the first 7 bosses need to be in order, then they can be randomized
            int bossNumber = (levelNr / 5) - 1;

            Random r = new Random();
            if (bossNumber >= InternalBossCount && InternalBossCount != 0)
            {
                //this means all hero's are unlocked, now you can randomize the boss sprites
                bossNumber = r.Next(1, InternalBossCount);
            }

            var item = Bosses.FirstOrDefault(n => n.BossId == bossNumber);
            if (item == null) throw new Exception("No bosses were loaded, make sure you have an internet connection.");
            Sprite = item.BossAsset.Base64Image; 
            Name = item.BossName;

            //health of the boss is 200 times its own boss number
            Health = item.BaseHealth * bossNumber;

            //currency is 3 ec per boss and a large amount of spark coins
            CurrencyAmount = (ulong)Math.Pow(levelNr, 3);
        }

        public override PlayerCurrency GetReward()
        {
            return new PlayerCurrency { EuropeanCredit = 3, SparkCoin = CurrencyAmount};
        }
    }

}
