
using System;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR;

namespace StendenClickerApi.Hubs
{
	public class MultiplayerHub : Hub
	{
        private Dictionary<string, MultiPlayerSession> Sessions;

        //private MultiplayerHubProxy multiplayerHubProxy;

        public void broadcastSession(MultiPlayerSession session)
        {

        }

        public void processBatch<T>(IBatchProcessable<T> batchItem)
        {

        }

    }

}

