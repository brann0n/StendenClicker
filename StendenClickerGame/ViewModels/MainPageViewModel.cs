using Microsoft.AspNet.SignalR.Client;
using StendenClicker.Library;
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

		private ApiPlayerHandler playerContext;
		private Multiplayer.MultiplayerHubProxy mpProxy;
		public ICommand command { get; set; }
		public CurrencyTrayViewModel CurrencyTray { get; set; }
		public ObservableCollection<heroes> HeroList { get; set; }
        public ObservableCollection<abilities> AbilitiesList { get; set; }
        public ObservableCollection<Coins> CoinList { get; set; }

		public MainPageViewModel()
		{

			playerContext = new ApiPlayerHandler();
			mpProxy = new MultiplayerHubProxy("http://localhost:50120/signalr");
			mpProxy.OnConnectionStateChanged += MpProxy_OnConnectionStateChanged;
			CurrencyTray = new CurrencyTrayViewModel();

			//asl test om te kijken of de heros aan de shop worden toegevoegd
			HeroList = new ObservableCollection<heroes>()
			{
				new heroes{ HeroName = "De Tester", HeroLevel = "150", HeroCurrencyAmount = "180K"},
				new heroes{ HeroName = "De Popper", HeroLevel = "150", HeroCurrencyAmount = "140K"},
				new heroes{ HeroName = "De man die alles kan", HeroLevel = "150", HeroCurrencyAmount = "140K"},
				new heroes{ HeroName = "Mr. Euh", HeroLevel = "150", HeroCurrencyAmount = "140K"},
				new heroes{ HeroName = "Mr. NoGo", HeroLevel = "150", HeroCurrencyAmount = "140K"},
				new heroes{ HeroName = "De Cyclist", HeroLevel = "150", HeroCurrencyAmount = "140K"},
				new heroes{ HeroName = "Bram Maaghaar", HeroLevel = "150", HeroCurrencyAmount = "140K"}
			};

			AbilitiesList = new ObservableCollection<abilities>()
			{
				new abilities { AbilitieName = "Koffie", Cooldown = 100, Image = "Assets/koffie.png" }, 
				new abilities { AbilitieName = "Water", Cooldown = 100, Image = "Assets/koffie.png" }, 
				new abilities { AbilitieName = "Depresso", Cooldown = 100, Image = "Assets/koffie.png" },
				new abilities { AbilitieName = "Depresso", Cooldown = 100, Image = "Assets/koffie.png" },
				new abilities { AbilitieName = "Depresso", Cooldown = 100, Image = "Assets/koffie.png" },
				new abilities { AbilitieName = "Depresso", Cooldown = 100, Image = "Assets/koffie.png" },
				new abilities { AbilitieName = "Depresso", Cooldown = 100, Image = "Assets/koffie.png" }
			};

			CoinList = new ObservableCollection<Coins>()
			{
				new Coins { point = new Point{ X = 50, Y=0}, CurrencyName="SparkCoin", Image=""},
				new Coins { point = new Point{ X = 1, Y=10}, CurrencyName="SparkCoin", Image=""},
				new Coins { point = new Point{ X = 70, Y=20}, CurrencyName="SparkCoin", Image=""}
			};

			command = new RelayCommand(clearCoinlist);	
		}

        private void MpProxy_OnConnectionStateChanged(StateChange state)
        {
			//todo: handle state changes, if it cant connect there might not be an internet connection
        }

		public Player getPlayerContext()
		{
			return playerContext.getPlayer();
		}

		public void clearCoinlist()
		{
			CoinList.Clear();
		}
	}	
}
