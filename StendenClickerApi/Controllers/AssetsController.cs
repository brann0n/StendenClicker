using Newtonsoft.Json;
using StendenClickerApi.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StendenClickerApi.Controllers
{
    [RoutePrefix("api/Assets")]
    public class AssetsController : Controller
    {
        StendenClickerDatabase db = new StendenClickerDatabase();

        //GET: api/Assets/GetMonsters
        public ActionResult GetMonsters()
		{
            return new JsonStringResult(JsonConvert.SerializeObject(db.Monsters));
		}

        public ActionResult GetBosses()
		{
            return new JsonStringResult(JsonConvert.SerializeObject(db.Bosses));
		}

        public ActionResult GetHeroes()
		{
            return new JsonStringResult(JsonConvert.SerializeObject(db.Heroes));
        }

        public ActionResult GetScenes()
		{
            return new JsonStringResult(JsonConvert.SerializeObject(db.Scenes));
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