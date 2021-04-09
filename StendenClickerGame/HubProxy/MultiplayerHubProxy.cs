using Microsoft.AspNet.SignalR.Client;
using StendenClicker.Library;
using StendenClicker.Library.AbstractScene;
using StendenClicker.Library.Batches;
using StendenClicker.Library.Factory;
using StendenClicker.Library.Models;
using StendenClicker.Library.Multiplayer;
using StendenClicker.Library.PlayerControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StendenClickerGame.Multiplayer
{
	public class MultiplayerHubProxy
	{
		private const string ServerURL = "https://stendenclicker.serverict.nl/signalr";

		//Singleton Variables
		private static readonly Lazy<MultiplayerHubProxy> instance = new Lazy<MultiplayerHubProxy>(() =>
		{
			MultiplayerHubProxy proxy = new MultiplayerHubProxy();
			proxy.InitProxyAsync(ServerURL);
			return proxy;
		});

		public static MultiplayerHubProxy Instance { get { return instance.Value; } }

		//signalR required objects
		private HubConnection hubConnection;
		private IHubProxy MultiPlayerHub;

		//signalR required events
		public delegate void SignalRConnectionStateHandler(StateChange state);
		public delegate void SignalRConnectionError(Exception excteption);
		public event SignalRConnectionStateHandler OnConnectionStateChanged;
		public event SignalRConnectionError OnConnectionError;

		public event EventHandler InitializeComplete;
		public event EventHandler OnInviteReceived;
		public event EventHandler OnSessionUpdateReceived;

		//Batch click handlers
		public delegate BatchedClick BatchClickRetrieveHandler();
		public event BatchClickRetrieveHandler OnRequireBatches;
		public event EventHandler OnBatchesReceived;

		//Required classes for player information and level generation
		public ApiPlayerHandler PlayerContext;
		public LevelGenerator LevelGenerator;

		//internal session
		private MultiPlayerSession SessionContext;

		public Player CurrentPlayer { get { return SessionContext?.CurrentPlayerList?.FirstOrDefault(n => n.UserId.ToString() == CurrentPlayerGuid); } }
		public string CurrentPlayerGuid { get; set; }

		public MultiplayerHubProxy()
		{
			PlayerContext = new ApiPlayerHandler();
			LevelGenerator = new LevelGenerator();
		}

		private async void InitProxyAsync(string serverUrl)
		{
			//perform async initiating operations.
			var pl = await PlayerContext.GetPlayerStateAsync(DeviceInfo.Instance.GetSystemId());
			CurrentPlayerGuid = pl.UserId.ToString();

			hubConnection = new HubConnection(serverUrl);
			hubConnection.Headers.Add("UserGuid", CurrentPlayerGuid);
			MultiPlayerHub = hubConnection.CreateHubProxy("MultiplayerHub");
			hubConnection.StateChanged += HubConnection_StateChanged;
			await hubConnection.Start().ContinueWith(async task =>
			{
				if (task.IsFaulted)
				{
					OnConnectionError?.Invoke(task.Exception);
					//could also be unauthorized due to no correct userguid
				}
				else
				{
					//connected:
					MultiPlayerHub.On<MultiPlayerSession>("updateSession", sessionObject => UpdateSession(sessionObject));
					MultiPlayerHub.On<List<Player>, NormalGamePlatform, bool>("receiveNormalMonsterBroadcast", ReceiveNormalMonsterBroadcast);
					MultiPlayerHub.On<List<Player>, BossGamePlatform, bool>("receiveBossMonsterBroadcast", ReceiveBossMonsterBroadcast);
					MultiPlayerHub.On("broadcastYourClicks", RequestClickBatches);
					MultiPlayerHub.On<BatchedClick>("receiveUploadedBatchClicks", ReceiveUploadedBatchClicks);
					MultiPlayerHub.On<InviteModel>("receiveInvite", ReceiveInvite);
				}

				//for now render a new level anyways.
				SessionContext = new MultiPlayerSession { CurrentPlayerList = new List<Player> { pl }, HostPlayerId = CurrentPlayerGuid };
				SessionContext.CurrentLevel = LevelGenerator.BuildLevel(SessionContext.CurrentPlayerList);
				await BroadcastSessionToServer();
			});

			InitializeComplete?.Invoke(null, null);

			Dictionary<string, string> dingetje = new Dictionary<string, string>
			{
				{ "sessionguid", CurrentPlayerGuid }
			};
			await RestHelper.GetRequestAsync("api/multiplayer/AddSession", dingetje);
		}

		#region ServerInvokableMethods
		private void ReceiveInvite(InviteModel invite)
		{
			//update the UI
			OnInviteReceived?.Invoke(invite, null);
		}

		private void ReceiveNormalMonsterBroadcast(List<Player> players, NormalGamePlatform pl, bool force)
		{
			MultiPlayerSession session = new MultiPlayerSession
			{
				CurrentPlayerList = players,
				CurrentLevel = new GamePlatform() { Monster = pl.Monster, Scene = pl.Scene },
				ForceUpdate = force
			};

			UpdateSession(session);
		}

		private void ReceiveBossMonsterBroadcast(List<Player> players, BossGamePlatform pl, bool force)
		{
			MultiPlayerSession session = new MultiPlayerSession
			{
				CurrentPlayerList = players,
				CurrentLevel = new GamePlatform() { Monster = pl.Monster, Scene = pl.Scene },
				ForceUpdate = force
			};

			UpdateSession(session);
		}

		private async void UpdateSession(MultiPlayerSession session)
		{
			//got a session update from the server. this happens when someone joins your session, or you are playing the game.
			var dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;
			await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
			{
				OnSessionUpdateReceived?.Invoke(session, null);
			});
		}

		private async void ReceiveUploadedBatchClicks(BatchedClick CollectedDamage)
		{
			var dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;
			await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
			{
				OnBatchesReceived?.Invoke(CollectedDamage, null);
			});
		}

		private void RequestClickBatches()
		{
			BatchedClick clicks = OnRequireBatches?.Invoke();
			if (clicks.GetDamage() > 0)
			{
				//if clicks is a valid object, serialize it and broadcast it to the server.
				MultiPlayerHub.Invoke<BatchedClick>("uploadBatchedClicks", clicks);
			}
		}
		#endregion

		public async Task BroadcastSessionToServer()
		{
			if (SessionContext.CurrentLevel.Scene is BossScene scene)
			{
				BossGamePlatform p = new BossGamePlatform
				{
					Monster = (StendenClicker.Library.AbstractMonster.Boss)SessionContext.CurrentLevel.Monster,
					Scene = scene
				};

				await MultiPlayerHub.Invoke("broadcastSessionBoss", SessionContext.HostPlayerId, SessionContext.CurrentPlayerList, p);
			}
			else
			{
				NormalGamePlatform p = new NormalGamePlatform
				{
					Monster = (StendenClicker.Library.AbstractMonster.Normal)SessionContext.CurrentLevel.Monster,
					Scene = (NormalScene)SessionContext.CurrentLevel.Scene
				};

				await MultiPlayerHub.Invoke<bool>("broadcastSessionNormal", SessionContext.HostPlayerId, SessionContext.CurrentPlayerList, p).ContinueWith((task) =>
				{
					bool success = task.Result;
				});
			}
		}

		public MultiPlayerSession GetContext()
		{
			return SessionContext;
		}

		private void HubConnection_StateChanged(StateChange obj)
		{
			OnConnectionStateChanged?.Invoke(obj);
		}

		public async Task SendInvite(string targetPlayerGuid)
		{
			await MultiPlayerHub.Invoke("sendInvite", targetPlayerGuid);
		}

		public async Task JoinFriend(string friendId)
		{
			await MultiPlayerHub.Invoke<bool>("joinFriend", friendId);
		}
	}
}