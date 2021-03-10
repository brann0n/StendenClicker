using Microsoft.AspNet.SignalR.Client;
using System.Net;

namespace StendenClickerGame.Multiplayer
{
	public class MultiplayerHubProxy
	{
		private MultiPlayerSession SessionContext;

		//http://localhost:50120/

		private HubConnection hubConnection;
		private IHubProxy MultiPlayerHub;

		public MultiplayerHubProxy()
        {
			

			hubConnection = new HubConnection("http://localhost:50120/signalr");
			MultiPlayerHub = hubConnection.CreateHubProxy("MultiplayerHub");
			hubConnection.Start().ContinueWith(task => 
			{
                if (task.IsFaulted)
                {

                }
                else
                {
					//connected:
					MultiPlayerHub.On<string>("doTest", (string1) =>
					{
						string dummy1 = string1;
					});
				}
			}).Wait();

			
        }

		public void SendTestToServer()
        {
			MultiPlayerHub.Invoke<string>("doTestServer","Dit is een test");
        }

		public void updateSession(MultiPlayerSession session)
		{

		}

		public void receiveUpdate()
		{

		}

		public MultiPlayerSession getContext()
		{
			return null;
		}

	}

}

