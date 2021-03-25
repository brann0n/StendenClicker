using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StendenClicker.Library.AbstractScene
{
	public class BossScene : AbstractScene
	{
		private static List<Models.DatabaseModels.Scene> BossScenes;
        private static int InternalSceneCount { get { return BossScenes.Count; } }

        public static async Task Initialize()
		{
            var response = await RestHelper.GetRequestAsync("api/Assets/scenes");
            BossScenes = RestHelper.ConvertJsonToObject<List<Models.DatabaseModels.Scene>>(response.Content);
            if (BossScenes != null)
            {
                await LocalPlayerData.SaveLocalData(BossScenes, "boss-scenes-asset-data.json");
            }
            else
            {
                BossScenes = await LocalPlayerData.LoadLocalData<List<Models.DatabaseModels.Scene>>("boss-scenes-asset-data.json");
            }
        }
        public BossScene()
        {
            Random r = new Random();
            int SceneNumber = r.Next(1, InternalSceneCount);

            var item = BossScenes.FirstOrDefault(n => n.SceneId == SceneNumber);
            if (item == null) throw new Exception("No scenes were loaded, make sure you have an internet connection.");
            Background = item.SceneAsset.Base64Image;
            Name = item.SceneName;
        }

        public override Image getBackground()
        {
            return new Image(Background);
        }

        public override int getCurrentMonster()
        {
            throw new System.NotImplementedException();
        }

        public override int getMonsterCount()
        {
            throw new System.NotImplementedException();
        }

        public override string getName()
        {
            return Name;
        }
    }

}
