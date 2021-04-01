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

namespace StendenClickerGame.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
		public MultiplayerHubProxy mpProxy { get { return MultiplayerHubProxy.Instance; } }

		public CurrencyTrayViewModel CurrencyTray { get; set; }
		public KoffieMachineViewModel KoffieMachine { get; set; }
		public FriendshipPanelViewmodel Friends { get; set; }

		public ObservableCollection<Hero> HeroList { get; set; }

		public List<Player> CurrentPlayers { get => mpProxy?.getContext()?.CurrentPlayerList.Where(n => n.UserId != mpProxy?.CurrentPlayer.UserId).ToList(); }

		public MainPageViewModel()
		{
			//sub viewmodels
			CurrencyTray = new CurrencyTrayViewModel();
			KoffieMachine = new KoffieMachineViewModel();
			Friends = new FriendshipPanelViewmodel();

			//Herolist
			HeroList = new ObservableCollection<Hero>();

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
			mpProxy.InitializeComplete += MpProxy_InitializeComplete;

			mpProxy.OnInviteReceived += MpProxy_OnInviteReceived;
			mpProxy.OnSessionUpdateReceived += MpProxy_OnSessionUpdateReceived;
		}

		/// <summary>
		/// Can only process current level and current playerlist, other objects are defaulted.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MpProxy_OnSessionUpdateReceived(object sender, EventArgs e)
		{
			MultiPlayerSession session = (MultiPlayerSession)sender;

			CurrencyTray.CurrentSession.CurrentLevel = session.CurrentLevel;
			CurrencyTray.CurrentSession.CurrentPlayerList = session.CurrentPlayerList;

			NotifyPropertyChanged("CurrentPlayers");
			NotifyPropertyChanged("CurrencyTray");
		}

		private void MpProxy_OnInviteReceived(object sender, EventArgs e)
		{
			Friends.AddPendingInvite((InviteModel)sender);
		}

		private async void MpProxy_InitializeComplete(object sender, EventArgs e)
		{
			NotifyPropertyChanged("CurrencyTray");
			Friends.InitializeFriendship(mpProxy.CurrentPlayer.UserId.ToString());

			//Sync session to server
			//await mpProxy.BroadcastSessionToServer();
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
