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
		private MultiplayerHubProxy mpProxy { get { return MultiplayerHubProxy.Instance; } }

		public CurrencyTrayViewModel CurrencyTray { get; set; }
		public ObservableCollection<Hero> HeroList { get; set; }
        public ObservableCollection<abilities> AbilitiesList { get; set; }
        public ObservableCollection<Coins> CoinList { get; set; }

		public string dingetje { get { return ((Hero)Hero.Heroes.FirstOrDefault()).Base64Image; } }
		public MainPageViewModel()
		{
			//sub viewmodels
			CurrencyTray = new CurrencyTrayViewModel();

			//asl test om te kijken of de heros aan de shop worden toegevoegd
			HeroList = new ObservableCollection<Hero>();
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

			CheckContextVariables();
		}

		public void LoadHeroes()
		{
			foreach(Hero h in Hero.Heroes)
			{
				//todo: add in player specific information from the list.
				HeroList.Add(h);
			}
		}

		public void CheckContextVariables()
		{
			//multiplayer connection
			mpProxy.OnConnectionStateChanged += MpProxy_OnConnectionStateChanged;
			mpProxy.OnRequireBatches += MpProxy_OnRequireBatches;
		}

		private StendenClicker.Library.Batches.BatchedClick MpProxy_OnRequireBatches()
		{
			return CurrencyTray.GetBatchedClick();
		}

		private void MpProxy_OnConnectionStateChanged(StateChange state)
        {
			//todo: handle state changes, if it cant connect there might not be an internet connection
        }

		public async Task<Player> GetPlayerContextAsync()
		{
			return await mpProxy.PlayerContext.GetPlayerStateAsync(DeviceInfo.Instance.Id);
		}
	}	
}
