using Microsoft.AspNet.SignalR.Client;
using StendenClicker.Library.Batches;
using StendenClicker.Library.Models;
using StendenClicker.Library.Multiplayer;
using StendenClicker.Library.PlayerControls;
using StendenClickerGame.Data;
using StendenClickerGame.Multiplayer;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using PlayerHero = StendenClicker.Library.Models.DatabaseModels.PlayerHero;

namespace StendenClickerGame.ViewModels
{
	public class MainPageViewModel : ViewModelBase
	{
		//static event for memento
		private static event EventHandler OnRequestSave;

		//window properties for binding a fixed width and height
		private int PrivateWidth { get; set; } = 1920;
		private int PrivateHeight { get; set; } = 1080;
		public int WindowHeight { get => PrivateHeight; set { PrivateHeight = value; NotifyPropertyChanged(); } }
		public int WindowWidth { get => PrivateWidth; set { PrivateWidth = value; NotifyPropertyChanged(); } }

		//viewmodels
		public CurrencyTrayViewModel CurrencyTray { get; set; }
		public KoffieMachineViewModel KoffieMachine { get; set; }
		public FriendshipPanelViewmodel Friends { get; set; }

		//pop-up variables
		public bool PopupShow { get; set; }
		public string PopupTitle { get; set; }
		public string PopupDescription { get; set; }

		public ObservableCollection<HeroListObject> HeroList { get; set; }

		//context fields
		public List<Player> CurrentPlayers { get => MpProxy?.GetContext()?.CurrentPlayerList.Where(n => n.UserId != MpProxy?.CurrentPlayer.UserId).ToList(); }
		private Player CurrentPlayer { get { return MultiplayerHubProxy.Instance.CurrentPlayer; } }
		public MultiplayerHubProxy MpProxy { get { return MultiplayerHubProxy.Instance; } }

		private DateTime LastMementoSavedTime { get; set; }

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

		public static void DoSave()
		{
			OnRequestSave?.Invoke(null, null);
		}

		private void LoadHeroes()
		{
			HeroList.Clear();

			foreach (StendenClicker.Library.Models.DatabaseModels.Hero h in Hero.Heroes)
			{
				int levelNeededForUnlock = (h.HeroId * 5);
				bool isLevelUnlocked = CurrentPlayer.State.MonstersDefeated >= levelNeededForUnlock && CurrentPlayer.State.BossesDefeated >= h.HeroId;

				HeroListObject heroListObject;
				PlayerHero heroObject = CurrentPlayer.Heroes?.FirstOrDefault(n => n.Hero.HeroName == h.HeroName);

				if (heroObject != null)
				{
					//hero has been bought, add code that performs a hero level upgrade.
					heroListObject = new HeroListObject { Hero = h, PlayerHeroInformation = heroObject, HeroUnlocked = isLevelUnlocked, NextUpgradePriceSparkCoins = (int)Math.Pow(h.HeroCost * heroObject.HeroUpgradeLevel, 2) };
					heroListObject.OnHeroButtonClicked = new RelayCommand(() =>
					{
						if (PerformTransaction(heroListObject))
							heroListObject.PlayerHeroInformation.HeroUpgradeLevel++;

						UpdateHeroListAndSave();
					});
				}
				else
				{
					//hero has not been bought yet, add code that allows you to buy this hero.
					heroListObject = new HeroListObject { Hero = h, HeroUnlocked = isLevelUnlocked, NextUpgradePriceSparkCoins = h.HeroCost };
					heroListObject.OnHeroButtonClicked = new RelayCommand(() =>
					{
						if (PerformTransaction(heroListObject))
						{
							//create a new heroes object:
							var playerHeroNew = new PlayerHero { Hero = h, HeroUpgradeLevel = 1, SpecialUpgradeLevel = 1 };
							heroListObject.PlayerHeroInformation = playerHeroNew;
							CurrentPlayer.Heroes.Add(playerHeroNew);
						}

						UpdateHeroListAndSave();
						LoadHeroes();
					});
				}

				HeroList.Add(heroListObject); //auto locks all heroes
			}
		}

		private bool PerformTransaction(HeroListObject transactableObject)
		{
			if (CurrentPlayer.Wallet.SparkCoin >= (ulong)transactableObject.NextUpgradePriceSparkCoins)
			{
				CurrentPlayer.Wallet.SparkCoin -= (ulong)transactableObject.NextUpgradePriceSparkCoins;
				NotifyPropertyChanged("CurrencyTray");
				return true;
			}

			return false;
		}

		private void CheckContextVariables()
		{
			//multiplayer connection
			MpProxy.OnConnectionStateChanged += MpProxy_OnConnectionStateChanged;
			MpProxy.OnRequireBatches += MpProxy_OnRequireBatches;
			MpProxy.OnBatchesReceived += MpProxy_OnBatchesReceived;
			MpProxy.InitializeComplete += MpProxy_InitializeComplete;
			MpProxy.OnInviteReceived += MpProxy_OnInviteReceived;
			MpProxy.OnSessionUpdateReceived += MpProxy_OnSessionUpdateReceived;

			//currency tray
			CurrencyTray.OnMonsterDefeated += CurrencyTray_OnMonsterDefeated;

			//memento logic
			OnRequestSave += MainPageViewModel_OnRequestSave;
		}

