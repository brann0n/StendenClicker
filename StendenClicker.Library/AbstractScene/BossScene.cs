using StendenClicker.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StendenClicker.Library.AbstractScene
{
	public class BossScene : AbstractScene
	{
        public BossScene(PlayerState state) : base(state)
        {
            Random r = new Random();
            int SceneNumber = r.Next(1, NormalScene.InternalSceneCount);

            var item = NormalScene.NormalScenes.FirstOrDefault(n => n.SceneId == SceneNumber);
            if (item == null) throw new Exception("No scenes were loaded, make sure you have an internet connection.");
            Background = item.SceneAsset.Base64Image;
            Name = item.SceneName;

            MonsterCount = 1;
            CurrentMonster = 0;
        }
    }

}
