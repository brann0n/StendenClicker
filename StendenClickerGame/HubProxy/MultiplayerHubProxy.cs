using Microsoft.AspNet.SignalR.Client;
using StendenClicker.Library;
using StendenClicker.Library.Batches;
using StendenClicker.Library.Factory;
using StendenClicker.Library.Multiplayer;
using StendenClicker.Library.PlayerControls;
using System;
using System.Net;
using System.Threading.Tasks;

namespace StendenClickerGame.Multiplayer
{
	public class MultiplayerHubProxy
	{

#if DEBUG
		private const string ServerURL = "http://localhost:50120/signalr";
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
			CurrentPlayer = await PlayerContext.GetPlayerStateAsync(DeviceInfo.Instance.Id);

			hubConnection = new HubConnection(serverUrl);
			hubConnection.Headers.Add("UserGuid", CurrentPlayer.UserId.ToString());
			MultiPlayerHub = hubConnection.CreateHubProxy("MultiplayerHub");
			hubConnection.StateChanged += HubConnection_StateChanged;
			await hubConnection.Start().ContinueWith(task =>
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
					MultiPlayerHub.On("receiveUpdate", receiveUpdate);
					MultiPlayerHub.On("requestClickBatch", requestClickBatches);

					//do what next?

				}

				//for now render a new level anyways.
				SessionContext = new MultiPlayerSession { CurrentPlayerList = new System.Collections.Generic.List<Player> { CurrentPlayer } };
				SessionContext.CurrentLevel = LevelGenerator.BuildLevel(SessionContext.CurrentPlayerList);
			});

			InitializeComplete?.Invoke(null, null);
		}

		#region ServerInvokableMethods
		private void updateSession(MultiPlayerSession session)
		{
			//got a session update from the server. TODO -> decide if the current session should be overwritten
			SessionContext = session;
		}

		private void receiveUpdate()
		{
			//received a game update from the server, decide what to do with this information.
		}

		private void requestClickBatches()
		{
			BatchedClick clicks = OnRequireBatches?.Invoke();

			//if clicks is a valid object, serialize it and broadcast it to the server along with some player information.
			MultiPlayerHub.Invoke<BatchedClick>("uploadBatchedClicks", clicks);
		}
		#endregion

		public void BroadcastSessionToServer(MultiPlayerSession session)
		{
			MultiPlayerHub.Invoke<MultiPlayerSession>("broadcastSession", session);
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
	}

}

