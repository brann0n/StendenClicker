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
        public ActionResult GetMonsters()
		{
            //todo: get all regular monster assets from the database

            return Json("");
		}

        public ActionResult GetHeros()
		{
            //todo: get all heros and their info from the database

            return Json("");
		}

        public ActionResult GetScenes()
		{
            //todo: get all scenes from the database

            return Json("");
		}
    }
}