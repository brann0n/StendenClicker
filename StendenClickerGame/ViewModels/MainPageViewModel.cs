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
		public ICommand command { get; set; }
		public CurrencyTrayViewModel CurrencyTray { get; set; }
		public ObservableCollection<heroes> HeroList { get; set; }
        public ObservableCollection<abilities> AbilitiesList { get; set; }
        public ObservableCollection<Coins> CoinList { get; set; }

		public string dingetje { get; set; } = "iVBORw0KGgoAAAANSUhEUgAAAfQAAAH0CAQAAABh3xcBAAAEOUlEQVR42u3TQQ0AAAgDsc2/aHjigbQSLrlmAjxXo4PRAaMDRgeMDhgdMDpgdMDoYHTA6IDRAaMDRgeMDhgdMDoYHTA6YHTA6IDRAaMDRgeMDkY3OhgdMDpgdMDogNEBowNGB4wORgeMDhgdMDpgdMDogNEBo4PRAaMDRgeMDhgdMDpgdMDoYHQRwOiA0QGjA0YHjA4YHTA6YHQwOmB0wOiA0QGjA0YHjA4YHYwOGB0wOmB0wOiA0QGjA0YHowNGB4wOGB0wOmB0wOiA0QGjg9EBowNGB4wOGB0wOmB0wOhgdMDogNEBowNGB4wOGB0wOhgdMDpgdMDogNEBowNGB4wOGB2MDhgdMDpgdMDogNEBowNGB6MDRgeMDhgdMDpgdMDogNHB6IDRAaMDRgeMDhgdMDpgdMDoYHTA6IDRAaMDRgeMDhgdMDoYHTA6YHTA6IDRAaMDRgeMDkYHjA4YHTA6YHTA6IDRAaMDRgejA0YHjA4YHTA6YHTA6IDRweiA0QGjA0YHjA4YHTA6YHQwOmB0wOiA0QGjA0YHjA4YHTA6GB0wOmB0wOiA0QGjA0YHjA5GB4wOGB0wOmB0wOiA0QGjg9EBowNGB4wOGB0wOmB0wOiA0cHogNEBowNGB4wOGB0wOmB0MDpgdMDogNEBowNGB4wOGB2MDhgdMDpgdMDogNEBowNGB4wORgeMDhgdMDpgdMDogNEBo4PRAaMDRgeMDhgdMDpgdMDoYHTA6IDRAaMDRgeMDhgdMDoY3ehgdMDogNEBowNGB4wOGB0wOhgdMDpgdMDogNEBowNGB4wORgeMDhgdMDpgdMDogNEBo4PRjQ5GB4wOGB0wOmB0wOiA0QGjg9EBowNGB4wOGB0wOmB0wOhgdMDogNEBowNGB4wOGB0wOhhdBDA6YHTA6IDRAaMDRgeMDhgdjA4YHTA6YHTA6IDRAaMDRgejA0YHjA4YHTA6YHTA6IDRweiA0QGjA0YHjA4YHTA6YHTA6GB0wOiA0QGjA0YHjA4YHTA6GB0wOmB0wOiA0QGjA0YHjA5GB4wOGB0wOmB0wOiA0QGjA0YHowNGB4wOGB0wOmB0wOiA0cHogNEBowNGB4wOGB0wOmB0MDpgdMDogNEBowNGB4wOGB0wOhgdMDpgdMDogNEBowNGB4wORgeMDhgdMDpgdMDogNEBo4PRAaMDRgeMDhgdMDpgdMDogNHB6IDRAaMDRgeMDhgdMDpgdDA6YHTA6IDRAaMDRgeMDhgdjA4YHTA6YHTA6IDRAaMDRgeMDkYHjA4YHTA6YHTA6IDRAaOD0QGjA0YHjA4YHTA6YHTA6GB0wOiA0QGjA0YHjA4YHTA6YHQwOmB0wOiA0QGjA0YHjA4YHYwOGB0wOmB0wOiA0QGjA0YHowNGB4wOGB0wOmB0wOiA0QGjg9EBowNGB4wOGB0wOmB04Czv1vQQBjIIIQAAAABJRU5ErkJggg==";

		public MainPageViewModel()
		{
			command = new RelayCommand(clearCoinlist);

			//sub viewmodels
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

			CheckContextVariables();
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

		public void clearCoinlist()
		{
			CoinList.Clear();
		}
	}	
}
