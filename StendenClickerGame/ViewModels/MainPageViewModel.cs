using Microsoft.AspNet.SignalR.Client;
using StendenClicker.Library;
using StendenClicker.Library.AbstractMonster;
using StendenClicker.Library.Batches;
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
		private static event EventHandler OnRequestSave;

		public static void DoSave()
		{
			OnRequestSave?.Invoke(null, null);
		}

		public MultiplayerHubProxy mpProxy { get { return MultiplayerHubProxy.Instance; } }
		
		private int _width { get; set; } = 1920;
		private int _height { get; set; } = 1080;
		public int WindowHeight { get => _height; set { _height = value; NotifyPropertyChanged(); } }
		public int WindowWidth { get => _width; set { _width = value; NotifyPropertyChanged(); } }

		public CurrencyTrayViewModel CurrencyTray { get; set; }
		public KoffieMachineViewModel KoffieMachine { get; set; }
		public FriendshipPanelViewmodel Friends { get; set; }

		public bool popupShow { get; set; }
		public string popupTitle { get; set; }
		public string popupDescription { get; set; }

		public ObservableCollection<HeroListObject> HeroList { get; set; }

		public List<Player> CurrentPlayers { get => mpProxy?.GetContext()?.CurrentPlayerList.Where(n => n.UserId != mpProxy?.CurrentPlayer.UserId).ToList(); }
		private Player CurrentPlayer { get { return MultiplayerHubProxy.Instance.CurrentPlayer; } }

		private DateTime LastSavedAfterDefeated { get; set; }
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
					heroListObject = new HeroListObject { Hero = h, PlayerHeroInformation = heroObject, HeroUnlocked = isLevelUnlocked, NextUpgradePriceSparkCoins = (int)Math.Pow(h.HeroCost * heroObject.HeroUpgradeLevel, 2)};
					heroListObject.OnHeroButtonClicked = new RelayCommand(() =>
					{
						if (PerformTransaction(heroListObject, "spark"))
							heroListObject.PlayerHeroInformation.HeroUpgradeLevel++;

						UpdateHeroList();
					});

					foreach (var u in h.Upgrades)
					{

					}
					//heroListObject.OnHeroButtonClickSpecialUpgrade = new RelayCommand(() =>
					//{
					//	if (PerformTransaction(heroListObject, "EC"))
					//		heroListObject.PlayerHeroInformation.SpecialUpgradeLevel++;

					//	UpdateHeroList();
					//});
				}
				else
				{
					//hero has not been bought yet, add code that allows you to buy this hero.
					heroListObject = new HeroListObject { Hero = h, HeroUnlocked = isLevelUnlocked, NextUpgradePriceSparkCoins = h.HeroCost };
					heroListObject.OnHeroButtonClicked = new RelayCommand(() =>
					{
						if (PerformTransaction(heroListObject, "spark"))
						{
							//create a new heroes object:
							var playerHeroNew = new PlayerHero { Hero = h, HeroUpgradeLevel = 1, SpecialUpgradeLevel = 1 };
							heroListObject.PlayerHeroInformation = playerHeroNew;
							CurrentPlayer.Heroes.Add(playerHeroNew);						

							
						}
						
						UpdateHeroList();
						LoadHeroes();
					});
				}

				HeroList.Add(heroListObject); //auto locks all heroes
			}
		}

		private bool PerformTransaction(HeroListObject transactableObject, string coin)
		{
			if(CurrentPlayer.Wallet.SparkCoin >= (ulong)transactableObject.NextUpgradePriceSparkCoins && coin == "spark")
			{
				CurrentPlayer.Wallet.SparkCoin -= (ulong)transactableObject.NextUpgradePriceSparkCoins;
				NotifyPropertyChanged("CurrencyTray");
				return true;
			}
			//else if(CurrentPlayer.Wallet.EuropeanCredit >= (ulong)transactableObject.UpgradePriceEuropeanCredits && coin == "EC")
			//{
			//	CurrentPlayer.Wallet.EuropeanCredit -= (ulong)transactableObject.UpgradePriceEuropeanCredits;
			//	NotifyPropertyChanged("CurrencyTray");
			//	return true;
			//}

			return false;
		}

		public void CheckContextVariables()
		{
			//multiplayer connection
			mpProxy.OnConnectionStateChanged += MpProxy_OnConnectionStateChanged;
			mpProxy.OnRequireBatches += MpProxy_OnRequireBatches;
			mpProxy.OnBatchesReceived += MpProxy_OnBatchesReceived;
			mpProxy.InitializeComplete += MpProxy_InitializeComplete;

			mpProxy.OnInviteReceived += MpProxy_OnInviteReceived;
			mpProxy.OnSessionUpdateReceived += MpProxy_OnSessionUpdateReceived;

			CurrencyTray.OnMonsterDefeated += CurrencyTray_OnMonsterDefeated;

			OnRequestSave += MainPageViewModel_OnRequestSave;
		}

		private void MpProxy_OnBatchesReceived(object sender, EventArgs e)
		{
			CurrencyTray.ProcessMultiplayerDamage((BatchedClick)sender);
		}

		private async void MainPageViewModel_OnRequestSave(object sender, EventArgs e)
		{
			await mpProxy.PlayerContext.SetPlayerStateAsync(CurrencyTray.CurrentPlayer);
		}

		private async void CurrencyTray_OnMonsterDefeated(object sender, EventArgs e)
		{
			if((DateTime.Now - LastSavedAfterDefeated).TotalSeconds > 1)
			{
				//doSave
				DoSave();
				LastSavedAfterDefeated = DateTime.Now;
			}

			if(CurrencyTray.CurrentSession.HostPlayerId == CurrentPlayer.UserId.ToString())
			{
				await mpProxy.BroadcastSessionToServer();
			}

			//unlock any new heroes perhaps.
			UpdateHeroList();
		}

		private void UpdateHeroList()
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
			Friends.InitializeFriendship(mpProxy.CurrentPlayer.UserId.ToString());

			//render the heroes with player context.
			LoadHeroes();
		}

		private StendenClicker.Library.Batches.BatchedClick MpProxy_OnRequireBatches()
		{
			return CurrencyTray.GetBatchedClick();
		}

		private async void MpProxy_OnConnectionStateChanged(StateChange state)
		{
			popupShow = true;

			var dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;
			await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
			{
				switch (state.NewState)
				{
					case ConnectionState.Connected:
						popupTitle = state.NewState.ToString() + "!";
						popupDescription = "You are connected! We will now steal your end assesments";
						break;
					case ConnectionState.Connecting:
						popupTitle = state.NewState.ToString() + "...";
						popupDescription = "The game is connecting to the online services";
						break;
					case ConnectionState.Disconnected:
						popupTitle = state.NewState.ToString();
						popupDescription = "You are disconnected. Maybe you're better of this way...";
						break;
					case ConnectionState.Reconnecting:
						popupTitle = state.NewState.ToString() + "...";
						popupDescription = "Holdup... We are reconnecting you to the beautiful NHL-Stenden Servers";
						break;
					default:
						popupTitle = "Initializing...";
						popupDescription = "Your super fast internet is making a connection to the online services";
						break;
				}

				NotifyPropertyChanged("popupShow");
				NotifyPropertyChanged("popupTitle");
				NotifyPropertyChanged("popupDescription");
				await Task.Delay(5000);
				popupShow = false;
				NotifyPropertyChanged("popupShow");

			});
		}

		public async Task<Player> GetPlayerContextAsync()
		{
			return await mpProxy.PlayerContext.GetPlayerStateAsync(DeviceInfo.Instance.GetSystemId());
		}

	}
}
