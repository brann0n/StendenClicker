using Microsoft.AspNet.SignalR.Client;
using StendenClicker.Library.Factory;
using StendenClicker.Library.PlayerControls;
using StendenClickerGame.Data;
using StendenClickerGame.Multiplayer;

using System;
using System.Collections.ObjectModel;
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

		public ObservableCollection<test> herolist { get; set; }

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

			//asl test om te kijken of de heros aan de shop worden toegevoegd
			herolist = new ObservableCollection<test>()
			{
				new test{ HeroName = "De Tester", HeroLevel = "150", HeroCurrencyAmount = "180K"},
				new test{ HeroName = "De Popper", HeroLevel = "150", HeroCurrencyAmount = "140K"},
				new test{ HeroName = "De man die alles kan", HeroLevel = "150", HeroCurrencyAmount = "140K"},
				new test{ HeroName = "Mr. Euh", HeroLevel = "150", HeroCurrencyAmount = "140K"},
				new test{ HeroName = "Mr. NoGo", HeroLevel = "150", HeroCurrencyAmount = "140K"},
				new test{ HeroName = "De Cyclist", HeroLevel = "150", HeroCurrencyAmount = "140K"},
				new test{ HeroName = "Bram Maaghaar", HeroLevel = "150", HeroCurrencyAmount = "140K"}
			};
        }

        private void MpProxy_OnConnectionStateChanged(StateChange state)
        {
			//todo: handle state changes, if it cant connect there might not be an internet connection
        }

		public Player getPlayerContext()
		{
			return playerContext.getPlayer();
		}

		
	}	
}
