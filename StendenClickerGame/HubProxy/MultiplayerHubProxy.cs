using Microsoft.AspNet.SignalR.Client;
using StendenClicker.Library.Batches;
using StendenClicker.Library.Factory;
using StendenClicker.Library.Multiplayer;
using StendenClicker.Library.PlayerControls;
using System;
using System.Net;

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
		private static readonly Lazy<MultiplayerHubProxy> instance = new Lazy<MultiplayerHubProxy>(() => new MultiplayerHubProxy(ServerURL));
		public static MultiplayerHubProxy Instance { get { return instance.Value; } }

		//signalR required objects
		private HubConnection hubConnection;
		private IHubProxy MultiPlayerHub;

		//signalR required events
		public delegate void SignalRConnectionStateHandler(StateChange state);
		public delegate void SignalRConnectionError(Exception excteption);
		public event SignalRConnectionStateHandler OnConnectionStateChanged;
		public event SignalRConnectionError OnConnectionError;

		//Batch click handlers
		public delegate BatchedClick BatchClickRetrieveHandler();
		public event BatchClickRetrieveHandler OnRequireBatches;

		//Required classes for player information and level generation
		public ApiPlayerHandler PlayerContext;
		public LevelGenerator LevelGenerator;

		//internal session
		private MultiPlayerSession SessionContext;

		public Player CurrentPlayer { get { return PlayerContext.GetPlayer(DeviceInfo.Instance.Id); } }


		public MultiplayerHubProxy(string serverUrl)
		{
			PlayerContext = new ApiPlayerHandler();
			LevelGenerator = new LevelGenerator();

			hubConnection = new HubConnection(serverUrl);
			hubConnection.Headers.Add("UserGuid", PlayerContext.GetPlayer(DeviceInfo.Instance.Id).UserId.ToString());
			MultiPlayerHub = hubConnection.CreateHubProxy("MultiplayerHub");
			hubConnection.StateChanged += HubConnection_StateChanged;
			hubConnection.Start().ContinueWith(task =>
			{
				if (task.IsFaulted)
				{
					OnConnectionError?.Invoke(task.Exception);
					//if this happens go into offline mode and load the most recent player information. then render a level out of that.
				}
				else
				{
					//connected:
					MultiPlayerHub.On<MultiPlayerSession>("updateSession", updateSession);
					MultiPlayerHub.On("receiveUpdate", receiveUpdate);
					MultiPlayerHub.On("requestClickBatch", requestClickBatches);
				}

				//for now render a new level anyways.
				SessionContext = new MultiPlayerSession { CurrentPlayerList = new System.Collections.Generic.List<Player> { CurrentPlayer } };
				SessionContext.CurrentLevel = LevelGenerator.BuildLevel(SessionContext.CurrentPlayerList);
			}).Wait();
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

