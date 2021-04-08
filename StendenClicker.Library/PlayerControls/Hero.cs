using StendenClicker.Library.Models.DatabaseModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StendenClicker.Library.PlayerControls
{
	public class Hero
	{
		public static List<Models.DatabaseModels.Hero> Heroes;
		private static int InternalBossCount { get { return Heroes.Count; } }

		public static async Task Initialize()
		{
			var response = await RestHelper.GetRequestAsync("api/Assets/heroes");
			Heroes = RestHelper.ConvertJsonToObject<List<Models.DatabaseModels.Hero>>(response.Content);
			if (Heroes != null)
			{
				await LocalPlayerData.SaveLocalData(Heroes, "heroes-asset-data.json");
			}
			else
			{
				Heroes = await LocalPlayerData.LoadLocalData<List<Models.DatabaseModels.Hero>>("heroes-asset-data.json");
			}
		}

		public string Name { get; set; }
		public string Base64Image { get; set; }
		public int HeroLevel { get; set; }
		public int Price { get; set; }

		public List<Upgrade> Upgrades { get; set; }

		public Image GetImageSprite()
		{
			return null;
		}

		public static implicit operator Hero(Models.DatabaseModels.Hero hero)
		{
			return new Hero
			{
				Base64Image = hero.HeroAsset.Base64Image,
				Name = hero.HeroName,
				HeroLevel = 0,
				Price = hero.HeroCost,
				Upgrades = hero.Upgrades?.ToList() //todo: this is null, fill in these fields with a seperate web call to upgrades.
			};
		}
	}
}