		private async void MainPageViewModel_OnRequestSave(object sender, EventArgs e)
		{
			await MpProxy.PlayerContext.SetPlayerStateAsync(CurrencyTray.CurrentPlayer);
		}

		private async void CurrencyTray_OnMonsterDefeated(object sender, EventArgs e)
		{
			if (CurrencyTray.CurrentSession.HostPlayerId == CurrentPlayer.UserId.ToString())
			{
				await MpProxy.BroadcastSessionToServer();
			}

			//update the hero list, new heroes may be unlocked or upgraded, also perform a save.
			UpdateHeroListAndSave();
		}

		private void UpdateHeroListAndSave()
		{
			foreach (HeroListObject item in HeroList)
			{
				int levelNeededForUnlock = (item.Hero.Id * 5);
				item.HeroUnlocked = CurrentPlayer.State.MonstersDefeated >= levelNeededForUnlock && CurrentPlayer.State.BossesDefeated >= item.Hero.Id;

				//recalculate price:
				if (item.HeroBought)
				{
					item.NextUpgradePriceSparkCoins = (int)Math.Pow(item.Hero.Price * item.PlayerHeroInformation.HeroUpgradeLevel, 2);
				}
				else
				{
					item.NextUpgradePriceSparkCoins = item.Hero.Price;
				}

				item.NotifyPropertyChanged("HeroUnlocked");
				item.NotifyPropertyChanged("OpacityEnabled");
				item.NotifyPropertyChanged("HeroBought");
				item.NotifyPropertyChanged("NextUpgradePriceSparkCoins");
				item.NotifyPropertyChanged("PlayerHeroInformation");
				item.NotifyPropertyChanged("UpgradePriceEuropeanCredits");
			}

			//perform a save for the memento design pattern.
			if ((DateTime.Now - LastMementoSavedTime).TotalSeconds > 2)
			{
				//doSave
				DoSave();
				LastMementoSavedTime = DateTime.Now;
			}
		}

		private void MpProxy_OnBatchesReceived(object sender, EventArgs e)
		{
			CurrencyTray.ProcessMultiplayerDamage((BatchedClick)sender);
		}

		/// <summary>
		/// Can only process current level and current playerlist, other objects are defaulted.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MpProxy_OnSessionUpdateReceived(object sender, EventArgs e)
		{
			MultiPlayerSession session = (MultiPlayerSession)sender;

			if (session.ForceUpdate)
			{
				CurrencyTray.CurrentSession.CurrentLevel = session.CurrentLevel;
				NotifyPropertyChanged("CurrencyTray");
				CurrencyTray.CurrentSession.CurrentPlayerList = session.CurrentPlayerList;
				NotifyPropertyChanged("CurrentPlayers");
			}

			LoadHeroes();
		}

		private void MpProxy_OnInviteReceived(object sender, EventArgs e)
		{
			Friends.AddPendingInvite((InviteModel)sender);
		}

		private void MpProxy_InitializeComplete(object sender, EventArgs e)
		{
			NotifyPropertyChanged("CurrencyTray");
			Friends.InitializeFriendship(MpProxy.CurrentPlayer.UserId.ToString());

			//render the heroes with player context.
			LoadHeroes();
		}

		private BatchedClick MpProxy_OnRequireBatches()
		{
			return CurrencyTray.GetBatchedClick();
		}

		private async void MpProxy_OnConnectionStateChanged(StateChange state)
		{
			PopupShow = true;

			var dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;
			await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
			{
				switch (state.NewState)
				{
					case ConnectionState.Connected:
						PopupTitle = state.NewState.ToString() + "!";
						PopupDescription = "You are connected! We will now steal your end assesments";
						break;
					case ConnectionState.Connecting:
						PopupTitle = state.NewState.ToString() + "...";
						PopupDescription = "The game is connecting to the online services";
						break;
					case ConnectionState.Disconnected:
						PopupTitle = state.NewState.ToString();
						PopupDescription = "You are disconnected. Maybe you're better of this way...";
						break;
					case ConnectionState.Reconnecting:
						PopupTitle = state.NewState.ToString() + "...";
						PopupDescription = "Holdup... We are reconnecting you to the beautiful NHL-Stenden Servers";
						break;
					default:
						PopupTitle = "Initializing...";
						PopupDescription = "Your super fast internet is making a connection to the online services";
						break;
				}

				NotifyPropertyChanged("PopupShow");
				NotifyPropertyChanged("PopupTitle");
				NotifyPropertyChanged("PopupDescription");
				await Task.Delay(5000);
				PopupShow = false;
				NotifyPropertyChanged("PopupShow");
			});
		}
	}
}
