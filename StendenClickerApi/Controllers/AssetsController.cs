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
        public ActionResult GetMonsters()
		{
            return Json(db.Monsters);
		}

        public ActionResult GetHeroes()
		{
            return Json(db.Heroes);
		}

        public ActionResult GetScenes()
		{
            //todo: get all scenes from the database

            return Json("");
		}
    }
}