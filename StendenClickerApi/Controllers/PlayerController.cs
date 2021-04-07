using StendenClickerApi.Database;
using StendenClickerApi.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StendenClickerApi.Controllers
{
    [RoutePrefix("api/player")]
    public class PlayerController : Controller
    {
        StendenClickerDatabase db = new StendenClickerDatabase();

        /// <summary>
        /// Endpoint that allows you to retrieve the player information
        /// </summary>
        /// <param name="device_id"></param>
        /// <returns></returns>
        [ApiKeySecurity, HttpGet, Route("Get")]
        public ActionResult GetPlayer(string device_id)
        {
            if (string.IsNullOrEmpty(device_id)) throw new Exception("No device_id provided on request.");

            List<Player> playerList = db.Players.ToList();

            var player = db.Players.FirstOrDefault(n => n.DeviceId == device_id);
            if (player == null)
            {
                return new HttpStatusCodeResult(500, "No player found with this deviceid");
            }
            return new JsonStringResult(player);
        }

        [ApiKeySecurity, HttpPost, Route("Set")]
        public HttpStatusCodeResult SetPlayer(Player player)
        {
            if (player == null) return new HttpStatusCodeResult(500, "no player");

            //check if playerobject is okay
            var dbPlayer = db.Players.FirstOrDefault(b => b.DeviceId == player.DeviceId);
            if (dbPlayer == null) return new HttpStatusCodeResult(401, "player not found");

            //maybe do a check that the object is actually valid before sending it into the databse
            //db.Players.AddOrUpdate(player);
            //dbPlayer.

            db.SaveChanges();

            return new HttpStatusCodeResult(200, "player updated");
        }

        [ApiKeySecurity, HttpPost, Route("Create")]
        public HttpStatusCodeResult CreatePlayer(Player player)
        {
            if (player == null) return new HttpStatusCodeResult(500, "Player object not present");

            if (Player.IsPlayerObjectEmpty(player))
            {
                return new HttpStatusCodeResult(401, "Player object is missing data");
            }

            db.Players.Add(player);
            db.SaveChanges();

            return new HttpStatusCodeResult(200, "Created the player");
        }

        [ApiKeySecurity, HttpGet, Route("Friendships")]
        public ActionResult GetFriendships(string PlayerId)
        {
            List<Friendship> friendships = db.Friendships
                    .Where(n => n.Player1.PlayerGuid == PlayerId || n.Player2.PlayerGuid == PlayerId)
                    .ToList();

            if(friendships == null)
            {
                return new HttpStatusCodeResult(404, "No friendships were found with player id [" + PlayerId + "]");
            }

            return new JsonStringResult(friendships);
        }

        [ApiKeySecurity, HttpGet, Route("GetAccountsByNameSearch")]
        public ActionResult GetAccountsByNameSearch(string name, string user)
        {
            if (string.IsNullOrEmpty(name)) return new HttpStatusCodeResult(500, "search name is empty.");
            if (name.Length > 128) return new HttpStatusCodeResult(500, "Searched for a too long string.");

            List<Player> players = db.Players.Where(n => n.PlayerName.Contains(name.Trim()) && n.PlayerGuid != user).ToList();

            //remove existing friends from above list
            List<Friendship> friendships = db.Friendships
                    .Where(n => n.Player1.PlayerGuid == user || n.Player2.PlayerGuid == user)
                    .ToList();

            List<string> FriendIds = friendships.Select(n => n.Player1.PlayerGuid).ToList();
            FriendIds.AddRange(friendships.Select(n => n.Player2.PlayerGuid));

            List<Player> ReturnNonFriends = new List<Player>();

            foreach(Player p in players)
			{
				if (!FriendIds.Contains(p.PlayerGuid))
				{
                    ReturnNonFriends.Add(p);
				}
			}

            return new JsonStringResult(ReturnNonFriends);
        }

        [ApiKeySecurity, HttpPost, Route("CreateFriendship")]
        public async Task<ActionResult> CreateFriendship(List<Player> playerList)
        {
            if (playerList == null) return new HttpStatusCodeResult(500, "No player list received.");
            if (playerList.Count != 2) return new HttpStatusCodeResult(500, "Player list didnt contain 2 players.");

            string guid1 = playerList[0].PlayerGuid;
            string guid2 = playerList[1].PlayerGuid;

            //check both player objects with the empty checker
            Player friend1 = db.Players.FirstOrDefault(n => n.PlayerGuid == guid1);
            Player friend2 = db.Players.FirstOrDefault(n => n.PlayerGuid == guid2);

            if (Player.IsPlayerObjectEmpty(friend1) && Player.IsPlayerObjectEmpty(friend2)) return new HttpStatusCodeResult(500, "Player objects didnt validate.");

            Friendship friendship = new Friendship();
            friendship.Player1 = friend1;
            friendship.Player2 = friend2;

            db.Friendships.Add(friendship);
            await db.SaveChangesAsync();

            return new HttpStatusCodeResult(200, "Friendship was created.");
        }

        [ApiKeySecurity, HttpGet, Route("IsUsernameAvailable")]
        public ActionResult IsUsernameAvailable(string username)
        {
            if (string.IsNullOrEmpty(username)) return new HttpStatusCodeResult(500, "Player search name is empty.");

            Player user = db.Players.FirstOrDefault(n => n.PlayerName.ToLower() == username.ToLower());

            if (user == null)
            {
                return new HttpStatusCodeResult(200, "true");
            }

            return new HttpStatusCodeResult(200, "false");
        }

        [ApiKeySecurity, HttpPost, Route("DeleteFriendship")]
        public async Task<ActionResult> DeleteFriendship(List<string> playerGuidList)
        {
            if (playerGuidList == null) return new HttpStatusCodeResult(500, "No player list received.");
            if (playerGuidList.Count != 2) return new HttpStatusCodeResult(500, "Player list didnt contain 2 players.");

            string guid1 = playerGuidList[0];
            string guid2 = playerGuidList[1];

            //check both player objects with the empty checker
            Player friend1 = db.Players.FirstOrDefault(n => n.PlayerGuid == guid1);
            Player friend2 = db.Players.FirstOrDefault(n => n.PlayerGuid == guid2);

            if (Player.IsPlayerObjectEmpty(friend1) && Player.IsPlayerObjectEmpty(friend2)) return new HttpStatusCodeResult(500, "Player objects didnt validate.");

            Friendship friendship = db.Friendships
                    .Where(n => n.Player1 == friend1 || n.Player2 == friend1)
                    .Where(n => n.Player1 == friend2 || n.Player2 == friend2)
                    .FirstOrDefault();

            if(friendship == null)
            {
                return new HttpStatusCodeResult(404, "Friendship has not been found");
            }

            db.Friendships.Remove(friendship);
            await db.SaveChangesAsync();

            return new HttpStatusCodeResult(200, "Friendship was deleted.");
        }
    }
}