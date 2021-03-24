using Microsoft.AspNet.SignalR.Client;
using StendenClicker.Library.Batches;
using StendenClicker.Library.Multiplayer;
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

		private static readonly Lazy<MultiplayerHubProxy> instance = new Lazy<MultiplayerHubProxy>(() => new MultiplayerHubProxy(ServerURL));
		public static MultiplayerHubProxy Instance { get { return instance.Value; } }

		private MultiPlayerSession SessionContext;
		private HubConnection hubConnection;
		private IHubProxy MultiPlayerHub;

		public delegate void SignalRConnectionStateHandler(StateChange state);
		public delegate void SignalRConnectionError(Exception excteption);

		public delegate BatchedClick BatchClickRetrieveHandler();

		public event BatchClickRetrieveHandler OnRequireBatches;

		public event SignalRConnectionStateHandler OnConnectionStateChanged;
		public event SignalRConnectionError OnConnectionError;

		public MultiplayerHubProxy(string serverUrl)
		{
			hubConnection = new HubConnection(serverUrl);
			MultiPlayerHub = hubConnection.CreateHubProxy("MultiplayerHub");
			hubConnection.StateChanged += HubConnection_StateChanged;
			hubConnection.Start().ContinueWith(task =>
			{
				if (task.IsFaulted)
				{
					OnConnectionError?.Invoke(task.Exception);
				}
				else
				{
					//connected:
					MultiPlayerHub.On<MultiPlayerSession>("updateSession", updateSession);
					MultiPlayerHub.On("receiveUpdate", receiveUpdate);
					MultiPlayerHub.On("requestClickBatch", requestClickBatches);
				}
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

		private void HubConnection_StateChanged(StateChange obj)
		{
			OnConnectionStateChanged?.Invoke(obj);
		}
	}

}

