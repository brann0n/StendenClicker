using StendenClicker.Library.Abilities;
using System.Collections.Generic;

namespace StendenClicker.Library.PlayerControls
{
	public class Hero
	{
		public static readonly List<Models.DatabaseModels.Hero> Heroes;
		private static int InternalBossCount { get { return Heroes.Count; } }

		static Hero()
		{
			var response = RestHelper.GetRequestAsync("api/Assets/bosses").GetAwaiter().GetResult();
			Heroes = RestHelper.ConvertJsonToObject<List<Models.DatabaseModels.Hero>>(response.Content);
			if (Heroes != null)
			{
				LocalPlayerData.SaveLocalData(Heroes, "bosses-asset-data.json");
			}
			else
			{
				Heroes = LocalPlayerData.LoadLocalData<List<Models.DatabaseModels.Hero>>("bosses-asset-data.json").GetAwaiter().GetResult();
			}
		}


		private string Name;

		private List<HeroUpgrades> Upgrades;

		public Image GetImageSprite()
		{
			return null;
		}
	}
}

