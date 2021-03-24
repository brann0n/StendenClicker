
using System;
using System.Collections.Generic;
using System.Linq;

namespace StendenClicker.Library.AbstractScene
{
	public class NormalScene : AbstractScene
	{
		private static List<Models.DatabaseModels.Scene> NormalScenes;
        private static int InternalSceneCount { get { return NormalScenes.Count; } }

        static NormalScene()
        {
            var response = RestHelper.GetRequestAsync("api/Assets/scenes").GetAwaiter().GetResult();
            NormalScenes = RestHelper.ConvertJsonToObject<List<Models.DatabaseModels.Scene>>(response.Content);
            if (NormalScenes != null)
            {
                LocalPlayerData.SaveLocalData(NormalScenes, "scenes-asset-data.json");
            }
            else
            {
                NormalScenes = LocalPlayerData.LoadLocalData<List<Models.DatabaseModels.Scene>>("scenes-asset-data.json").GetAwaiter().GetResult();
            }
        }

        public NormalScene()
        {
            Random r = new Random();
            int SceneNumber = r.Next(1, InternalSceneCount);

            var item = NormalScenes.FirstOrDefault(n => n.SceneId == SceneNumber);
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
