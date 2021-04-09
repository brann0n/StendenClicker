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
			if (Heroes != null && Heroes?.Count != 0)
			{
				await LocalPlayerData.SaveLocalData(Heroes, "heroes-asset-data.json");
			}
			else
			{
				Heroes = await LocalPlayerData.LoadLocalData<List<Models.DatabaseModels.Hero>>("heroes-asset-data.json");
			}
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public string Base64Image { get; set; }
		public string HeroInformation { get; set; }
		public int HeroLevel { get; set; }
		public int Price { get; set; }

		public List<Upgrade> Upgrades { get; set; }

		public static implicit operator Hero(Models.DatabaseModels.Hero hero)
		{
			return new Hero
			{
				Id = hero.HeroId,
				Base64Image = hero.HeroAsset?.Base64Image,
				Name = hero.HeroName,
				HeroLevel = 0,
				Price = hero.HeroCost,
				HeroInformation = hero.HeroInformation,
				Upgrades = hero.Upgrades?.ToList() //todo: this is null, fill in these fields with a seperate web call to upgrades.
			};
		}
	}
}