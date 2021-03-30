
using StendenClicker.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StendenClicker.Library.AbstractScene
{
	public class NormalScene : AbstractScene
	{
		public static List<Models.DatabaseModels.Scene> NormalScenes = new List<Models.DatabaseModels.Scene>();
        public static int InternalSceneCount { get { return NormalScenes.Count; } }

        public static async Task Initialize()
		{           
            NormalScenes = await LocalPlayerData.LoadLocalData<List<Models.DatabaseModels.Scene>>("normal-scenes-asset-data.json");
            if(NormalScenes == null)
			{
                var response = await RestHelper.GetRequestAsync("api/Assets/scenes");
                NormalScenes = RestHelper.ConvertJsonToObject<List<Models.DatabaseModels.Scene>>(response.Content);
                if (NormalScenes != null)
                {
                    await LocalPlayerData.SaveLocalData(NormalScenes, "normal-scenes-asset-data.json");
                }
                else
                {
                    NormalScenes = await LocalPlayerData.LoadLocalData<List<Models.DatabaseModels.Scene>>("normal-scenes-asset-data.json");
                }
            }
        }

        public NormalScene(PlayerState state) : base(state)
        {
            int levelId = state.LevelsDefeated % InternalSceneCount + 1;

            var item = NormalScenes.FirstOrDefault(n => n.SceneId == levelId);
            if (item == null) throw new Exception("No scenes were loaded, make sure you have an internet connection.");
            Background = item.SceneAsset.Base64Image;
            Name = item.SceneName;

            int currentMonster = (state.MonstersDefeated % 5);

            MonsterCount = 5;
            CurrentMonster = currentMonster;
        }
    }
}
