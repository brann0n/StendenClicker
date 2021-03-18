using StendenClickerApi.Database;
using StendenClickerApi.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StendenClickerApi.Controllers
{
    public class PlayerController : Controller
    {
        StendenClickerDatabase db = new StendenClickerDatabase();

        /// <summary>
        /// Endpoint that allows you to retrieve the player information
        /// </summary>
        /// <param name="device_id"></param>
        /// <returns></returns>
        [ApiKeySecurity, HttpGet]
        public Player GetPlayer(string device_id)
		{
            if (string.IsNullOrEmpty(device_id)) throw new Exception("No device_id provided on request.");

            var player = db.Players.FirstOrDefault(n => n.DeviceId == device_id);
            if (player == null)
            {
                throw new Exception("No player found with this deviceid");
            }
            return player;
        }

        [ApiKeySecurity, HttpPost]
        public HttpStatusCodeResult SetPlayer(Player player)
		{
            if (player == null) return new HttpStatusCodeResult(500, "no player");

            //check if playerobject is okay
            var dbPlayer = db.Players.FirstOrDefault(b => b.DeviceId == player.DeviceId);
            if (dbPlayer == null) return new HttpStatusCodeResult(401, "player not found");

            //maybe do a check that the object is actually valid before sending it into the databse
            db.Players.AddOrUpdate(player);
            db.SaveChanges();

            return new HttpStatusCodeResult(200, "player updated");
		}
    }
}