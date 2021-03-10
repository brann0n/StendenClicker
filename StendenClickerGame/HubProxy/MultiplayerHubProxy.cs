using Microsoft.AspNet.SignalR.Client;
using StendenClicker.Library.Multiplayer;
using System;
using System.Net;

namespace StendenClickerGame.Multiplayer
{
	public class MultiplayerHubProxy
	{
		private MultiPlayerSession SessionContext;
		private HubConnection hubConnection;
		private IHubProxy MultiPlayerHub;

		public delegate void SignalRConnectionStateHandler(StateChange state);
		public delegate void SignalRConnectionError(Exception excteption);

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
				}
			}).Wait();		
        }

        public void BroadcastSessionToServer(MultiPlayerSession session)
        {
			MultiPlayerHub.Invoke<MultiPlayerSession>("broadcastSession",session);
        }

		public void ProcessBatchOnServer()
        {

        }

		private void updateSession(MultiPlayerSession session)
		{
			//got a session update from the server. TODO -> decide if the current session should be overwritten
		}

		private void receiveUpdate()
		{
			//received a game update from the server, decide what to do with this information.
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

