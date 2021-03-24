using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using StendenClicker.Library.Batches;
using StendenClicker.Library.Multiplayer;

namespace StendenClickerApi.Hubs
{
	public class MultiplayerHub : Hub
	{
		private static Dictionary<string, MultiPlayerSession> Sessions = new Dictionary<string, MultiPlayerSession>();

		public override Task OnConnected()
		{
			string connectionId = Context.ConnectionId;
			//in this class the user should be looked up by their connection id, this changes everytime they reconnect,
			//so the onConnected might be a good way to initialize a new player object, just in case, and then let the clients request their own signing in.

			return base.OnConnected();
		}

		public override Task OnDisconnected(bool stopCalled)
		{
			return base.OnDisconnected(stopCalled);
		}

		public override Task OnReconnected()
		{
			return base.OnReconnected();
		}


		public void broadcastSession(MultiPlayerSession session)
        {

        }


		public void processBatch<T>(IBatchProcessable<T> batchItem)
        {

        }

    }

}

