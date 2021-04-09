using Newtonsoft.Json;
using StendenClickerApi.Database;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StendenClickerApi.Controllers
{
    [RoutePrefix("api/Assets")]
    public class AssetsController : Controller
    {
        private readonly StendenClickerDatabase db = new StendenClickerDatabase();

        //GET: api/Assets/GetMonsters
        [Route("Monsters")]
        public ActionResult GetMonsters()
		{
            List<Monster> Monsters = new List<Monster>();
            int counter = 1;
            foreach(Monster m in db.Monsters)
			{
                m.MonsterId = counter++;
                Monsters.Add(m);
			}

            return new JsonStringResult(JsonConvert.SerializeObject(Monsters));
		}
        [Route("Bosses")]
        public ActionResult GetBosses()
		{
            List<Boss> Bosses = new List<Boss>();
            int counter = 1;
            foreach (Boss m in db.Bosses)
            {
                m.BossId = counter++;
                Bosses.Add(m);
            }
            return new JsonStringResult(JsonConvert.SerializeObject(Bosses));
		}
        [Route("Heroes")]
        public ActionResult GetHeroes()
		{
            List<Hero> Heroes = new List<Hero>();
            int counter = 1;
            foreach (Hero m in db.Heroes)
            {
                m.HeroId = counter++;
                Heroes.Add(m);
            }
            return new JsonStringResult(JsonConvert.SerializeObject(Heroes));
        }
        [Route("Scenes")]
        public ActionResult GetScenes()
		{
            List<Scene> Scenes = new List<Scene>();
            int counter = 1;
            foreach (Scene m in db.Scenes)
            {
                m.SceneId = counter++;
                Scenes.Add(m);
            }
            return new JsonStringResult(JsonConvert.SerializeObject(Scenes));
        }
    }

    public class JsonStringResult : ContentResult
    {
        public JsonStringResult(string json)
        {
            Content = json;
            ContentType = "application/json";
        }

        public JsonStringResult(object obj)
        {
            Content = JsonConvert.SerializeObject(obj);
            ContentType = "application/json";
        }
    }
}