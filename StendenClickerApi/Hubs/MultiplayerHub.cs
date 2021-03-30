using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using StendenClicker.Library.Batches;
using StendenClicker.Library.Multiplayer;
using StendenClickerApi.Database;
using StendenClickerApi.Helpers;
using MultiPlayerSession = StendenClicker.Library.Multiplayer.MultiPlayerSession;

namespace StendenClickerApi.Hubs
{
	[UserGUIDSecurity]
	public class MultiplayerHub : Hub
	{
		private static readonly Dictionary<string, MultiPlayerSession> Sessions = new Dictionary<string, MultiPlayerSession>();

		private readonly StendenClickerDatabase db = new StendenClickerDatabase();

		private static object SessionWriteLock = new object();

		private string UserGuid { get => Context.Headers.Get("UserGuid"); }

		public override Task OnConnected()
		{
			string connectionId = Context.ConnectionId;
			string userGuid = Context.Headers.Get("UserGuid");

			//check if that userguid is in the database.
			Player p = db.Players.FirstOrDefault(n => n.PlayerGuid == userGuid);

			if(p != null) //NOTE: users are created in the production database, multiplayer connects with the local database.
			{
				p.ConnectionId = Context.ConnectionId;
				db.SaveChanges();

				//player exists, create a new multiplayer session with current player as host, if the host wants to join another player, this session will be abandoned.
				//tell the client that it can subscribe to the batched click function so the server can periodically receive its clicks.
				MultiPlayerSession session = new MultiPlayerSession() 
				{
					hostPlayerId = p.PlayerGuid,
					CurrentPlayerList = new List<StendenClicker.Library.PlayerControls.Player> { p }
				};

				Sessions.Add(p.PlayerGuid, session);
			}

			return base.OnConnected();
		}

		public override Task OnDisconnected(bool stopCalled)
		{
			//remove connection id from database.
			Player p = db.Players.FirstOrDefault(n => n.PlayerGuid == UserGuid);
			p.ConnectionId = null;
			db.SaveChanges();

			//save the current session to the database and then dispose of the object.

			return base.OnDisconnected(stopCalled);
		}

		public override Task OnReconnected()
		{
			//find the player's most recent session and rejoin it.
			Player p = db.Players.FirstOrDefault(n => n.PlayerGuid == UserGuid);
			p.ConnectionId = Context.ConnectionId;
			db.SaveChanges();


			return base.OnReconnected();
		}

		public void beginGameThread()
		{
			//Thread.
		}

		/// <summary>
		/// Checks if friend has a session, checks if the current player is friends, adds them to their friend's session and deletes the current player session
		/// </summary>
		/// <param name="FriendId"></param>
		/// <returns></returns>
		public bool joinFriend(string FriendId)
		{
			bool SessionExists = Sessions.ContainsKey(FriendId);
			if(SessionExists)
            {
				Sessions.ContainsKey(FriendId);

				Friendship fship = db.Friendships
					.Where(n => n.Player1.PlayerGuid == UserGuid || n.Player2.PlayerGuid == UserGuid)
					.Where(n => n.Player1.PlayerGuid == FriendId || n.Player2.PlayerGuid == FriendId)
					.FirstOrDefault();
				if(fship != null)
                {
					Player p = db.Players.FirstOrDefault(n => n.PlayerGuid == UserGuid);
					MultiPlayerSession FriendMultiPlayerSession = Sessions[FriendId];
					FriendMultiPlayerSession.CurrentPlayerList.Add(p);
					if(Sessions.ContainsKey(UserGuid))
					{
						Sessions.Remove(UserGuid);
						return true;
					}
                }
            }

			return false;
		}

		public void broadcastSession(MultiPlayerSession session)
        {
			//verifys that only the host can broadcast
			if (session.hostPlayerId != UserGuid) throw new Exception("Session doesnt match the current userguid");

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

