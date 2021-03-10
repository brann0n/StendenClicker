using Microsoft.AspNet.SignalR.Client;
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

		}

		private void receiveUpdate()
		{

		}

		public MultiPlayerSession getContext()
		{
			return null;
		}
		private void HubConnection_StateChanged(StateChange obj)
		{
			OnConnectionStateChanged?.Invoke(obj);
		}
	}

}

