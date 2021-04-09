using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using StendenClicker.Library.Batches;
using StendenClicker.Library.Factory;
using StendenClicker.Library.Models;
using StendenClicker.Library.Multiplayer;
using StendenClickerApi.Database;
using StendenClickerApi.Helpers;
using MultiPlayerSession = StendenClicker.Library.Multiplayer.MultiPlayerSession;
using PlayerObject = StendenClicker.Library.PlayerControls.Player;

namespace StendenClickerApi.Hubs
{
	[UserGUIDSecurity]
	public class MultiplayerHub : Hub
	{
		private readonly StendenClickerDatabase db = new StendenClickerDatabase();

		private string UserGuid { get => Context.Headers.Get("UserGuid"); }

		public override Task OnConnected()
		{
			string connectionId = Context.ConnectionId;
			string userGuid = Context.Headers.Get("UserGuid");

			//check if that userguid is in the database.
			Player p = db.Players.FirstOrDefault(n => n.PlayerGuid == userGuid);

			if (p != null) //NOTE: users are created in the production database, multiplayer connects with the local database.
			{
				Groups.Add(Context.ConnectionId, p.PlayerGuid);

				//player exists, create a new multiplayer session with current player as host, if the host wants to join another player, this session will be abandoned.
				//tell the client that it can subscribe to the batched click function so the server can periodically receive its clicks.
				MultiPlayerSession session = new MultiPlayerSession()
				{
					HostPlayerId = p.PlayerGuid,
					CurrentPlayerList = new List<StendenClicker.Library.PlayerControls.Player> { p }
				};

				if (SessionExtensions.ContainsKey(p.PlayerGuid))
				{
					//this player already has an active session...
					SessionExtensions.Remove(p.PlayerGuid);
				}

				SessionExtensions.Add(p.PlayerGuid, session);

			}

			return base.OnConnected();
		}

		public override Task OnDisconnected(bool stopCalled)
		{
			//remove connection id from database.
			Player p = db.Players.FirstOrDefault(n => n.PlayerGuid == UserGuid);
			Groups.Remove(Context.ConnectionId, p.PlayerGuid);
			//todo: save the current session to the database and then dispose of the object.
			//todo: tell the other players in the session that someone left them, and remove the players from the game.
			return base.OnDisconnected(stopCalled);
		}

		public override Task OnReconnected()
		{
			//find the player's most recent session and rejoin it.
			Player p = db.Players.FirstOrDefault(n => n.PlayerGuid == UserGuid);
			Groups.Add(Context.ConnectionId, p.PlayerGuid);

			return base.OnReconnected();
		}

		/// <summary>
		/// Checks if friend has a session, checks if the current player is friends, adds them to their friend's session and deletes the current player session
		/// </summary>
		/// <param name="FriendId"></param>
		/// <returns></returns>
		[HubMethodName("joinFriend")]
		public async Task<bool> JoinFriend(string FriendId)
		{
			bool SessionExists = SessionExtensions.ContainsKey(FriendId);
			if (SessionExists)
			{
				Friendship fship = db.Friendships
					.Where(n => n.Player1.PlayerGuid == UserGuid || n.Player2.PlayerGuid == UserGuid)
					.Where(n => n.Player1.PlayerGuid == FriendId || n.Player2.PlayerGuid == FriendId)
					.FirstOrDefault();

				if (fship != null)
				{
					Player player = db.Players.FirstOrDefault(n => n.PlayerGuid == UserGuid);

					MultiPlayerSession FriendMultiPlayerSession = SessionExtensions.Get(FriendId);

					if (FriendMultiPlayerSession.CurrentPlayerList.Count <= 4)
					{
						List<PlayerObject> sessionMembers = new List<PlayerObject>();
						sessionMembers.AddRange(FriendMultiPlayerSession.CurrentPlayerList);
						sessionMembers.Add(player);
						SessionExtensions.UpdatePlayers(FriendMultiPlayerSession.HostPlayerId, sessionMembers);

						if (SessionExtensions.ContainsKey(UserGuid))
						{
							SessionExtensions.Remove(UserGuid);
							var targetClients = FriendMultiPlayerSession.CurrentPlayerList.Select(n => n.UserId.ToString()).ToList();

							var session = SessionExtensions.Get(FriendId);

							if (session.CurrentLevel is NormalGamePlatform)
							{
								await Clients.Groups(targetClients).receiveNormalMonsterBroadcast(session.CurrentPlayerList, session.CurrentLevel, targetClients.Count != 1);
							}
							else
							{
								await Clients.Groups(targetClients).receiveBossMonsterBroadcast(session.CurrentPlayerList, session.CurrentLevel, targetClients.Count != 1);
							}

							return true;
						}
					}
				}
			}

			return false;
		}

