using Microsoft.AspNet.SignalR.Client;
using StendenClickerGame.Factory;
using StendenClickerGame.Multiplayer;
using StendenClickerGame.PlayerControls;

using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StendenClickerGame.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
		private LevelGenerator levelGenerator;
		private ApiPlayerHandler playerContext;
		private Multiplayer.MultiplayerHubProxy mpProxy;

		public ICommand TestCommand { get; set; }

		public MainPageViewModel()
        {
			levelGenerator = new LevelGenerator();
			playerContext = new ApiPlayerHandler();
			mpProxy = new MultiplayerHubProxy("http://localhost:50120/signalr");
            mpProxy.OnConnectionStateChanged += MpProxy_OnConnectionStateChanged;

			TestCommand = new RelayCommand(() => 
			{
				//mpProxy.SendTestToServer();
			});
        }

        private void MpProxy_OnConnectionStateChanged(StateChange state)
        {
			//todo: handle state changes, if it cant connect there might be no internet connection
        }

		public Player getPlayerContext()
		{
			return playerContext.getPlayer();
		}

		
	}	
}
