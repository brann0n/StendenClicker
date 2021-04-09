using Microsoft.AspNet.SignalR;
using StendenClicker.Library.Multiplayer;
using StendenClickerApi.Helpers;
using StendenClickerApi.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace StendenClickerApi.Controllers
{
	[RoutePrefix("api/multiplayer")]
	public class MultiplayerController : Controller
	{

		private static Dictionary<string, Action> tasklist = new Dictionary<string, Action>();

		/// <summary>
		/// Call this function when you want to perform actions per session.
		/// </summary>
		public static async Task RunTasks()
		{
			if(tasklist.Count != 0)
			{
				Parallel.Invoke(tasklist.Values.ToArray());				
			}

			await Task.Yield();
		}

		[ApiKeySecurity, HttpGet, Route("AddSession")]
		public ActionResult AddSession(string sessionguid)
		{
			if (SessionExtensions.ContainsKey(sessionguid))
			{
				MultiPlayerSession currsession = SessionExtensions.Get(sessionguid);

				tasklist.Add(currsession.hostPlayerId, () =>
				{
					if (SessionExtensions.ContainsKey(currsession.hostPlayerId))
					{
						MultiPlayerSession currentActualSession = SessionExtensions.Get(currsession.hostPlayerId);

						var clientList = currentActualSession.CurrentPlayerList.Select(n => n.UserId.ToString()).ToList();

						IHubContext multiplayerHub = GlobalHost.ConnectionManager.GetHubContext<MultiplayerHub>();
						multiplayerHub.Clients.Groups(clientList).broadcastYourClicks();
					}
					else
					{
						//remove this session.
						RemoveSession(currsession.hostPlayerId);
					}
				});
			}
			else
			{
				return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Session not found.");
			}

			return new HttpStatusCodeResult(HttpStatusCode.OK, "Multiplayer session was added to the tasklist.");
		}

		[ApiKeySecurity, HttpGet, Route("RemoveSession")]
		public ActionResult RemoveSession(string sessionguid)
		{
			if (SessionExtensions.ContainsKey(sessionguid))
			{
				tasklist.Remove(sessionguid);
				return new HttpStatusCodeResult(HttpStatusCode.OK, "Multiplayer session was removed from the tasklist.");
			}
			return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Session not found.");
		}

	}
}