		[HubMethodName("broadcastSessionBoss")]
		public async Task<bool> BroadcastSession(string key, List<PlayerObject> sessionPlayers, BossGamePlatform a)
		{
			//verifys that only the host can broadcast
			if (key != UserGuid) throw new Exception("Session doesnt match the current userguid");

			//verify the given session against the server's copy
			bool SessionIsValid = SessionExtensions.ContainsKey(key);

			if (SessionIsValid)
			{
				//find all players connectionid's in this session
				List<string> PlayersInSession = sessionPlayers.Select(n => n.UserId.ToString()).ToList();

				SessionExtensions.UpdatePlayers(key, sessionPlayers);
				SessionExtensions.UpdateLevel(key, a);

				await Clients.Groups(PlayersInSession).receiveBossMonsterBroadcast(sessionPlayers, a, PlayersInSession.Count != 1);
			}

			return SessionIsValid;
		}

		[HubMethodName("broadcastSessionNormal")]
		public async Task<bool> BroadcastSessionNormal(string key, List<PlayerObject> sessionPlayers, NormalGamePlatform a)
		{
			//verifys that only the host can broadcast
			if (key != UserGuid) throw new Exception("Session doesnt match the current userguid");

			//verify the given session against the server's copy
			bool SessionIsValid = SessionExtensions.ContainsKey(key);

			if (SessionIsValid)
			{
				//find all players connectionid's in this session
				List<string> PlayersInSession = sessionPlayers.Select(n => n.UserId.ToString()).ToList();

				SessionExtensions.UpdatePlayers(key, sessionPlayers);
				SessionExtensions.UpdateLevel(key, a);
				await Clients.Groups(PlayersInSession).receiveNormalMonsterBroadcast(sessionPlayers, a, PlayersInSession.Count != 1);
			}

			return SessionIsValid;
		}

		[HubMethodName("uploadBatchedClicks")]
		public async Task UploadBatchedClicks(BatchedClick batchItem)
		{
			//send these received clicks to the other clients.
			MultiPlayerSession session = SessionExtensions.GetSessionByAnyClientId(UserGuid);

			if (session != null)
			{
				List<string> playerGuids = session.CurrentPlayerList
					.Where(m => m.UserId.ToString() != UserGuid)
					.Select(n => n.UserId.ToString()).ToList();

				await Clients.Groups(playerGuids).receiveUploadedBatchClicks(batchItem);

				await Task.Yield();
			}
		}

		[HubMethodName("sendInvite")]
		public async Task SendInvite(string targetPlayer)
		{
			//get the target player his connection id.
			Player TargetPlayer = db.Players.FirstOrDefault(n => n.PlayerGuid == targetPlayer);
			Player InviteFromPlayer = db.Players.FirstOrDefault(n => n.PlayerGuid == UserGuid);
			if (TargetPlayer == null) return;

			await Clients.Group(TargetPlayer.PlayerGuid).receiveInvite(new InviteModel { UserGuid = InviteFromPlayer.PlayerGuid, UserName = InviteFromPlayer.PlayerName });
		}
	}
}

