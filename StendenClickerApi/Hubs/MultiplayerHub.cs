
using System;
using Microsoft.AspNet.SignalR;

namespace StendenClickerApi.Hubs
{
	public class MultiplayerHub : Hub
	{
		//private Dictionary<string, MultiPlayerSession> Sessions;

		//private MultiplayerHubProxy multiplayerHubProxy;

		//public void broadcastSession(MultiPlayerSession session)
		//{

		//}

		//public void processBatch(IBatchProcessable _T_ batchItem)
		//{

		//}
		public void DoTestServer(string data)
        {
			Clients.Caller.doTest(data);
        }
	}

}

