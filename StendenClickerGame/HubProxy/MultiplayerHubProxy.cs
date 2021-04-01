using Microsoft.AspNet.SignalR.Client;
using StendenClicker.Library;
using StendenClicker.Library.Batches;
using StendenClicker.Library.Factory;
using StendenClicker.Library.Models;
using StendenClicker.Library.Multiplayer;
using StendenClicker.Library.PlayerControls;
using System;
using System.Net;
using System.Threading.Tasks;

namespace StendenClickerGame.Multiplayer
{
	public class MultiplayerHubProxy
	{

#if !DEBUG
		private const string ServerURL = "http://localhost:50420/signalr";
#else
		private const string ServerURL = "https://stendenclicker.serverict.nl/signalr";
#endif
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

		//Required classes for player information and level generation
		public ApiPlayerHandler PlayerContext;
		public LevelGenerator LevelGenerator;

		//internal session
		private MultiPlayerSession SessionContext;

		public Player CurrentPlayer { get; private set; }


		public MultiplayerHubProxy()
		{
			PlayerContext = new ApiPlayerHandler();
			LevelGenerator = new LevelGenerator();		
		}

		private async void InitProxyAsync(string serverUrl)
		{
			//perform async initiating operations.
			CurrentPlayer = await PlayerContext.GetPlayerStateAsync(DeviceInfo.Instance.GetSystemId());

			hubConnection = new HubConnection(serverUrl);
			hubConnection.Headers.Add("UserGuid", CurrentPlayer.UserId.ToString());
			MultiPlayerHub = hubConnection.CreateHubProxy("MultiplayerHub");
			hubConnection.StateChanged += HubConnection_StateChanged;
			await hubConnection.Start().ContinueWith(async task =>
			{
				if (task.IsFaulted)
				{
					OnConnectionError?.Invoke(task.Exception);
					//could also be unauthorized due to no correct userguid
					//if this happens go into offline mode and load the most recent player information. then render a level out of that.
				}
				else
				{
					//connected:
					MultiPlayerHub.On<MultiPlayerSession>("updateSession", updateSession);
					MultiPlayerHub.On("requestClickBatch", requestClickBatches);
					MultiPlayerHub.On<InviteModel>("receiveInvite", receiveInvite);

					//do what next?
					//await MultiPlayerHub.Invoke("beginGameThread"); //tells the server it can start a thread for this user.
				}

				//for now render a new level anyways.
				SessionContext = new MultiPlayerSession { CurrentPlayerList = new System.Collections.Generic.List<Player> { CurrentPlayer } };
				SessionContext.CurrentLevel = LevelGenerator.BuildLevel(SessionContext.CurrentPlayerList);
				
			});
			BroadcastSessionToServer(); //perform the first broadcast.
			InitializeComplete?.Invoke(null, null);
		}

		#region ServerInvokableMethods
		private async void receiveInvite(InviteModel invite)
		{
			//update the UI
			OnInviteReceived?.Invoke(invite, null);
		}

		private async void updateSession(MultiPlayerSession session)
		{
			//got a session update from the server. this happens when someone joins your session, or you are playing the game.
			var dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;
			await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
			{
				OnSessionUpdateReceived?.Invoke(session, null);
			});
								
		}

		private void requestClickBatches()
		{
			BatchedClick clicks = OnRequireBatches?.Invoke();

			//if clicks is a valid object, serialize it and broadcast it to the server along with some player information.
			MultiPlayerHub.Invoke<BatchedClick>("uploadBatchedClicks", clicks);
		}
		#endregion

		public void BroadcastSessionToServer()
		{
			MultiPlayerHub.Invoke<MultiPlayerSession>("broadcastSession", SessionContext);
		}

		public void ProcessBatchOnServer()
		{
			requestClickBatches();
		}

		public MultiPlayerSession getContext()
		{
			return SessionContext;
		}

		public string GetConnectionId()
		{
			return hubConnection.ConnectionId;
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
			await MultiPlayerHub.Invoke<bool>("joinFriend", friendId).ContinueWith((task) => 
			{
				//check if joining the session succeded.
				bool success = task.Result;
				if (success)
				{
					//render the new session and wait for updates.
				}
				else
				{
					//show the user that joining the game failed.
				}
			});
		}
	}

}

