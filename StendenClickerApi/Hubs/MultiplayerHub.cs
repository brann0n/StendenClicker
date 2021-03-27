using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using StendenClicker.Library.Batches;
using StendenClicker.Library.Multiplayer;
using StendenClickerApi.Database;
using MultiPlayerSession = StendenClicker.Library.Multiplayer.MultiPlayerSession;

namespace StendenClickerApi.Hubs
{
	public class MultiplayerHub : Hub
	{
		private static Dictionary<string, MultiPlayerSession> Sessions = new Dictionary<string, MultiPlayerSession>();

		private Database.StendenClickerDatabase db = new Database.StendenClickerDatabase();

		public override Task OnConnected()
		{
			string connectionId = Context.ConnectionId;
			string userGuid = Context.Headers.Get("UserGuid");

			//check if that userguid is in the database.
			Player p = db.Players.FirstOrDefault(n => n.PlayerGuid == userGuid);

			if(p == null) //NOTE: users are created in the production database, multiplayer connects with the local database.
			{
				//player does not exists in database yet, send request to client to abort this session.
			}
			else
			{
				//player exists, create a new multiplayer session with current player as host, if the host wants to join another player, this session will be abandoned.
				//tell the client that it can subscribe to the batched click function so the server can periodically receive its clicks.
			}

			return base.OnConnected();
		}

		public override Task OnDisconnected(bool stopCalled)
		{
			//save the current session to the database and then dispose of the object.

			return base.OnDisconnected(stopCalled);
		}

		public override Task OnReconnected()
		{
			//find the player's most recent session and rejoin it.
			return base.OnReconnected();
		}


		public void broadcastSession(MultiPlayerSession session)
        {
			//verify the given session against the server's copy
			bool SessionIsValid = Sessions.ContainsKey(session.hostPlayerId);

			if (SessionIsValid)
			{
				//find all players connectionid's in this session
				List<string> PlayersInSession = session.CurrentPlayerList.Where(n => string.IsNullOrEmpty(n.connectionId)).Select(n => n.connectionId).ToList();


				Clients.Clients(PlayersInSession).updateSession(session);
			}
		}


		public void processBatch<T>(IBatchProcessable<T> batchItem)
        {

        }

    }

}

