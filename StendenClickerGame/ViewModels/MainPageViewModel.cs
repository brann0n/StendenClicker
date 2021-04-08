using Microsoft.AspNet.SignalR.Client;
using StendenClicker.Library;
using StendenClicker.Library.Factory;
using StendenClicker.Library.Models;
using StendenClicker.Library.Multiplayer;
using StendenClicker.Library.PlayerControls;
using StendenClickerGame.Data;
using StendenClickerGame.Multiplayer;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PlayerHero = StendenClicker.Library.Models.DatabaseModels.PlayerHero;

namespace StendenClickerGame.ViewModels
{
	public class MainPageViewModel : ViewModelBase
	{
		public MultiplayerHubProxy mpProxy { get { return MultiplayerHubProxy.Instance; } }

		private int _width { get; set; } = 1920;
		private int _height { get; set; } = 1080;
		public int WindowHeight { get => _height; set { _height = value; NotifyPropertyChanged(); } }
		public int WindowWidth { get => _width; set { _width = value; NotifyPropertyChanged(); } }

		public CurrencyTrayViewModel CurrencyTray { get; set; }
		public KoffieMachineViewModel KoffieMachine { get; set; }
		public FriendshipPanelViewmodel Friends { get; set; }

		public ObservableCollection<HeroListObject> HeroList { get; set; }

		public List<Player> CurrentPlayers { get => mpProxy?.getContext()?.CurrentPlayerList.Where(n => n.UserId != mpProxy?.CurrentPlayer.UserId).ToList(); }
		private Player CurrentPlayer { get { return MultiplayerHubProxy.Instance.CurrentPlayer; } }
		public MainPageViewModel()
		{
			//sub viewmodels
			CurrencyTray = new CurrencyTrayViewModel();
			KoffieMachine = new KoffieMachineViewModel();
			Friends = new FriendshipPanelViewmodel();

			//Herolist
			HeroList = new ObservableCollection<HeroListObject>();

			CheckContextVariables();
		}

		public void LoadHeroes()
		{
			foreach (Hero h in Hero.Heroes)
			{
				//todo: add in player specific information from the list.

				int levelNeededForUnlock = (h.Id * 5);
				bool isLevelUnlocked = CurrentPlayer.State.MonstersDefeated >= levelNeededForUnlock && CurrentPlayer.State.BossesDefeated >= h.Id;


				HeroListObject heroListObject;
				PlayerHero heroObject = CurrentPlayer.Heroes.FirstOrDefault(n => n.Hero.HeroName == h.Name);

				if (heroObject != null)
				{
					heroListObject = new HeroListObject { Hero = h, PlayerHeroInformation = heroObject, HeroUnlocked = isLevelUnlocked };
				}
				else
				{
					heroListObject = new HeroListObject { Hero = h , HeroUnlocked = isLevelUnlocked };
				}

				HeroList.Add(heroListObject); //auto locks all heroes
			}
		}

		public void CheckContextVariables()
		{
			//multiplayer connection
			mpProxy.OnConnectionStateChanged += MpProxy_OnConnectionStateChanged;
			mpProxy.OnRequireBatches += MpProxy_OnRequireBatches;
			mpProxy.InitializeComplete += MpProxy_InitializeComplete;

			mpProxy.OnInviteReceived += MpProxy_OnInviteReceived;
			mpProxy.OnSessionUpdateReceived += MpProxy_OnSessionUpdateReceived;

			CurrencyTray.OnMonsterDefeated += CurrencyTray_OnMonsterDefeated;
		}

		private async void CurrencyTray_OnMonsterDefeated(object sender, EventArgs e)
		{
			await mpProxy.PlayerContext.SetPlayerStateAsync(CurrencyTray.CurrentPlayer);

			//unlock any new heroes perhaps.
			UpdateHeroList();
		}

		private void UpdateHeroList()
		{
			foreach(HeroListObject item in HeroList)
			{
				int levelNeededForUnlock = (item.Hero.Id * 5);
				item.HeroUnlocked = CurrentPlayer.State.MonstersDefeated >= levelNeededForUnlock && CurrentPlayer.State.BossesDefeated >= item.Hero.Id;
				item.NotifyPropertyChanged("HeroUnlocked");
				item.NotifyPropertyChanged("OpacityEnabled");
				item.NotifyPropertyChanged("HeroBought");
			}
		}

		/// <summary>
		/// Can only process current level and current playerlist, other objects are defaulted.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MpProxy_OnSessionUpdateReceived(object sender, EventArgs e)
		{
			MultiPlayerSession session = (MultiPlayerSession)sender;
			//todo: check if the session is already loaded.
			CurrencyTray.CurrentSession.CurrentLevel = session.CurrentLevel;
			CurrencyTray.CurrentSession.CurrentPlayerList = session.CurrentPlayerList;

			NotifyPropertyChanged("CurrentPlayers");
			NotifyPropertyChanged("CurrencyTray");
		}

		private void MpProxy_OnInviteReceived(object sender, EventArgs e)
		{
			Friends.AddPendingInvite((InviteModel)sender);
		}

		private void MpProxy_InitializeComplete(object sender, EventArgs e)
		{
			NotifyPropertyChanged("CurrencyTray");
			Friends.InitializeFriendship(mpProxy.CurrentPlayer.UserId.ToString());

			//render the heroes with player context.
			LoadHeroes();
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
			return await mpProxy.PlayerContext.GetPlayerStateAsync(DeviceInfo.Instance.GetSystemId());
		}

	}
}
