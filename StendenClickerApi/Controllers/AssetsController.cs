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
            List<Monster> test = db.Monsters.ToList();

            return new JsonStringResult(JsonConvert.SerializeObject(test));
		}

        public ActionResult GetHeroes()
		{
            return new JsonStringResult(JsonConvert.SerializeObject(db.Heroes));
        }

        public ActionResult GetPlayers()
        {
            List<Player> test = db.Players.ToList();
            return new JsonStringResult(JsonConvert.SerializeObject(test));
        }

        public ActionResult GetScenes()
		{
            //todo: get all scenes from the database

            return Json("");
		}
    }

    public class JsonStringResult : ContentResult
    {
        public JsonStringResult(string json)
        {
            Content = json;
            ContentType = "application/json";
        }
    }
}