﻿using Newtonsoft.Json;
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
        [Route("Monsters")]
        public ActionResult GetMonsters()
		{
            return new JsonStringResult(JsonConvert.SerializeObject(db.Monsters));
		}
        [Route("Bosses")]
        public ActionResult GetBosses()
		{
            return new JsonStringResult(JsonConvert.SerializeObject(db.Bosses));
		}
        [Route("Heroes")]
        public ActionResult GetHeroes()
		{
            return new JsonStringResult(JsonConvert.SerializeObject(db.Heroes));
        }
        [Route("Scenes")]
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